using UnityEngine;

public class Weapon : InteractableItem
{
    public float damage = 0.0f;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Light"))
        {
            if(other.relativeVelocity.magnitude > 2 && GetHandController() != null)
            {
                GetHandController().GetController().SendHapticImpulse(hapticChannel, 1.0f, 1.0f);
            }
        }
    }
}
