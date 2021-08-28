using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class UIInteractor : MonoBehaviour, ILineRenderable, IUIInteractor
    {
        protected sealed class RaycastHitComparer : IComparer<RaycastHit>
        {
            public int Compare(RaycastHit a, RaycastHit b)
            {
                var aDistance = a.collider != null ? a.distance : float.MaxValue;
                var bDistance = b.collider != null ? b.distance : float.MaxValue;
                return aDistance.CompareTo(bDistance);
            }
        }

        private XRDirectInteractor xrInteractor;

        const int maxRaycastHits = 10;

        public enum LineType
        {
            StraightLine
        }

        private LineType m_LineType = LineType.StraightLine;
        public bool blendVisualLinePoints = true;
        public float maxRaycastDistance = 30f;
        private Transform referenceFrame;
        private int sampleFrequency = 2;
        public LayerMask raycastMask = -1;
        public QueryTriggerInteraction raycastTriggerInteraction = QueryTriggerInteraction.Ignore;
        private bool enableUIInteraction = true;
        private Transform originalAttachTransform;

        Transform startTransform => originalAttachTransform != null ? originalAttachTransform : transform;

        int closestAnyHitIndex => (m_RaycastHitEndpointIndex > 0 && m_UIRaycastHitEndpointIndex > 0) // Are both valid?
            ? Mathf.Min(m_RaycastHitEndpointIndex, m_UIRaycastHitEndpointIndex) // When both are valid, return the closer one
            : (m_RaycastHitEndpointIndex > 0 ? m_RaycastHitEndpointIndex : m_UIRaycastHitEndpointIndex); // Otherwise return the valid one

        XRUIInputModule inputModule;
        XRUIInputModule m_RegisteredInputModule;

        readonly RaycastHit[] m_RaycastHits = new RaycastHit[maxRaycastHits];
        int m_RaycastHitsCount;
        readonly RaycastHitComparer m_RaycastHitComparer = new RaycastHitComparer();

        List<SamplePoint> m_SamplePoints;
        int m_SamplePointsFrameUpdated = -1;
        int m_RaycastHitEndpointIndex;

        /// <summary>
        /// The index of the sample endpoint if a UI hit occurred. Otherwise, a value of <c>0</c> if no hit occurred.
        /// </summary>
        int m_UIRaycastHitEndpointIndex;

        static List<SamplePoint> s_ScratchSamplePoints;

        protected void OnValidate()
        {
            RegisterOrUnregisterXRUIInputModule();
        }

        protected void Awake()
        {
            xrInteractor = GetComponent<XRDirectInteractor>();

            var capacity = m_LineType == LineType.StraightLine ? 2 : sampleFrequency;
            m_SamplePoints = new List<SamplePoint>(capacity);
            if (s_ScratchSamplePoints == null)
                s_ScratchSamplePoints = new List<SamplePoint>(capacity);

            FindReferenceFrame();
        }

        protected  void OnEnable()
        {
            if (enableUIInteraction)
                RegisterWithXRUIInputModule();
            

        }

        protected void OnDisable()
        {
            // Clear lines
            m_SamplePoints.Clear();

            if (enableUIInteraction)
                UnregisterFromXRUIInputModule();
        }

        void FindReferenceFrame()
        {   
            var xrRig = FindObjectOfType<XRRig>();
            if (xrRig != null)
            {
                var rig = xrRig.rig;
                if (rig != null)
                {
                    referenceFrame = rig.transform;
                }
                else
                {
                    Debug.Log($"Reference frame of the curve not set and {nameof(XRRig)}.{nameof(XRRig.rig)} is not set, using global up as default.", this);
                }
            }
            else
            {
                Debug.Log($"Reference frame of the curve not set and {nameof(XRRig)} is not found, using global up as default.", this);
            }
        }

        void FindOrCreateXRUIInputModule()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
                eventSystem = new GameObject("EventSystem", typeof(EventSystem)).GetComponent<EventSystem>();
            else
            {
                // Remove the Standalone Input Module if already implemented, since it will block the XRUIInputModule
                var standaloneInputModule = eventSystem.GetComponent<StandaloneInputModule>();
                if (standaloneInputModule != null)
                    Destroy(standaloneInputModule);
            }

            inputModule = eventSystem.GetComponent<XRUIInputModule>();
            if (inputModule == null)
                inputModule = eventSystem.gameObject.AddComponent<XRUIInputModule>();
        }

        /// <summary>
        /// Register with the <see cref="XRUIInputModule"/> (if necessary).
        /// </summary>
        /// <seealso cref="UnregisterFromXRUIInputModule"/>
        void RegisterWithXRUIInputModule()
        {
            if (inputModule == null)
                FindOrCreateXRUIInputModule();

            if (m_RegisteredInputModule == inputModule)
                return;

            UnregisterFromXRUIInputModule();

            inputModule.RegisterInteractor(this);
            m_RegisteredInputModule = inputModule;
        }

        /// <summary>
        /// Unregister from the <see cref="XRUIInputModule"/> (if necessary).
        /// </summary>
        /// <seealso cref="RegisterWithXRUIInputModule"/>
        void UnregisterFromXRUIInputModule()
        {
            if (m_RegisteredInputModule != null)
                m_RegisteredInputModule.UnregisterInteractor(this);

            m_RegisteredInputModule = null;
        }

        /// <summary>
        /// Register with or unregister from the Input Module (if necessary).
        /// </summary>
        /// <remarks>
        /// If this behavior is not active and enabled, this function does nothing.
        /// </remarks>
        void RegisterOrUnregisterXRUIInputModule()
        {
            if (!isActiveAndEnabled || !Application.isPlaying)
                return;

            if (enableUIInteraction)
                RegisterWithXRUIInputModule();
            else
                UnregisterFromXRUIInputModule();
        }

        public bool GetLinePoints(ref Vector3[] linePoints, out int numPoints)
        {
            if (m_SamplePoints == null || m_SamplePoints.Count < 2)
            {
                numPoints = default;
                return false;
            }

            if (!blendVisualLinePoints)
            {
                numPoints = m_SamplePoints.Count;
                EnsureCapacity(ref linePoints, numPoints);

                for (var i = 0; i < numPoints; ++i)
                    linePoints[i] = m_SamplePoints[i].position;

                return true;
            }

            // Because this method may be invoked during OnBeforeRender, the current positions
            // of sample points may be different as the controller moves. Recompute the current
            // positions of sample points.
            UpdateSamplePoints(m_SamplePoints.Count, s_ScratchSamplePoints);

            if (m_LineType == LineType.StraightLine)
            {
                numPoints = 2;
                EnsureCapacity(ref linePoints, numPoints);

                linePoints[0] = s_ScratchSamplePoints[0].position;
                linePoints[1] = m_SamplePoints[m_SamplePoints.Count - 1].position;

                return true;
            }

            // Recompute the equivalent Bezier curve.
            var hitIndex = closestAnyHitIndex;

            numPoints = sampleFrequency;
            EnsureCapacity(ref linePoints, numPoints);

            return true;
        }

        static void EnsureCapacity(ref Vector3[] linePoints, int numPoints)
        {
            if (linePoints == null || linePoints.Length < numPoints)
                linePoints = new Vector3[numPoints];
        }

        public bool TryGetHitInfo(out Vector3 position, out Vector3 normal, out int positionInLine, out bool isValidTarget)
        {
            position = default;
            normal = default;
            positionInLine = default;
            isValidTarget = default;

            if (!TryGetCurrentRaycast(
                out var raycastHit,
                out var raycastHitIndex,
                out var raycastResult,
                out var raycastResultIndex,
                out var isUIHitClosest))
            {
                return false;
            }

            if (raycastResult.HasValue && isUIHitClosest)
            {
                position = raycastResult.Value.worldPosition;
                normal = raycastResult.Value.worldNormal;
                positionInLine = raycastResultIndex;

                isValidTarget = raycastResult.Value.gameObject != null;
            }
            else if (raycastHit.HasValue)
            {
                position = raycastHit.Value.point;
                normal = raycastHit.Value.normal;
                positionInLine = raycastHitIndex;
            }

            return true;
        }

        public virtual void UpdateUIModel(ref TrackedDeviceModel model)
        {
            if (!isActiveAndEnabled || m_SamplePoints == null)
                return;

            model.position = startTransform.position;
            model.orientation = startTransform.rotation;
            if(GetComponent<HandController>().GetTriggerButtonState())
            {
                model.select = true;
            } 
            else
            {
                model.select = false;
            }
            model.raycastLayerMask = raycastMask;

            var raycastPoints = model.raycastPoints;
            raycastPoints.Clear();

            // Update curve approximation used for raycasts.
            // This method will be called before ProcessInteractor.
            UpdateSamplePoints(sampleFrequency, m_SamplePoints);
            m_SamplePointsFrameUpdated = Time.frameCount;

            var numPoints = m_SamplePoints.Count;
            if (numPoints > 0)
            {
                if (raycastPoints.Capacity < numPoints)
                    raycastPoints.Capacity = numPoints;

                for (var i = 0; i < numPoints; ++i)
                    raycastPoints.Add(m_SamplePoints[i].position);
            }
        }

        public bool TryGetUIModel(out TrackedDeviceModel model)
        {
            if (inputModule != null)
            {
                return inputModule.GetTrackedDeviceModel(this, out model);
            }

            model = new TrackedDeviceModel(-1);
            return false;
        }

        public bool TryGetCurrent3DRaycastHit(out RaycastHit raycastHit)
        {
            return TryGetCurrent3DRaycastHit(out raycastHit, out _);
        }

        public bool TryGetCurrent3DRaycastHit(out RaycastHit raycastHit, out int raycastEndpointIndex)
        {
            if (m_RaycastHitsCount > 0)
            {
                Assert.IsTrue(m_RaycastHits.Length >= m_RaycastHitsCount);
                raycastHit = m_RaycastHits[0];
                raycastEndpointIndex = m_RaycastHitEndpointIndex;
                return true;
            }

            raycastHit = default;
            raycastEndpointIndex = default;
            return false;
        }

        public bool TryGetCurrentUIRaycastResult(out RaycastResult raycastResult)
        {
            return TryGetCurrentUIRaycastResult(out raycastResult, out _);
        }

        public bool TryGetCurrentUIRaycastResult(out RaycastResult raycastResult, out int raycastEndpointIndex)
        {
            if (TryGetUIModel(out var model) && model.currentRaycast.isValid)
            {
                raycastResult = model.currentRaycast;
                raycastEndpointIndex = model.currentRaycastEndpointIndex;
                
                return true;
            }

            raycastResult = default;
            raycastEndpointIndex = default;
            return false;
        }

        public bool TryGetCurrentRaycast(
            out RaycastHit? raycastHit,
            out int raycastHitIndex,
            out RaycastResult? uiRaycastHit,
            out int uiRaycastHitIndex,
            out bool isUIHitClosest)
        {
            raycastHit = default;
            uiRaycastHit = default;
            isUIHitClosest = default;

            var hitOccurred = false;

            var hitIndex = int.MaxValue;
            var distance = float.MaxValue;
            if (TryGetCurrent3DRaycastHit(out var raycastHitValue, out raycastHitIndex))
            {
                raycastHit = raycastHitValue;
                hitIndex = raycastHitIndex;
                distance = raycastHitValue.distance;

                hitOccurred = true;
            }

            if (TryGetCurrentUIRaycastResult(out var raycastResultValue, out uiRaycastHitIndex))
            {
                uiRaycastHit = raycastResultValue;

                // Determine if the UI hit is closer than the 3D hit.
                // The Raycast segments are sourced from a polygonal chain of endpoints.
                // Within each segment, this Interactor could have hit either a 3D object or a UI object.
                // The distance is just from the segment start position, not from the origin of the whole curve.
                isUIHitClosest = uiRaycastHitIndex > 0 &&
                    (uiRaycastHitIndex < hitIndex || (uiRaycastHitIndex == hitIndex && raycastResultValue.distance <= distance));

                hitOccurred = true;
            }

            return hitOccurred;
        }

        void UpdateSamplePoints(int count, List<SamplePoint> samplePoints)
        {
            Assert.IsTrue(count >= 2);

            samplePoints.Clear();
            var samplePoint = new SamplePoint
            {
                position = startTransform.position,
                parameter = 0f,
            };
            samplePoints.Add(samplePoint);

            samplePoint.position = samplePoints[0].position + startTransform.forward * maxRaycastDistance;
            samplePoint.parameter = 1f;
            samplePoints.Add(samplePoint);
        }

        void UpdateUIHitIndex()
        {
            TryGetCurrentUIRaycastResult(out _, out m_UIRaycastHitEndpointIndex);
        }

        struct SamplePoint
        {
            public Vector3 position { get; set; }
            public float parameter { get; set; }
        }
    }
}