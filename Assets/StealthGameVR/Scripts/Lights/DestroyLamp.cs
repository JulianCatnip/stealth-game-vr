using UnityEngine;

public class DestroyLamp : MonoBehaviour
{
    /*
    * Destroys the fireball gameobject containing the particle system and light component.
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shuriken") || other.CompareTag("Tanto") || other.CompareTag("Ninjato") || other.CompareTag("Kunai") || other.CompareTag("Stone"))
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
}
