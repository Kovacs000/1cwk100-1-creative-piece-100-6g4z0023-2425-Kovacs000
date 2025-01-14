using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public QuestManager questManager; // Reference to the QuestManager to manage quest updates

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "GoldenKey" tag
        if (other.CompareTag("GoldenKey"))
        {
            // Update the quest progress when the golden key is collected
            questManager.CollectGoldenKey();

            // Optionally destroy the collected key object
            Destroy(other.gameObject);
        }
    }
}
