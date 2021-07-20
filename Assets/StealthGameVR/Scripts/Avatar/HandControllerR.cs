using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControllerR : MonoBehaviour
{
    ActionBasedController controller;
    public Hand hand;

    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        // hand.SetGripL(controller.selectAction.action.ReadValue<float>());
        hand.SetGripR(controller.selectAction.action.ReadValue<float>());
        //hand.SetTriggerL(controller.activateAction.action.ReadValue<float>());
        hand.SetTriggerR(controller.activateAction.action.ReadValue<float>());
    }
}
