using System.Collections;
using UnityEngine;

public class OrbitalDroneSpawner : MonoBehaviour
{
    public GameObject dronePrefab; // Reference to the drone prefab
    public float spawnInterval = 15f; // Time between drone spawns
    public float spawnHeightOffset = 5f; // Height above the camera to spawn the drone
    public float despawnHeightOffset = 10f; // Height below the camera to despawn the drone

    private Camera mainCamera;
    private GameObject currentDrone; // Reference to the currently spawned drone

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnDroneRoutine());
    }

    IEnumerator SpawnDroneRoutine()
    {
        while (true)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn a drone if none exist
            if (currentDrone == null)
            {
                // Calculate the spawn position above the camera
                float spawnY = mainCamera.transform.position.y + mainCamera.orthographicSize + spawnHeightOffset;
                float spawnX = Random.Range(mainCamera.transform.position.x - mainCamera.orthographicSize, mainCamera.transform.position.x + mainCamera.orthographicSize);
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

                // Instantiate the drone
                currentDrone = Instantiate(dronePrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Drone spawned at: " + spawnPosition);
            }
        }
    }

    void Update()
    {
        if (currentDrone != null)
        {
            // Calculate the position below the camera to despawn the drone
            float despawnY = mainCamera.transform.position.y - mainCamera.orthographicSize - despawnHeightOffset;

            // Despawn the drone only if it goes *well below* the camera view
            if (currentDrone.transform.position.y < despawnY)
            {
                Debug.Log("Drone despawned at: " + currentDrone.transform.position);
                Destroy(currentDrone);
                // Allow a new drone to be spawned later
                currentDrone = null;
            }
        }
    }
}
