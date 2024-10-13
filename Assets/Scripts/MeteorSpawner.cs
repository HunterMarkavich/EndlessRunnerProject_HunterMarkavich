using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // Reference to the meteor prefab
    public Transform[] spawnColumns; // Array of columns to spawn meteors in
    public GameObject[] hazardSigns; // Hazard signs indicating upcoming meteor

    public float spawnInterval = 20f; // Initial interval between meteor spawns
    public float minSpawnInterval = 10f; // Minimum spawn interval for increased difficulty
    public float spawnSpeedIncreaseRate = 0.1f; // Rate of speed increase over time
    public float initialFallTime = 6f; // Time in seconds for the first meteor to fall from top to bottom
    public float initialMeteorSpeed = 3f; // Initial speed for subsequent meteors

    private float currentSpeed = 1f; // Initial speed multiplier for meteors
    private bool isFirstMeteor = true; // Track if it is the first meteor
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (spawnColumns.Length == 0 || hazardSigns.Length == 0)
        {
            Debug.LogError("No spawn columns or hazard signs assigned!");
            return;
        }

        StartCoroutine(SpawnMeteorRoutine());
    }

    void Update()
    {
        // Make the hazard signs follow the camera upwards
        foreach (GameObject hazardSign in hazardSigns)
        {
            Vector3 newPosition = hazardSign.transform.position;
            // Adjust to keep it near the top of the screen
            newPosition.y = mainCamera.transform.position.y + (mainCamera.orthographicSize - 1f);
            hazardSign.transform.position = newPosition;
        }
    }

    IEnumerator SpawnMeteorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Choose a random column to spawn the meteor in
            int randomColumnIndex = Random.Range(0, spawnColumns.Length);
            Transform spawnPoint = spawnColumns[randomColumnIndex];
            GameObject hazardSign = hazardSigns[randomColumnIndex];

            if (spawnPoint == null || hazardSign == null)
            {
                Debug.LogError("Spawn point or hazard sign is missing!");
                continue;
            }

            // Activate the hazard sign
            hazardSign.SetActive(true);
            Debug.Log("Hazard sign activated in column " + randomColumnIndex);

            // Wait 2 seconds before spawning the meteor
            yield return new WaitForSeconds(2f);

            // Spawn the meteor
            GameObject meteor = Instantiate(meteorPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Meteor spawned in column " + randomColumnIndex);

            // Set the meteor's velocity to move it downward
            Rigidbody2D meteorRb = meteor.GetComponent<Rigidbody2D>();
            if (meteorRb != null)
            {
                float fallSpeed = isFirstMeteor ? (2 * Camera.main.orthographicSize) / initialFallTime : currentSpeed;
                meteorRb.velocity = Vector2.down * fallSpeed;

                Debug.Log("Meteor velocity set successfully");
            }
            else
            {
                Debug.LogError("Meteor prefab is missing a Rigidbody2D component!");
            }

            // Set the first meteor flag to false after spawning the first meteor
            isFirstMeteor = false;

            // Deactivate the hazard sign after the meteor is spawned
            hazardSign.SetActive(false);
            Debug.Log("Hazard sign deactivated in column " + randomColumnIndex);

            // Increase the meteor speed gradually for difficulty
            currentSpeed += spawnSpeedIncreaseRate;

            // Reduce spawn interval to increase meteor frequency over time
            if (spawnInterval > minSpawnInterval)
            {
                // Adjust this rate as needed
                spawnInterval -= 1f;
            }
        }
    }
}