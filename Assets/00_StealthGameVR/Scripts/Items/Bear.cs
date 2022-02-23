using UnityEngine;

public class Bear : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hand") || other.gameObject.layer == 3)
        {
            this.GetComponent<AudioSource>().Play();
        }
    }
}
