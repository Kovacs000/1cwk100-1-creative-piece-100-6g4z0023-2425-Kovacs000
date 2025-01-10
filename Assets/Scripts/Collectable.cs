using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public GameObject itemPrefab;
    public QuestManager questManager;  // Add a reference to the QuestManager script

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Inventory playerInventory = coll.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.Add(itemName, 1);  // Add the item to the inventory
                MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
                disp.ShowMessage("You picked up a " + itemName, 2.0f);

                // Call the method from QuestManager to update the quest progress
                if (questManager != null && itemName == "Golden Key")
                {
                    questManager.CollectGoldenKey(); // Update the quest when the Golden Key is collected
                }

                // Disable the Golden Key GameObject instead of destroying it
                gameObject.SetActive(false);  // Disable the object to simulate "collecting" it
            }
        }
    }
}
