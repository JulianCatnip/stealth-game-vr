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

        if(this.CompareTag("Dummy"))
        {
            enemyIsDummy = true;
            animator.SetBool("enemyIsDummy", true);
        } else
        {
            enemyIsDummy = false;
            animator.SetBool("enemyIsDummy", false);
        }
    }

    void Update()
    {
        if(this.healthPoints < 1)
        {
            if(enemyIsDummy)
            {
                animator.SetBool("isDead", true);
            } else
            {
                animator.SetBool("isDead", true);
            }
        }

        if(!enemyIsDummy)
        {
            animator.SetFloat("healthPoints", this.healthPoints);
        }
    }

    public void ApplyDamage(float magnitude, float damage, float multiplier) 
    {
        float finalDamage = damage * magnitude * multiplier;
        this.healthPoints -= finalDamage;
        
        Debug.Log("Applied damage: " + finalDamage);
        // Debug.Log("Applied magnitude: " + magnitude);
    }
}
