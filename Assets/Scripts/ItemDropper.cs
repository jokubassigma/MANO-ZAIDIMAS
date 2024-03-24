using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject spritePrefab; // Drag the sprite prefab you want to spawn into this field in the Unity Inspector
    public int maxSprites = 10; // Maximum number of sprites allowed
    public float spawnRadius = 5f; // Radius within which sprites can spawn
    private int currentSpriteCount = 0;
    [SerializeField] private float y;

    // Add a LayerMask for specifying which layers should block the spawning of a new sprite
    public LayerMask obstacleLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSpriteCount < maxSprites)
            {
                Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPosition = new Vector3(randomPosition.x, y, 0f) + transform.position;
                TrySpawnSpriteAtPosition(spawnPosition);
            }
        }
    }

    void TrySpawnSpriteAtPosition(Vector3 position)
    {
        // Dynamically get the sprite renderer from the prefab (assuming it's the same for all instances)
        SpriteRenderer spriteRenderer = spritePrefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return; // Ensure the prefab has a SpriteRenderer

        Bounds bounds = spriteRenderer.bounds;
        float spriteWidth = bounds.size.x;
        float spriteHeight = bounds.size.y;

        // Calculate rays' start positions
        Vector2 left = new Vector2(position.x - spriteWidth / 2, position.y);
        Vector2 right = new Vector2(position.x + spriteWidth / 2, position.y);
        Vector2 top = new Vector2(position.x, position.y + spriteHeight / 2);
        Vector2 bottom = new Vector2(position.x, position.y - spriteHeight / 2);

        // Adjust the length of the rays to slightly exceed the sprite bounds to ensure clear space
        float rayLength = Mathf.Max(spriteWidth, spriteHeight) * 0.5f + 0.1f; // Adding a small buffer

        // Cast rays to check for obstacles
        bool canSpawn = !Physics2D.Raycast(left, Vector2.right, rayLength, obstacleLayer) &&
                        !Physics2D.Raycast(right, Vector2.left, rayLength, obstacleLayer) &&
                        !Physics2D.Raycast(top, Vector2.down, rayLength, obstacleLayer) &&
                        !Physics2D.Raycast(bottom, Vector2.up, rayLength, obstacleLayer);

        if (canSpawn)
        {
            Instantiate(spritePrefab, position, Quaternion.identity);
            currentSpriteCount++;
        }
    }
}
