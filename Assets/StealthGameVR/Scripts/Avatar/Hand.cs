using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    // Animation
    public float animationSpeed;
    Animator animator;
    private float gripTargetL;
    private float gripTargetR;
    private float triggerTargetL;
    private float triggerTargetR;
    private float gripCurrentL;
    private float gripCurrentR;
    private float triggerCurrentL;
    private float triggerCurrentR;

    // // private SkinnedMeshRenderer _mesh;
    private string animatorGripLParam = "gripL";
    private string animatorGripRParam = "gripR";
    private string animatorTriggerLParam = "triggerL";
    private string animatorTriggerRParam = "triggerR";
    // private static readonly int gripL = Animator.StringToHash(animatorGripLParam);
    // private static readonly int gripR = Animator.StringToHash(animatorGripRParam);
    // private static readonly int triggerL = Animator.StringToHash(animatorTriggerLParam);
    // private static readonly int triggerR = Animator.StringToHash(animatorTriggerRParam);

    // Physics movement
    // [SerializeField] private GameObject followObject;
    // [SerializeField] private float followSpeed = 30f;
    // [SerializeField] private float rotateSpeed = 100f;
    // [SerializeField] private Vector3 positionOffset;
    // [SerializeField] private Vector3 rotationOffset;
    // private Transform _followtarget;
    // private Rigidbody _body;
    
    void Start()
    {
        // Animation
        animator = GetComponent<Animator>();
        // _mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        // Physics movement
        // _followtarget = followObject.transform;
        // _body = GetComponent<Rigidbody>();
        // _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // _body.interpolation = RigidbodyInterpolation.Interpolate;
        // _body.mass = 20f;

        // // // Teleport hands
        // _body.position = _followtarget.position;
        // _body.rotation = _followtarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
        // PhysicsMove();
    }

    private void PhysicsMove()
    {
        // Position
        // var positionWithOffset = _followtarget.position + positionOffset;
        // var positionWithOffset = _followtarget.TransformPoint(positionOffset);
        // var distance = Vector3.Distance(positionWithOffset, transform.position);
        // _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        // // Rotation
        // var rotationWithOffset = _followtarget.rotation * Quaternion.Euler(rotationOffset);
        // var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        // q.ToAngleAxis(out float angle, out Vector3 axis);
        // _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }

    internal void SetGripL(float v)
    {
        gripTargetL = v;
        if(v > 0.01)
        {
            Debug.Log("GripL pressed.");
        }
    }

    internal void SetGripR(float v)
    {
        gripTargetR = v;
        if(v > 0.01)
        {
            Debug.Log("GripR pressed.");
        }
    }

    internal void SetTriggerL(float v)
    {
        triggerTargetL = v;
        if(v > 0.01)
        {
            Debug.Log("TriggerL pressed.");
        }
    }

    internal void SetTriggerR(float v)
    {
        triggerTargetR = v;
        if(v > 0.01)
        {
            Debug.Log("TriggerR pressed.");
        }
    }

    void AnimateHand()
    {
        if(gripCurrentL != gripTargetL)
        {
            gripCurrentL = Mathf.MoveTowards(gripCurrentL, gripTargetL, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorGripLParam, gripCurrentL);
        }

        if(gripCurrentR != gripTargetR)
        {
            gripCurrentR = Mathf.MoveTowards(gripCurrentR, gripTargetR, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorGripRParam, gripCurrentR);
        }

        if(triggerCurrentL != triggerTargetL)
        {
            triggerCurrentL = Mathf.MoveTowards(triggerCurrentL, triggerTargetL, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorTriggerLParam, triggerCurrentL);
        }

        if(triggerCurrentR != triggerTargetR)
        {
            triggerCurrentR = Mathf.MoveTowards(triggerCurrentR, triggerTargetR, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorTriggerRParam, triggerCurrentR);
        }


        // if(Mathf.Abs(gripCurrent - gripTarget) > 0)
        // {
        //     gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed); 
        //     animator.SetFloat(Grip, gripCurrent);
        // }

        // if(!(Mathf.Abs(triggerCurrent - triggerTarget) > 0)) return;
        
        // triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
        // animator.SetFloat(QueryTriggerInteraction, triggerCurrent);

    }

    // public void ToggleVisibility()
    // {
    //     mesh.enabled = !_mesh.enabled;
    // }
}
