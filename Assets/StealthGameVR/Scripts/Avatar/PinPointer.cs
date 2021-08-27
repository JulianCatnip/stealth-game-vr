using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class PinPointer : MonoBehaviour
{
    // we need the handcontroller 
        // to get input and 
        // to know if the current interactible (in the other hand) is a weapon
    // we need to attach the hitpoint to the enemy aka pin it down
    // we need to visualize the hitpoint
    [System.Serializable]
    public enum Hand {
        Left,
        Right
    }
    public Hand handType;

    public HandController leftHandController;
    public HandController rightHandController;

    private XRDirectInteractor directInteractor;
    private LineRenderer lineRenderer;
    private RaycastHit hitInfo;
    private int directionFactor;

    public GameObject hitPoint;
    public float raycastDistance = 100.0f;
    private GameObject activeHitPoint;

    private Vector3 acceleration;
    private Vector3 lastVelocity;

    void Start() {
        // get the LineRenderer component
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.lineRenderer.enabled = false;
        // get the direct interactor of the other hand to detect interactible
        if(this.handType == Hand.Left)
        {
            directInteractor = this.rightHandController.GetComponent<XRDirectInteractor>();
            directionFactor = -1;
        }
        else if(this.handType == Hand.Right)
        {
            directInteractor = this.leftHandController.GetComponent<XRDirectInteractor>();
            directionFactor = 1;
        }
    }

    void FixedUpdate() {
        if(this.handType == Hand.Left)
        {
            // only if the grip button of this hand is pressed and a throwable weapon is available
            if(this.leftHandController.GetGripButtonState() && !this.leftHandController.GetTriggerButtonState() && ThrowableWeaponAvailable())
            {
                // selected item of other hand can be thrown
                EnableLaserPointer();
            } 
            else if(ThrowableWeaponAvailable() && this.activeHitPoint != null) 
            {
                // observe for throw
                WaitForThrow();
            } 
            else 
            {
                DisableLaserPointer();
            }
        }
        else if(this.handType == Hand.Right)
        {
            // if the grip button of this hand is pressed
            if(this.rightHandController.GetGripButtonState() && !this.rightHandController.GetTriggerButtonState() && ThrowableWeaponAvailable())
            {
                // selected item of other hand can be thrown
                EnableLaserPointer();
            } 
            else if(ThrowableWeaponAvailable() && this.activeHitPoint != null)  
            {
                WaitForThrow();
            } 
            else {
                DisableLaserPointer();
            }
        }
    }

    private bool ThrowableWeaponAvailable()
    {
        if(this.directInteractor.selectTarget != null && (this.directInteractor.selectTarget.CompareTag("Shuriken") || this.directInteractor.selectTarget.CompareTag("Kunai") || this.directInteractor.selectTarget.CompareTag("Stone")))
        {
            return true;
        } else {
            return false;
        }
    }

    private void EnableLaserPointer()
    {
        // Enable Raycast only on Enemy and Light layer
        if(Physics.Raycast(this.transform.position, directionFactor * this.transform.GetChild(0).TransformDirection(Vector3.right), out this.hitInfo, this.raycastDistance, LayerMask.GetMask("Enemy", "Light")))
        {
            // Make raycast visible
            this.lineRenderer.enabled = true;
            this.lineRenderer.SetPosition(0, this.transform.position);
            this.lineRenderer.SetPosition(1, hitInfo.point);
            
            // save current raycast hit on primary button press
            if(this.handType == Hand.Left && this.leftHandController.GetPrimaryButtonState())
            {
                this.activeHitPoint = SetActiveHitPoint();
            } 
            else if(this.handType == Hand.Right && this.rightHandController.GetPrimaryButtonState())
            {
                this.activeHitPoint = SetActiveHitPoint();
            }
        }
        else
        {
            DisableLaserPointer();
        }
    }

    void DisableLaserPointer()
    {
        this.lineRenderer.enabled = false;
    }

    GameObject SetActiveHitPoint()
    {
        if(this.hitPoint != null && this.hitInfo.transform.GetComponent<WeakPoint>() != null)
        {
            return this.hitInfo.transform.GetComponent<WeakPoint>().SaveAndVisualizeHitPoint(this.hitInfo, this.hitPoint);
        } 
        else if(this.hitInfo.transform.GetComponentInChildren<WeakPoint>() != null)
        {
            return this.hitInfo.transform.GetComponentInChildren<WeakPoint>().SaveAndVisualizeHitPoint(this.hitInfo, this.hitPoint);
        } 
        else if(this.hitInfo.transform.GetComponent<LightController>() != null)
        {
            return this.hitInfo.transform.GetComponent<LightController>().SaveAndVisualizeHitPoint(this.hitInfo, this.hitPoint);
        } 
        else
        {
            return null;
        }
    }

    void WaitForThrow()
    {
        acceleration = (this.directInteractor.selectTarget.GetComponent<Rigidbody>().velocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = this.directInteractor.selectTarget.GetComponent<Rigidbody>().velocity;

        // If the acceleration of the throwable item is above 50.0f and we dont cheat by using locomotion
        if(acceleration.magnitude > 30.0f && !this.leftHandController.GetThumbstickUseState() && !this.rightHandController.GetThumbstickUseState())
        {
            //Debug.Log(acceleration.magnitude);
            // Let the item get shot towards the hitpoint at selectexit
            this.directInteractor.selectTarget.GetComponent<InteractableItem>().ThrowWeapon(this.activeHitPoint, this.raycastDistance);

        }
    }
}
