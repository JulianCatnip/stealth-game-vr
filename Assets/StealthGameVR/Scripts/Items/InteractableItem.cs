using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableItem : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public int defaultLayer = 0;
    public int socketLayer = 7; // darf nur mit Hand kollidieren wenn ausgerüstet
    public int grabbedItemsLayer = 3; // darf nicht mit Körper kollidieren wenn in der Hand
    private uint hapticChannel = 0;
    private float hapticAmplitude = 0.5f;
    private float hapticDuration = 0.3f;
    private HandController handController;
    private Transform prevParent;

    protected void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.hoverEntered.AddListener(HoverEntered);
        grabInteractable.hoverExited.AddListener(HoverExited);
        grabInteractable.selectEntered.AddListener(SelectEntered);
        grabInteractable.selectExited.AddListener(SelectExited);
    }

    protected void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(HoverEntered);
        grabInteractable.hoverExited.RemoveListener(HoverExited);
        grabInteractable.selectEntered.RemoveListener(SelectEntered);
        grabInteractable.selectExited.RemoveListener(SelectExited);   
    }

    protected virtual void HoverEntered(HoverEnterEventArgs args)
    {
        // if the interactor is a socket
        if(args.interactor.CompareTag("Socket") && args.interactor.CanSelect(args.interactable))
        {
            // if the current selecting interactor of the interactable is a hand
            if(args.interactable.selectingInteractor.CompareTag("Hand"))
            {
                // get the hand controller
                handController = args.interactable.selectingInteractor.GetComponent<HandController>();
                // initiate haptic feedback
                handController.GetController().SendHapticImpulse(hapticChannel, hapticAmplitude, hapticDuration);
            }
        // if the interactor is a hand
        } else if(args.interactor.CompareTag("Hand"))
        {
            // if the current selecting interactor of the interactable is a socket
            if(args.interactable.selectingInteractor != null && args.interactable.selectingInteractor.CompareTag("Socket"))
            {
                // get the hand controller
                handController = args.interactor.GetComponent<HandController>();
                // initiate haptic feedback
                handController.GetController().SendHapticImpulse(hapticChannel, hapticAmplitude, hapticDuration);
            }
        }

        //Debug.Log("Hover entered.");
        //Debug.Log(args.interactable + ", " + args.interactor);
    }

    protected virtual void HoverExited(HoverExitEventArgs args)
    {
        // Debug.Log("Hover exited.");
    }

    protected virtual void SelectEntered(SelectEnterEventArgs args)
    {   
        // Add item to an appropriate a layer
        if(args.interactor.CompareTag("Socket")){
            gameObject.layer = socketLayer;
            if(gameObject.transform.childCount > 0){
                foreach(Transform child in gameObject.transform){
                    child.gameObject.layer = socketLayer;
                }
            }
        } else { // the interactor is a hand
            gameObject.layer = grabbedItemsLayer;
            if(gameObject.transform.childCount > 0){
                foreach(Transform child in gameObject.transform){
                    child.gameObject.layer = grabbedItemsLayer;
                }
            }

        }

        // Set up fitting hand attachment/pivot point
        handController = args.interactor.GetComponent<HandController>();
        if(args.interactor.CompareTag("Hand")){
            if(handController != null){
                if(handController.handType == HandController.Hand.Left){
                    if(gameObject.transform.Find("GrabPointL") != null) {
                        gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("GrabPointL");
                    } else if(gameObject.transform.Find("PivotPoint") != null) {
                        gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("PivotPoint");
                    }
                } else {
                    if(gameObject.transform.Find("GrabPointR") != null) {
                        gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("GrabPointR");
                    } else if(gameObject.transform.Find("PivotPoint") != null) {
                        gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("PivotPoint");
                    }
                }
            }
            // give haptic feedback on controller
            handController.GetController().SendHapticImpulse(hapticChannel, hapticAmplitude, hapticDuration);
        } else {
            if(gameObject.transform.Find("PivotPoint") != null) {
                gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("PivotPoint");
            }
        }

        // Remember previous parent in scene hierarchy and set new parent
        this.prevParent = gameObject.transform.parent;
        gameObject.transform.SetParent(grabInteractable.selectingInteractor.attachTransform);
        
        // Debug.Log("InteractableItem OnSelectEntered");
    }

    protected virtual void SelectExited(SelectExitEventArgs args)
    {
        // recover previous parent in scene hierarchy
        if(args.interactor.attachTransform.gameObject.activeInHierarchy == true) // attach must be active
        {
            gameObject.transform.SetParent(this.prevParent);
        }

        // recover previous layer
        gameObject.layer = defaultLayer;
        if(gameObject.transform.childCount > 0){
            foreach(Transform child in gameObject.transform){
                child.gameObject.layer = defaultLayer;
            }
        }
        
        // Debug.Log("InteractableItem OnSelectExited");
    }
}
