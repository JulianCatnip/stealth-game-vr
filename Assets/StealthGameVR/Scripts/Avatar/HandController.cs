using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    public Animator avatarAnimator;
    public float animationFadeDuration = 0.05f;
    
    [System.Serializable]
    public enum Hand {
        Left,
        Right
    }
    public Hand handType;

    // private ActionBasedController controller;
    private InputDevice controller;
    private string output;
    private int lHandIndex;
    private int rHandIndex;
    private bool triggerPressed;
    private bool gripPressed;
    private bool thumbTouched;

    void Start()
    {
        // xr controller input
        if(handType == Hand.Left){
            controller = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        } else if(handType == Hand.Right) {
            controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        } else {
            Debug.Log("Hand not assigned.");
        }

        // controller = GetComponent<ActionBasedController>();
        triggerPressed = false;
        gripPressed = false;
        thumbTouched = false;
        
        // animator controller
        lHandIndex = avatarAnimator.GetLayerIndex("Hand L");
        rHandIndex = avatarAnimator.GetLayerIndex("Hand R");
        avatarAnimator.SetLayerWeight(lHandIndex, 1f);
        avatarAnimator.SetLayerWeight(rHandIndex, 1f);
    }

    private void GetInput()
    {
        controller.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out thumbTouched); // thumbstick touch
        controller.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed); // left trigger
        controller.TryGetFeatureValue(CommonUsages.gripButton, out gripPressed); // left grip
    }

    // Update is called once per frame
    void Update()
    {
        // Hand animation pseudocode
        // if triggerButton pressed then "Okey Dokey"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Okey Dokey", 0.1f);
        // if gripButton pressed "Point 2"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Point 2", 0.1f);
        // if ThumbTouch then "Peace"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Peace", 0.1f);
        // if triggerButton and gripButton pressed then "Thumbs Up"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Thumbs Up", 0.1f);
        // if gripButton and ThumbTouch then "Point 2"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Point 2", 0.1f);
        // if triggerButton and gripButton and ThumbTouch then "Grab"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Grab", 0.1f);
        // else "Idle"
            // animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1f);
            // animator.CrossFade ("Idle", 0.1f);
        
        GetInput();

        if (triggerPressed && gripPressed && thumbTouched) {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Grab", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Grab", animationFadeDuration, rHandIndex);
                    break;
            }
        } else if (triggerPressed && gripPressed) {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Thumbs Up", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Thumbs Up", animationFadeDuration, rHandIndex);
                    break;
            }
        } else if (gripPressed && thumbTouched) {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Point 1", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Point 1", animationFadeDuration, rHandIndex);
                    break;
            }
        } else if(triggerPressed) {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Okey Dokey", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Okey Dokey", animationFadeDuration, rHandIndex);
                    break;
            }
        } else if (gripPressed) {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Point 2", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Point 2", animationFadeDuration, rHandIndex);
                    break;
            }
        } else {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Basic", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Basic", animationFadeDuration, rHandIndex);
                    break;
            }
        }
    }
}
