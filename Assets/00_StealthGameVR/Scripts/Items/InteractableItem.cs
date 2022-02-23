using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableItem : MonoBehaviour
{
    protected XRGrabInteractable grabInteractable;
    private int defaultLayer = 0;
    private int equippedLayer = 7; // darf nur mit Hand kollidieren wenn ausgerüstet
    private int grabbedItemsLayer = 3; // darf nicht mit Körper kollidieren wenn in der Hand
    
    protected uint hapticChannel = 0;
    protected float hapticAmplitude = 0.5f;
    protected float hapticDuration = 0.3f;
    protected HandController handController;
    private Transform prevParent;

    private GameObject activeHitpoint;
    private bool weaponGotThrown = false;
    private bool selectGotExited = false;
    private float oldDamage;
    private float speed = 10.0f;
    private float maxThrowingDistance;

    protected void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.hoverEntered.AddListener(HoverEntered);
        grabInteractable.selectEntered.AddListener(SelectEntered);
        grabInteractable.selectExited.AddListener(SelectExited);
    }

    protected void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(HoverEntered);
        grabInteractable.selectEntered.RemoveListener(SelectEntered);
        grabInteractable.selectExited.RemoveListener(SelectExited);
    }

    protected virtual void HoverEntered(HoverEnterEventArgs args)
    {
        // Sends an haptic impulse to the controller in case a grabbed interactible hovers over a valid socket
        if(args.interactor.CompareTag("Socket") && args.interactor.CanSelect(args.interactable))
        {
            if(args.interactable.selectingInteractor.CompareTag("Hand"))
            {
                handController = args.interactable.selectingInteractor.GetComponent<HandController>();
                handController.GetController().SendHapticImpulse(hapticChannel, hapticAmplitude, hapticDuration);
            }
        // Sends an haptic impulse to the controller in case the hand hovers over a eqipped socket
        } else if(args.interactor.CompareTag("Hand"))
        {
            if(args.interactable.selectingInteractor != null && args.interactable.selectingInteractor.CompareTag("Socket"))
            {
                handController = args.interactor.GetComponent<HandController>();
                handController.GetController().SendHapticImpulse(hapticChannel, hapticAmplitude, hapticDuration);
            }
        }
    }

    protected virtual void SelectEntered(SelectEnterEventArgs args)
    {   
        // Add interactible to the equipped layer in case the its in a socket
        if(args.interactor.CompareTag("Socket")){
            gameObject.layer = this.equippedLayer;
            if(gameObject.transform.childCount > 0){
                foreach(Transform child in gameObject.transform){
                    child.gameObject.layer = this.equippedLayer;
                }
            }
        // Add interactible to the grabbed layer in case the its in a hand
        } else {
            gameObject.layer = grabbedItemsLayer;
            if(gameObject.transform.childCount > 0){
                foreach(Transform child in gameObject.transform){
                    child.gameObject.layer = grabbedItemsLayer;
                }
            }

        }

        // Set up fitting hand attachment/pivot point for the hand in case its grabbed by a hand and give haptiv feedback
        if(args.interactor.CompareTag("Hand")){
            handController = args.interactor.GetComponent<HandController>();
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
            handController.GetController().SendHapticImpulse(hapticChannel, hapticAmplitude, hapticDuration);
        // Set up fitting attachment/pivot point for the socket in case its not grabbed by a hand
        } else {
            if(gameObject.transform.Find("PivotPoint") != null) {
                gameObject.GetComponent<XRGrabInteractable>().attachTransform = gameObject.transform.Find("PivotPoint");
            }
        }

        // Remember previous parent in scene hierarchy and set attachment transform of the interactor as parent
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

        // Initiate flying weapon in FixedUpdate
        if(weaponGotThrown)
        {
            selectGotExited = true;
        }
        
        // Debug.Log("InteractableItem OnSelectExited");
    }

    void FixedUpdate()
    {
        if(selectGotExited)
        {
            // Disable rigidbody parameters to stop gravity from interrupting the flight
            this.GetComponent<Rigidbody>().mass = 0.0f;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().angularDrag = 0.0f;

            // raise weapon damage to kill instantly (doesnt work wtf)
            if(this.GetComponent<Weapon>().damage < 10)
            {
                oldDamage = this.GetComponent<Weapon>().damage;
            }
            this.GetComponent<Weapon>().damage *= 100;

            // turn weapon towards target (bad implementation but coudnt do better)
            transform.LookAt(this.activeHitpoint.transform.position, transform.up);
            transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

            // move the weapon towards the target
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.activeHitpoint.transform.position, this.speed*Time.fixedDeltaTime);
            
            // Debug.Log("SelectExited WeaponGotThrown");
            
            // If this weapon got close to the target
            if(Vector3.Distance(this.transform.position, this.activeHitpoint.transform.position) < 0.1f)
            {
                // enable rigidbody gravity properties again
                this.GetComponent<Rigidbody>().mass = 1.0f;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Rigidbody>().angularDrag = 0.05f;
                
                // try to force a stronger collide to get more damage
                if(this.activeHitpoint.GetComponent<HitPoint>().GetHitCollider() != null)
                {
                    this.GetComponent<Rigidbody>().AddForce((this.activeHitpoint.GetComponent<HitPoint>().GetHitCollider().transform.position - this.transform.position) * speed, ForceMode.Force);
                }

                // destroy the target point and reset values
                activeHitpoint.GetComponent<HitPoint>().Destroy();
                this.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                this.GetComponent<Weapon>().damage = oldDamage;
                weaponGotThrown = false;
                selectGotExited = false;
            }
        }
    }

    protected HandController GetHandController()
    {
        if(this.GetComponent<XRGrabInteractable>().selectingInteractor != null)
        {
            return this.handController = this.GetComponent<XRGrabInteractable>().selectingInteractor.GetComponent<HandController>();
        } 
        else
        {
            return null;
        }
    }

    public void ThrowWeapon(GameObject activeHitpoint, float distance)
    {
        // Call this function to initiate an auto aiming throw at selectexit and fixedupdate
        this.activeHitpoint = activeHitpoint;
        this.maxThrowingDistance = distance;
        
        if(ValidDistance())
        {
            this.weaponGotThrown = true;
        } else
        {
            this.weaponGotThrown = false;
        }
    }

    bool ValidDistance()
    {
        if(Vector3.Distance(this.transform.position, this.activeHitpoint.transform.position) < this.maxThrowingDistance + 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}