using System.Collections;
using UnityEngine;

public class PassThroughCrumblingScript : MonoBehaviour
{
    private bool isPlayerOnPlatform = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // The player is no longer on this platform
            isPlayerOnPlatform = false;
        }
    }
}
