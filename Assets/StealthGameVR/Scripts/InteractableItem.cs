using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableItem : MonoBehaviour
{
    private XRGrabInteractable m_GrabInteractable;
    private int defaultLayer = 0;
    private int grabbedItemsLayer = 3;
    public GameObject lGrabPoint;
    public GameObject rGrabPoint;
    private HandController handController;

    // private bool m_Held;

    protected void OnEnable()
    {
        m_GrabInteractable = GetComponent<XRGrabInteractable>();
        
        m_GrabInteractable.selectEntered.AddListener(OnSelectEntered);
        m_GrabInteractable.selectExited.AddListener(OnSelectExited);
    }

    protected void OnDisable()
    {
        m_GrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        m_GrabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args)
    {
        //m_Held = true;
        gameObject.layer = grabbedItemsLayer;
        handController = args.interactor.gameObject.GetComponent<HandController>();
        if(handController.handType == HandController.Hand.Left){
            gameObject.transform.SetParent(lGrabPoint.transform);
            m_GrabInteractable.attachTransform = lGrabPoint.transform;
        } else {
            gameObject.transform.SetParent(rGrabPoint.transform);
            m_GrabInteractable.attachTransform = rGrabPoint.transform;
        }

        //gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //Debug.Log("Entered");
    }

    protected virtual void OnSelectExited(SelectExitEventArgs args)
    {
        //m_Held = false;
        gameObject.layer = defaultLayer;
        gameObject.transform.SetParent(null);
        //Debug.Log("Exited");
    }
}
