using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideFr : MonoBehaviour
{
    public float frictionCoefficient = 0.1f; // Adjust this value as needed

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyFriction();
    }

    void ApplyFriction()
    {
        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            // Calculate the opposite force to slow down the object
            Vector2 frictionForce = -rb.velocity.normalized * frictionCoefficient;

            // Apply the friction force to the rigidbody
            rb.AddForce(frictionForce, ForceMode2D.Force);
        }
    }
}
