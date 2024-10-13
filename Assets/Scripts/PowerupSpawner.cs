using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array containing the prefabs for SlowDownTime, Invincible, and RocketBoost
    public float spawnInterval = 25f; // Time between power-up spawns
    public float spawnHeightOffset = 8f; // Height above the camera to spawn the power-up

    private Camera mainCamera;
    private GameObject currentPowerUp; // Reference to the currently spawned power-up

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn a power-up if none exists
            if (currentPowerUp == null)
            {
                // Randomly pick a power-up from the array
                int randomIndex = Random.Range(0, powerUpPrefabs.Length);
                GameObject powerUpPrefab = powerUpPrefabs[randomIndex];

                // Calculate the spawn position above the camera
                float spawnY = mainCamera.transform.position.y + mainCamera.orthographicSize + spawnHeightOffset;
                float spawnX = Random.Range(mainCamera.transform.position.x - mainCamera.orthographicSize, mainCamera.transform.position.x + mainCamera.orthographicSize);
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

                // Instantiate the power-up
                currentPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"{powerUpPrefab.name} spawned at: {spawnPosition}");
            }
        }
    }

    void Update()
    {
        if (currentPowerUp != null)
        {
            // Calculate the position below the camera to despawn the power-up
            float despawnY = mainCamera.transform.position.y - mainCamera.orthographicSize - spawnHeightOffset;

            // Despawn the power-up if it goes below the camera view
            if (currentPowerUp.transform.position.y < despawnY)
            {
                Destroy(currentPowerUp);
                // Allow a new power-up to be spawned later
                currentPowerUp = null;
                Debug.Log("Power-up despawned");
            }
        }
    }
}
