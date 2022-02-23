using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NinjatoSocket : XRSocketInteractor
{
    public Material hoverMaterial;
    private Material standardMaterial;
    
    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag("Ninjato"); // nur Steine gehören in diesen Sockel
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag("Ninjato"); // nur Steine gehören in diesen Sockel
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.hoverEntered.AddListener(HoverEntered);
        this.hoverExited.AddListener(HoverExited);
        this.selectEntered.AddListener(SelectEntered);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.hoverEntered.RemoveListener(HoverEntered);
        this.hoverExited.RemoveListener(HoverExited);
        this.selectEntered.RemoveListener(SelectEntered);
    }

    protected virtual void HoverEntered(HoverEnterEventArgs args) 
    {
        this.standardMaterial = this.GetComponent<MeshRenderer>().material;
        this.GetComponent<MeshRenderer>().material = hoverMaterial;
    }

    protected virtual void HoverExited(HoverExitEventArgs args) 
    {
        this.GetComponent<MeshRenderer>().material = this.standardMaterial;
    }

    protected virtual void SelectEntered(SelectEnterEventArgs args) // when an Interactable is snapped into the socket
    {
        this.GetComponent<MeshRenderer>().material = this.standardMaterial;
    }
}
