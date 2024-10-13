using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    protected virtual void ActivateEffect(GameObject player)
    {
        // This method will be overridden by derived classes to define the power-up effect
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateEffect(collision.gameObject);

            // Remove this line that destroys the power-up immediately:
            // Destroy(gameObject); 

            Debug.Log($"{gameObject.name} collected by player");
        }
    }
}
