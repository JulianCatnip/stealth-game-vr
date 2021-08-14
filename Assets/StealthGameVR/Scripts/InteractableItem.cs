using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableItem : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public int defaultLayer = 0;
    public int socketLayer = 7; // darf nur mit Hand kollidieren wenn ausgerüstet
    public int grabbedItemsLayer = 3; // darf nicht mit Körper kollidieren wenn in der Hand
    private HandController handController;
    private Transform prevParent;

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
        // Add item to an appropriate a layer
        if(args.interactor.CompareTag("Socket")){
            gameObject.layer = socketLayer;
            if(gameObject.transform.childCount > 0){
                foreach(Transform child in gameObject.transform){
                    child.gameObject.layer = socketLayer;
                }
            }
        } else {
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

    protected virtual void OnSelectExited(SelectExitEventArgs args)
    {
        // recover previous parent in scene hierarchy
        gameObject.transform.SetParent(this.prevParent);

        // recover previous layer
        gameObject.layer = defaultLayer;
        if(gameObject.transform.childCount > 0){
            foreach(Transform child in gameObject.transform){
                child.gameObject.layer = defaultLayer;
            }
        }

        /* if(gameObject.CompareTag("Stone")){
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }*/
        
        // Debug.Log("InteractableItem OnSelectExited");
    }
}
