using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public GameObject itemPrefab;
    public QuestManager questManager;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Inventory playerInventory = coll.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.Add(itemName, 1); // Add item to inventory

                MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
                if (disp != null)
                {
                    disp.ShowMessage("You picked up a " + itemName, 2.0f); // Display pickup message
                }

                if (questManager != null && itemName == "Golden Key")
                {
                    questManager.CollectGoldenKey(); // Notify quest progress
                }

                gameObject.SetActive(false); // Disable the collected object
            }
            else
            {
                Debug.LogWarning("Player Inventory not found.");
            }
        }
    }
}
