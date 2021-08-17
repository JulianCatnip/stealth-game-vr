using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float healthPoints = 100.0f;
    private Animator animator;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if(this.healthPoints < 1)
        {
            animator.SetBool("isDead", true);
        }
        animator.SetFloat("healthPoints", this.healthPoints);
    }

    public void ApplyDamage(float magnitude, float damage, float multiplier) 
    {
        float finalDamage = damage * magnitude * multiplier;
        this.healthPoints -= finalDamage;
        Debug.Log("Applied damage: " + finalDamage);
        // Debug.Log("Applied magnitude: " + magnitude);
    }
}
