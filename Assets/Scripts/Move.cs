using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    private new SpriteRenderer renderer;


    private Rigidbody2D rb;
    private bool isGrounded;//public daromes, kad galetume patestuoti radius, paskui pakeisti i private
    [SerializeField] private float groundedRadius;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move1();
        CheckGroundedState();
    }

    private void Move1()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * movementSpeed, rb.velocity.y);//

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);//

        }
        if (horizontalInput < 0)
        {
            renderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            renderer.flipX = false;
        }
    }

    private void CheckGroundedState()
    {
        Vector2 pos = transform.position;
        Vector2 direction = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(pos, direction, groundedRadius, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    public float frictionCoefficient = 0.1f; // Adjust this value as needed
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
