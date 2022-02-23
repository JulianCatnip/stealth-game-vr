using UnityEngine;

public class LightController : WeakPoint
{
    protected override void OnCollisionEnter(Collision other) 
    {
        // If the weapon has a blade
        if(other.gameObject.CompareTag("Tanto") || other.gameObject.CompareTag("Ninjato")) 
        {
            // destroy light only if at least some force was used
            this.magnitude = other.relativeVelocity.magnitude;
            if(this.magnitude > 2)
            {
                DestroyLight();
            }
        } 
        // if the weapon is throwable
        else if(other.gameObject.CompareTag("Kunai") || other.gameObject.CompareTag("Shuriken") ||  other.gameObject.CompareTag("Stone"))
        {
            // destroy without condition
            DestroyLight();
        }
    }
    
    /*
    * Destroys the fireball gameobject containing the particle system and light component.
    */
    public void DestroyLight()
    {
        Destroy(this.gameObject);
    }

    public void EnableLight()
    {
       this.gameObject.SetActive(true);
    }

    public void DisableLight()
    {
       this.gameObject.SetActive(false);
    }
}
