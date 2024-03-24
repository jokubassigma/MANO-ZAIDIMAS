using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject objectToDrop; // Drag the object you want to drop into this field in the Unity Inspector
    public float dropForce = 5f; // Adjust the force at which the object is dropped

    private void Start()
    {
        // Check if the designated key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropObject();
        }
    }

    void DropObject()
    {
        // Check if the object to drop is assigned
        if (objectToDrop != null)
        {
            // Instantiate a new instance of the object
            GameObject droppedObject = Instantiate(objectToDrop, transform.position, Quaternion.identity);

            // Apply force to the dropped object
            Rigidbody rb = droppedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.down * dropForce, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("Object to drop is not assigned!");
        }
    }
    public GameObject spritePrefab; // Drag the sprite prefab you want to spawn into this field in the Unity Inspector
    public int maxSprites = 10; // Maximum number of sprites allowed
    public float spawnRadius = 5f; // Radius within which sprites can spawn
    private int currentSpriteCount = 0;
    private float y;

    public GameObject objectPrefab; // Prefab of the object to be placed
    private GameObject currentObject; // Reference to the currently placed object
    private bool canPlaceObject = true; // Flag to check if object can be placed

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSpriteCount < maxSprites)
            {
                SpawnSprite();
            }
        }
    }

    void SpawnSprite()
    {
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPosition.x, y, 0f) + transform.position;

        // Check if there's no sprite at the spawn position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0f); // Adjust the radius as per your sprite size
        bool canSpawn = true;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Trap"))
            {
                canSpawn = false;
                break;
            }
        }

        if (canSpawn)
        {
            Instantiate(spritePrefab, spawnPosition, Quaternion.identity);
            currentSpriteCount++;
        }
    }

}