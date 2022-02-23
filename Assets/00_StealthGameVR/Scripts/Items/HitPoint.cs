using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public Collider GetHitCollider()
    {
        if(this.transform.parent != null && this.transform.parent.GetComponent<Collider>() != null)
        {
            return this.transform.parent.GetComponent<Collider>();
        } else
        {
            return null;
        }
    }
}
