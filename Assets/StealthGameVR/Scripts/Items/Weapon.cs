using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InteractableItem
{
    public float damage = 0.0f; 
    private Rigidbody rb;
    private Transform blade;
    private BoxCollider bladeCollider;

    // rigidbody
        // velocity -> kraft
        // ab nehm bestimmten treashold -> schaden

    void Start() {
        rb = this.GetComponent<Rigidbody>();
        blade = this.transform.Find("Blade");
        if(blade != null)
        {
            bladeCollider = this.transform.Find("Blade").GetComponent<BoxCollider>();
        }
    }

    void Update() {
        // if weapon is on equipped layer
            // collides with vulnerable point (enemy) or
            // destructible item
                // and has magnitude over x
                    // damage multiplikator
                // and has magnitude over y
                    // damage multiplikator
        // Debug.Log("Sword magnitude:" + rb.velocity.magnitude);

        if(rb.velocity.magnitude > 1){
            Debug.Log("Sword magnitude is above 1.");
        }

        if(rb.velocity.magnitude > 2){
            Debug.Log("Sword magnitude is above 2.");
        }

        if(rb.velocity.magnitude > 4){
            Debug.Log("Sword magnitude is above 4.");
        }

        if(rb.velocity.magnitude > 5){
            Debug.Log("Sword magnitude is above 5.");
        }

        if(rb.velocity.magnitude > 10){
            Debug.Log("Sword magnitude is above 10.");
        }
    }
}
