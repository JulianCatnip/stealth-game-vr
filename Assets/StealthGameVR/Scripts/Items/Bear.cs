using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hand"))
        {
            this.GetComponent<AudioSource>().Play();
        }
    }
}
