using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShurikenSocket : XRSocketInteractor
{
    //public Material hoverMaterial;
    //private Material standardMaterial;
    
    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag("Shuriken"); // nur Steine gehören in diesen Sockel
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag("Shuriken"); // nur Steine gehören in diesen Sockel
    }

    // protected override void OnEnable()
    // {
    //     base.OnEnable();
    //     this.hoverEntered.AddListener(HoverEntered);
    //     this.hoverExited.AddListener(HoverExited);
    // }

    // protected override void OnDisable()
    // {
    //     base.OnDisable();
    //     this.hoverEntered.RemoveListener(HoverEntered);
    //     this.hoverExited.RemoveListener(HoverExited);
    // }

    // protected virtual void HoverEntered(HoverEnterEventArgs args) // wenn item in hand
    // {
    //     this.standardMaterial = this.GetComponent<MeshRenderer>().material;
    //     this.GetComponent<MeshRenderer>().material = hoverMaterial;
    // }

    // protected virtual void HoverExited(HoverExitEventArgs args) // wenn item in hand
    // {
    //     this.GetComponent<MeshRenderer>().material = this.standardMaterial;
    // }
}
