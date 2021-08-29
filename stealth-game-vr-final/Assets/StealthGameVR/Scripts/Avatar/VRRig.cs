using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap 
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        // rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.position = Vector3.Lerp(rigTarget.position, vrTarget.TransformPoint(trackingPositionOffset), 1 - Mathf.Exp(-50.0f * Time.deltaTime));
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    
    public float turnSmoothness;
    public float moveSmoothness = 50.0f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstraint;
    private Vector3 headBodyOffset;

    private Vector3 pastRigPosition, pastVRPosition;

    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    void FixedUpdate()
    {
        // transform.position = headConstraint.position + headBodyOffset; // original
        transform.position = Vector3.Lerp(transform.position, headConstraint.position + headBodyOffset, 1 - Mathf.Exp(-moveSmoothness * Time.deltaTime));

        //transform.position = SmoothApproach(pastRigPosition, pastVRPosition, headConstraint.position + headBodyOffset, 20f); // this ?
        //pastFollowerPosition = followerTransform.position;
        // pastTargetPosition = targetTransform.position;

        transform.forward = Vector3.Lerp(transform.forward, 
            Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    Vector3 SmoothApproach( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float speed )
    {
        float t = Time.deltaTime * speed;
        Vector3 v = ( targetPosition - pastTargetPosition ) / t;
        Vector3 f = pastPosition - pastTargetPosition + v;
        return targetPosition - v + f * Mathf.Exp( -t );
    }
}
