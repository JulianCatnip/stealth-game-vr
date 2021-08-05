using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableItem : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private int defaultLayer = 0;
    private int grabbedItemsLayer = 3;
    private HandController handController;

    // private bool m_Held;

    protected void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    protected void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args)
    {
        //m_Held = true;
        handController = grabInteractable.selectingInteractor.GetComponent<HandController>();
        gameObject.layer = grabbedItemsLayer;

        if(handController.handType == HandController.Hand.Left){
            if(gameObject.transform.Find("GrabPointL") != null) {
                gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("GrabPointL");
            }
        } else {
            if(gameObject.transform.Find("GrabPointR") != null) {
                gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("GrabPointR");
            }
        }

        gameObject.transform.SetParent(grabInteractable.selectingInteractor.attachTransform);
    }

    protected virtual void OnSelectExited(SelectExitEventArgs args)
    {
        //m_Held = false;
        gameObject.layer = defaultLayer;
    }
}
