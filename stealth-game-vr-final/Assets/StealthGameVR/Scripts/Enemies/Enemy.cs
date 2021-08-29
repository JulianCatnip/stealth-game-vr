using UnityEngine;

/*
* This class manages enemy animation and health. 
*/
public class Enemy : MonoBehaviour
{
    public float healthPoints = 100.0f;
    public bool enemyIsDummy;
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
        float finalDamage;

        // Its a sword stroke
        if(magnitude > 2)
        {
            finalDamage = damage * magnitude * multiplier;
        }
        // Its a thrown weapon 
        else
        {
            finalDamage = damage * multiplier;
        }
        this.healthPoints -= finalDamage;
        
        Debug.Log("Applied damage: " + finalDamage);
        // Debug.Log("Applied magnitude: " + magnitude);
    }
}
