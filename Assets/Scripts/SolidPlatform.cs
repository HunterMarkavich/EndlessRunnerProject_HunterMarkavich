using System.Collections;
using UnityEngine;

public class SolidPlatformScript : MonoBehaviour
{
    // Reference to the spike prefab
    public GameObject spikePrefab;
    // Chance of a spike spawning on top of this platform
    [Range(0, 100)] public int spikeSpawnChance = 5;
    // Offset for spike height, adjustable in the Inspector
    [Range(0f, 2f)] public float spikeHeightOffset = 0.5f;

    void Start()
    {
        // Skip spike spawning if this is the tutorial platform
        if (gameObject.CompareTag("TutorialPlatform"))
        {
            return;
        }

        // Determine if a spike should spawn on top of this platform
        if (Random.Range(0, 100) < spikeSpawnChance)
        {
            // Spawn the spike above the platform using the adjustable offset
            Vector3 spikePosition = new Vector3(transform.position.x, transform.position.y + spikeHeightOffset, 0);
            GameObject spike = Instantiate(spikePrefab, spikePosition, Quaternion.identity);

            // Ensure the spike is tagged correctly
            spike.tag = "Spikes";

            Debug.Log("Spike spawned on top of Solid platform at: " + spikePosition);
        }
    }

}
