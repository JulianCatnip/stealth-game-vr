using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoneSocket : XRSocketInteractor
{
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

    protected virtual void HoverEntered(HoverEnterEventArgs args) // wenn item in hand
    {
        this.transform.parent.GetComponent<PouchManager>().SetStandardMaterial(this.transform.parent.GetComponent<MeshRenderer>().material);
        this.transform.parent.GetComponent<MeshRenderer>().material = this.transform.parent.GetComponent<PouchManager>().hoverMaterial;
    }

    protected virtual void HoverExited(HoverExitEventArgs args) // wenn item in hand
    {
        this.transform.parent.GetComponent<MeshRenderer>().material = this.transform.parent.GetComponent<PouchManager>().GetStandardMaterial();
    }

    protected virtual void SelectEntered(SelectEnterEventArgs args) // when an Interactable is snapped into the socket
    {
        this.transform.parent.GetComponent<MeshRenderer>().material = this.transform.parent.GetComponent<PouchManager>().GetStandardMaterial();
        // Debug.Log("SelectEntered");
    }

    protected virtual void SelectExited(SelectExitEventArgs args)
    {
        // recover collider and mesh if it didnt already happened
        if(args.interactable != null)
        {
            if(args.interactable.GetComponent<MeshRenderer>().enabled == false || args.interactable.GetComponent<MeshCollider>().enabled == false)
            {
                args.interactable.GetComponent<MeshRenderer>().enabled = true;
                args.interactable.GetComponent<MeshCollider>().enabled = true;
            }
        }

        // Debug.Log("SelectExited");
    }
}
