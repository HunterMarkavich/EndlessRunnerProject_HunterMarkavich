using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFragmentSpawner : MonoBehaviour
{
    public GameObject starFragmentPrefab; // Prefab for the star fragment collectible
    public PlatformSpawner platformSpawner; // Reference to the PlatformSpawner script to track platforms
    public float minOffsetY = 1f; // Minimum vertical offset from platform to spawn
    public float maxOffsetY = 3f; // Maximum vertical offset from platform to spawn
    public int maxFragmentsOnScreen = 5; // Maximum number of fragments allowed on the screen
    public float despawnHeightOffset = 10f; // Height below the camera's view to despawn
    public float minDistanceBetweenFragments = 1f; // Minimum distance between fragments to avoid overlapping
    public float minDistanceFromPlatform = 1f; // Minimum distance from platforms to avoid overlapping

    private Camera mainCamera;
    private List<GameObject> starFragments = new List<GameObject>();

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Spawn new Star Fragments if there are fewer than the maximum on the screen
        if (GetActiveFragmentCount() < maxFragmentsOnScreen)
        {
            SpawnStarFragment();
        }

        // Despawn fragments below the camera
        DespawnStarFragments();
    }

    private void SpawnStarFragment()
    {
        if (platformSpawner.spawnedPlatforms.Count > 0)
        {
            int attempts = 0;
            bool validPositionFound = false;
            Vector3 spawnPosition = Vector3.zero;

            while (attempts < 10 && !validPositionFound)
            {
                // Choose a random platform to spawn the Star Fragment above
                GameObject platform = platformSpawner.spawnedPlatforms[Random.Range(0, platformSpawner.spawnedPlatforms.Count)];

                // Generate spawn position based on the platform's position, ensuring it spawns above the camera
                Vector3 platformPosition = platform.transform.position;
                float spawnX = platformPosition.x;
                float spawnY = Mathf.Max(platformPosition.y + Random.Range(minOffsetY, maxOffsetY), mainCamera.transform.position.y + mainCamera.orthographicSize + minOffsetY);

                spawnPosition = new Vector3(spawnX, spawnY, 0);

                if (IsValidSpawnPosition(spawnPosition))
                {
                    validPositionFound = true;
                }

                attempts++;
            }

            if (validPositionFound)
            {
                GameObject newStarFragment = Instantiate(starFragmentPrefab, spawnPosition, Quaternion.identity);

                // Add the new star fragment to the list
                starFragments.Add(newStarFragment);
            }
        }
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Ensure the position is not too close to other star fragments
        foreach (GameObject fragment in starFragments)
        {
            if (fragment != null && Vector3.Distance(position, fragment.transform.position) < minDistanceBetweenFragments)
            {
                return false;
            }
        }

        // Ensure the position is not too close to platforms
        foreach (GameObject platform in platformSpawner.spawnedPlatforms)
        {
            if (platform != null && Vector3.Distance(position, platform.transform.position) < minDistanceFromPlatform)
            {
                return false;
            }
        }

        return true;
    }

    private void DespawnStarFragments()
    {
        float cameraBottomEdge = mainCamera.transform.position.y - mainCamera.orthographicSize - despawnHeightOffset;

        // Iterate through the star fragments and destroy any that are below the camera's view
        for (int i = starFragments.Count - 1; i >= 0; i--)
        {
            if (starFragments[i] != null && starFragments[i].transform.position.y < cameraBottomEdge)
            {
                Destroy(starFragments[i]);
                starFragments.RemoveAt(i);
            }
        }
    }

    private int GetActiveFragmentCount()
    {
        // Count only the active fragments that are still on the screen
        int count = 0;
        foreach (GameObject fragment in starFragments)
        {
            if (fragment != null)
            {
                count++;
            }
        }
        return count;
    }
}
