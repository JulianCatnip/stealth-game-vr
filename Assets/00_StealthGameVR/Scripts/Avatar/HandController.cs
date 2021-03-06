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
    private XRDirectInteractor directInteractor;
    private InputDevice controller;
    private string output;
    private int lHandIndex;
    private int rHandIndex;

    private bool primaryButtonPressed;
    private Vector2 thumbstickUsed;
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

        directInteractor = GetComponent<XRDirectInteractor>();

        primaryButtonPressed = false;
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
        controller.TryGetFeatureValue(CommonUsages.primaryButton, out this.primaryButtonPressed); // primary button touch
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out this.thumbstickUsed); // thumbstick use
        controller.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out this.thumbTouched); // thumbstick touch
        controller.TryGetFeatureValue(CommonUsages.triggerButton, out this.triggerPressed); // left trigger
        controller.TryGetFeatureValue(CommonUsages.gripButton, out this.gripPressed); // left grip
    }

    // Update is called once per frame
    void Update()
    {   
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

    public InputDevice GetController()
    {
        return this.controller;
    }

    public bool GetPrimaryButtonState()
    {
        return this.primaryButtonPressed;
    }

    public bool GetGripButtonState()
    {
        return this.gripPressed;
    }

    public bool GetTriggerButtonState()
    {
        return this.triggerPressed;
    }

    public bool GetThumbstickUseState()
    {
        if(this.thumbstickUsed.magnitude > 0)
        {
            return true;
        } else
        {
             return false;
        }
    }
}
