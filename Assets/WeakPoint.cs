using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public float damageMultiplier;
    public Enemy damageReceiver;
    private float magnitude;
    private float damage;
   
    void OnCollisionEnter(Collision other) {

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
}
