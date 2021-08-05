using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{/**
    private void OnTriggerEvent(Collider other)
    {
        if(other.tag == "Hand")
        {
            this.GetComponent<Canvas>().enabled = true;
                
        }
    }
*/
    public void TurnOnOffLight()
    {
        if (this.GetComponent<Light>().enabled == true) {
            this.GetComponent<Light>().enabled = false;
        } else
        {
            this.GetComponent<Light>().enabled = true;
        }
    }


 
}
