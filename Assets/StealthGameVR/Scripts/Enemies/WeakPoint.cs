using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public float damageMultiplier;
    public Enemy damageReceiver;
    private float magnitude;
    private float damage;

    private static GameObject currentHitPoint; // only one hitpoint is allowed to exist in all instances
   
    void OnCollisionEnter(Collision other) 
    {

        if(other.gameObject.CompareTag("Tanto") || other.gameObject.CompareTag("Ninjato") || other.gameObject.CompareTag("Shuriken") || other.gameObject.CompareTag("Kunai")) 
        {
            this.magnitude = other.relativeVelocity.magnitude;
            if(this.magnitude > 2)
            {
                this.damage = other.gameObject.GetComponent<Weapon>().damage;
                damageReceiver.ApplyDamage(this.magnitude, this.damage, damageMultiplier);
            }
        }
        
    }

    public GameObject SaveAndVisualizeHitPoint(RaycastHit hitInfo, GameObject hitPoint)
    {
        // Debug.Log("SaveAndVisualizeHitPoint called.");
        // if the current hit Point is not null destroy it
        RemoveHitPoint();
        // instatiate a new one placed at the hitpoint info parented by the collider
        currentHitPoint = Instantiate(hitPoint, hitInfo.point, Quaternion.identity, this.transform);
        
        return currentHitPoint;
    }

    public void RemoveHitPoint()
    {
        if(currentHitPoint != null)
        {
            Destroy(currentHitPoint);
        }
        // (-bad-) if there isnt a throwable weapon in the hand anymore
        // when the collider got hit by a throwable weapon
        // when the avatar is out of range
        // when there is a obstacle between collider and throwable weapon (when thrown)
            // then hit the obstable
    }

    public GameObject GetCurrentHitPoint()
    {
        return currentHitPoint;
    }
}
