using UnityEngine;

/*
* This class manages weak spots on enemies. 
* It detects collisions of weapons and manages an auto-aim hit hitpoint for throwable weapons.
*/
public class WeakPoint : MonoBehaviour
{
    public float damageMultiplier;
    public Enemy damageReceiver;
    private float magnitude;
    private float damage;

    private static GameObject currentHitPoint; // only one hitpoint is allowed to exist in all instances of weak spots
    
    /*
    * If a weapon collides, apply damage to the receiver (enemy class).
    */
    void OnCollisionEnter(Collision other) 
    {
        // If the weapon has a blade
        if(other.gameObject.CompareTag("Tanto") || other.gameObject.CompareTag("Ninjato")) 
        {
            // apply damage only if at least some force was used
            this.magnitude = other.relativeVelocity.magnitude;
            if(this.magnitude > 2)
            {
                this.damage = other.gameObject.GetComponent<Weapon>().damage;
                damageReceiver.ApplyDamage(this.magnitude, this.damage, damageMultiplier);
            }
        } 
        // if the weapon is throwable
        else if(other.gameObject.CompareTag("Kunai") || other.gameObject.CompareTag("Shuriken") ||  other.gameObject.CompareTag("Stone"))
        {
            // apply damage without condition
            this.damage = other.gameObject.GetComponent<Weapon>().damage;
            damageReceiver.ApplyDamage(this.magnitude, this.damage, damageMultiplier);
        }
        
    }

    /*
    * Create and visualize a hitpoint, as an auto-aim for throwable weapons.
    */
    public GameObject SaveAndVisualizeHitPoint(RaycastHit hitInfo, GameObject hitPoint)
    {
        // if the current hit Point is not null destroy it
        RemoveHitPoint();
        // instatiate a new one placed at the hitpoint info parented by the collider
        currentHitPoint = Instantiate(hitPoint, hitInfo.point, Quaternion.identity, this.transform);
        
        return currentHitPoint;
    }

    /*
    * Removes the current existing hitpoint.
    */
    public void RemoveHitPoint()
    {
        if(currentHitPoint != null)
        {
            Destroy(currentHitPoint);
        }
        // when the collider got hit by a throwable weapon
        // when the avatar is out of range
        // when there is a obstacle between collider and throwable weapon (when thrown)
            // then hit the obstable
    }

    /*
    * Returns the current hitpoint.
    */
    public GameObject GetCurrentHitPoint()
    {
        return currentHitPoint;
    }
}
