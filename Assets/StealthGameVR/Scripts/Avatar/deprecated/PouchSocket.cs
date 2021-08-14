using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PouchSocket : XRSocketInteractor
{
    public int nStones; // Anzahl abgelegter Steine
    public Material hoverMaterial;
    private Material standardMaterial;
    private int nChild;

    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag("Stone"); // nur Steine gehören in diesen Sockel
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag("Stone"); // nur Steine gehören in diesen Sockel
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.hoverEntered.AddListener(HoverEntered);
        this.hoverExited.AddListener(HoverExited);
        this.selectEntered.AddListener(SelectEntered);
        this.selectExited.AddListener(SelectExited);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.hoverEntered.RemoveListener(HoverEntered);
        this.hoverExited.RemoveListener(HoverExited);
        this.selectEntered.RemoveListener(SelectEntered);
        this.selectExited.RemoveListener(SelectExited);
    }

    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if(other.CompareTag("Hand")){
            if(other.GetComponent<XRDirectInteractor>().selectTarget == null) {
                this.nChild = this.attachTransform.childCount;
                if(nChild > 0){
                    Transform lastChild = this.attachTransform.GetChild(nChild-1);
                    lastChild.gameObject.SetActive(true);
                    SetupRigidbodyGrab(lastChild.GetComponent<Rigidbody>());
                }
            }
        }
        //Debug.Log("PouchSocket OnTriggerEnter");
    }

    protected new void OnTriggerExit(Collider other) 
    {
        base.OnTriggerExit(other);

        if(other.CompareTag("Hand")){
            if(this.attachTransform.childCount > 0) {
                // anzahl der kinder von attach hat sich nicht verandert
                Transform lastChild = this.attachTransform.GetChild(this.attachTransform.childCount-1);
                bool stoneStayed = this.nChild == this.attachTransform.childCount;
                bool stoneIsActive = lastChild.gameObject.activeInHierarchy == true;
                if(stoneStayed && stoneIsActive){
                    lastChild.gameObject.SetActive(false);
                }
                if(!stoneStayed){
                    this.nStones = this.attachTransform.childCount;
                }
            } else {
                this.nStones = 0;
            }
        }

        //Debug.Log("PouchSocket OnTriggerExit");
    }

    protected virtual void HoverEntered(HoverEnterEventArgs args) // wenn item in hand
    {
        this.standardMaterial = this.GetComponent<MeshRenderer>().material;
        this.GetComponent<MeshRenderer>().material = hoverMaterial;
    }

    protected virtual void HoverExited(HoverExitEventArgs args) // wenn item in hand
    {
        this.GetComponent<MeshRenderer>().material = this.standardMaterial;
    }

    protected virtual void SelectEntered(SelectEnterEventArgs args) // when an Interactable is snapped into the socket
    {
        XRBaseInteractable stone = args.interactable;
        stone.transform.SetParent(this.attachTransform);
        this.nStones = this.attachTransform.childCount;
        SetupRigidbodyGrab(stone.GetComponent<Rigidbody>());
        stone.gameObject.SetActive(false);
        // Debug.Log("PouchSocket SelectEntered");
    }

    protected virtual void SelectExited(SelectExitEventArgs args) // when an Interactable is removed from the socket
    {
        // ...
    }

    protected virtual void SetupRigidbodyGrab(Rigidbody rb)
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;
    }

    protected virtual void SetupRigidbodyDrop(Rigidbody rigidbody)
    {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.drag = 0f;
        rigidbody.angularDrag = 0.05f;
    }
}
