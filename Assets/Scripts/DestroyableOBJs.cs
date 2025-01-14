using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public int remainingClicks;  // Remaining clicks to destroy
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        SetRemainingClicks();
    }

    // Set initial remaining clicks based on sword status
    void SetRemainingClicks()
    {
        // Generate a random number of clicks between 1 and 7 regardless of sword status
        remainingClicks = Random.Range(1, 8);
    }

    void OnMouseDown()
    {
        if (remainingClicks > 0)
        {
            remainingClicks--;  // Decrease clicks on interaction

            // If the object is destroyed (remaining clicks are 0)
            if (remainingClicks == 0)
            {
                DestroyObject();
            }
        }
    }

    void DestroyObject()
    {
        // Destroy the object when remainingClicks reaches 0
        Destroy(gameObject);  // Destroy the current object
    }
}
