using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Transform targetObject;
    public Transform player;
    public float moveSpeed = 2f;
    public float rotationSpeed = 2f;
    public float detectionRange = 5f;
    public LayerMask playerLayer;

    private bool playerDetected = false;

    void Start()
    {
        // Set initial target to the target object
        transform.LookAt(targetObject);
    }

    void Update()
    {
        if (!playerDetected)
        {
            // Move towards the target object if player is not detected
            transform.position = Vector3.MoveTowards(transform.position, targetObject.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Move towards the player if detected
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            // Rotate towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Check for player detection
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, detectionRange, playerLayer))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    playerDetected = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is near, hide in dirt
            HideInDirt();
        }
    }

    void HideInDirt()
    {
        // Your code to hide the zombie in the dirt
    }
}
