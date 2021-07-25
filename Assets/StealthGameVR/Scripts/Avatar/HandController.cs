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

    private XRController xrController;
    private string output;
    private int lHandIndex;
    private int rHandIndex;
    private bool triggerPressed;
    private bool gripPressed;
    private bool thumbTouched;

    void Start()
    {
        xrController = GetComponent<XRController>();
        lHandIndex = avatarAnimator.GetLayerIndex("Hand L");
        rHandIndex = avatarAnimator.GetLayerIndex("Hand R");
        avatarAnimator.SetLayerWeight(lHandIndex, 1f);
        avatarAnimator.SetLayerWeight(rHandIndex, 1f);
        triggerPressed = false;
        gripPressed = false;
        thumbTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Hand animation
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

        xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out triggerPressed);
        xrController.inputDevice.IsPressed(InputHelpers.Button.Grip, out gripPressed);
        xrController.inputDevice.IsPressed(InputHelpers.Button.Primary2DAxisTouch, out thumbTouched);

        if (triggerPressed && gripPressed && thumbTouched) {
            switch (handType) {
                case Hand.Left:
                    avatarAnimator.CrossFade("Hold 2", animationFadeDuration, lHandIndex);
                    break;
                case Hand.Right:
                    avatarAnimator.CrossFade("Hold 2", animationFadeDuration, rHandIndex);
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
        // } else if(thumbTouched) {
        //     switch (handType) {
        //         case Hand.Left:
        //             avatarAnimator.CrossFade("Peace", animationFadeDuration, lHandIndex);
        //             break;
        //         case Hand.Right:
        //             avatarAnimator.CrossFade("Peace", animationFadeDuration, rHandIndex);
        //             break;
        //     }
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
