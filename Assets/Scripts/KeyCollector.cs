using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public QuestManager questManager; // Reference to the QuestManager

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoldenKey"))  // Ensure that the object is tagged as GoldenKey
        {
            // Call the method to update the Golden Key quest progress
            questManager.CollectGoldenKey();

            // Destroy the Golden Key object after collection (optional)
            Destroy(other.gameObject);
        }
    }
}
