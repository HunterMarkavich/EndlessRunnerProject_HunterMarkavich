using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject spikePrefab; // Reference to the spike prefab

    public float minSpawnX = -6f; // Minimum X position for spawning
    public float maxSpawnX = 6f;  // Maximum X position for spawning
    public float minSpawnY = 1.5f;  // Minimum vertical distance between platforms
    public float maxSpawnY = 3.5f;  // Maximum vertical distance between platforms
    public float spawnHeightOffset = 5f; // Height above the camera's view to spawn
    public float despawnHeightOffset = 10f; // Height below the camera's view to despawn

    [Range(0, 100)] public int solidPlatformPercentage = 60; // Spawn percentage for Solid platform
    [Range(0, 100)] public int passThroughPlatformPercentage = 30; // Spawn percentage for Pass Through platform
    [Range(0, 100)] public int bouncePlatformPercentage = 10; // Spawn percentage for Bounce platform
    [Range(0, 100)] public int spikeSpawnRate = 5; // Percentage chance to spawn a spike on top of a Solid platform

    private Camera mainCamera;
    private float lastSpawnY; // Last Y position of a spawned platform
    public List<GameObject> spawnedPlatforms = new List<GameObject>(); // Track all spawned platforms

    void Start()
    {
        mainCamera = Camera.main;
        lastSpawnY = mainCamera.transform.position.y + spawnHeightOffset;
        SpawnInitialPlatforms();
    }

    void Update()
    {
        // Spawn new platforms as the player progresses
        if (mainCamera.transform.position.y + spawnHeightOffset > lastSpawnY)
        {
            SpawnPlatform();
        }

        // Despawn platforms below the camera
        DespawnPlatforms();
    }

    private void SpawnInitialPlatforms()
    {
        // Spawn initial platforms above the starting position
        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        // Generate random X and Y positions for spawning a new platform
        float spawnX = Random.Range(minSpawnX, maxSpawnX);
        float spawnY = lastSpawnY + Random.Range(minSpawnY, maxSpawnY);

        // Ensure that the new platform has an offset to avoid direct vertical stacking
        if (spawnedPlatforms.Count > 0)
        {
            Vector3 lastPlatformPos = spawnedPlatforms[spawnedPlatforms.Count - 1].transform.position;
            if (Mathf.Abs(lastPlatformPos.x - spawnX) < 1.5f)
            {
                // Offset horizontally to prevent stacking
                spawnX += Random.Range(1.5f, maxSpawnX);
                // Ensure it stays within bounds
                spawnX = Mathf.Clamp(spawnX, minSpawnX, maxSpawnX);
            }
        }

        // Determine which platform to spawn based on weighted randomization
        int totalPercentage = solidPlatformPercentage + passThroughPlatformPercentage + bouncePlatformPercentage;
        int randomValue = Random.Range(0, totalPercentage);

        GameObject platformPrefab = null;

        if (randomValue < solidPlatformPercentage)
        {
            // Spawn Solid platform
            platformPrefab = platformPrefabs[0];
            // Assuming index 0 is Solid
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
            GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

            // Add the new platform to the list
            spawnedPlatforms.Add(newPlatform);

            // Randomly decide if a spike should spawn on top of the Solid platform
            if (Random.Range(0, 100) < spikeSpawnRate)
            {
                Vector3 spikePosition = new Vector3(spawnPosition.x, spawnPosition.y + 0.5f, 0); // Adjust position to place on top
                GameObject newSpike = Instantiate(spikePrefab, spikePosition, Quaternion.identity);
                Debug.Log("Spike spawned on Solid platform at position: " + spikePosition);
            }
        }
        else if (randomValue < solidPlatformPercentage + passThroughPlatformPercentage)
        {
            // Spawn Pass Through platform
            platformPrefab = platformPrefabs[1];
        }
        else
        {
            // Spawn Bounce platform
            platformPrefab = platformPrefabs[2];
        }

        // Instantiate and add the non-solid platform to the list
        if (platformPrefab != null && platformPrefab != platformPrefabs[0])
        {
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
            GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            spawnedPlatforms.Add(newPlatform);
        }

        // Update the lastSpawnY position to ensure proper spacing for the next platform
        lastSpawnY = spawnY;
    }


    private void DespawnPlatforms()
    {
        float cameraBottomEdge = mainCamera.transform.position.y - mainCamera.orthographicSize - despawnHeightOffset;

        // Iterate through the platforms and destroy any that are below the camera's view
        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] != null && spawnedPlatforms[i].transform.position.y < cameraBottomEdge)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}

