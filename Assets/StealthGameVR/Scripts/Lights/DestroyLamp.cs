using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLamp : MonoBehaviour
{

  // Destroys the light component of a game object when there is a collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Destroy(this.GetComponent<Light>());
            // this.GetComponent<Light>().enabled = false;
        }
    }
}
