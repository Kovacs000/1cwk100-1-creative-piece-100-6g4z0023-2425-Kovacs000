using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public int remainingClicks; // Number of interactions required to destroy
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        SetRemainingClicks();
    }

    void SetRemainingClicks()
    {
        remainingClicks = Random.Range(1, 8); // Randomize clicks needed to destroy
    }

    void OnMouseDown()
    {
        if (remainingClicks > 0)
        {
            remainingClicks--; // Reduce clicks remaining

            if (remainingClicks == 0)
            {
                DestroyObject(); // Handle destruction
            }
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject); // Remove the object from the scene
    }
}
