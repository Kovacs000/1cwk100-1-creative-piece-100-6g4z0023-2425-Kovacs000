using UnityEngine;

public class NPC1 : MonoBehaviour
{
    private MessageDisplay messageBox;
    public GameObject rewardPrefab;
    public string keyItemName = "Golden Key";
    private bool questComplete = false;
    public float rewardDistance = 2.0f;

    // QuestManager reference
    private QuestManager questManager;

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
        questManager = FindObjectOfType<QuestManager>();  // Get the QuestManager in the scene
    }

    void AnswerFunc(bool answer)
    {
        if (answer && !questComplete)
        {
            messageBox.ShowMultilineMessage("Thank you! The blacksmith's house is just beyond the mountain pass. I'll be here when you return.");
            if (questManager != null)
            {
                questManager.StartQuest("Golden Key"); // Start the Golden Key quest
            }
        }
        else if (!answer)
        {
            messageBox.ShowMultilineMessage("I understand. If you change your mind, the offer still stands.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Inventory playerInventory = collision.gameObject.GetComponent<Inventory>();

            if (questManager != null)
            {
                string questStatus = questManager.GetQuestStatus("Golden Key"); // Get the current quest status

                if (questStatus == "Not Started")
                {
                    messageBox.YesNoMessage(
                        "Oh, hello there! Could you help me? I've lost my golden key. I think I left it near the blacksmith's house. Could you take a look?",
                        AnswerFunc
                    );
                }
                else if (questStatus == "In Progress")
                {
                    messageBox.ShowMultilineMessage("You're already on the quest to retrieve the golden key. Please return once you have it.");
                }
                else if (questStatus == "Return the Key")
                {
                    // Check if the player has the golden key in their inventory
                    if (playerInventory != null && playerInventory.HasItem(keyItemName))
                    {
                        messageBox.ShowMultilineMessage("Quest Completed: You have returned the key! Claim your reward.");
                        CompleteQuest(collision.gameObject);  // Complete the quest when the key is returned
                    }
                    else
                    {
                        messageBox.ShowMultilineMessage("You don't seem to have the golden key. Please bring it to me.");
                    }
                }
                else if (questStatus == "Completed")
                {
                    // Adjusted message after completion to avoid "thank you" in quest description
                    messageBox.ShowMultilineMessage("Quest Completed! Enjoy your reward.");
                }
            }
        }
    }

    private void CompleteQuest(GameObject player)
    {
        questComplete = true;

        // Give reward to the player (a sword in this case)
        if (rewardPrefab != null)
        {
            Vector3 rewardPosition = transform.position + transform.right * rewardDistance;
            GameObject sword = Instantiate(rewardPrefab, rewardPosition, Quaternion.identity);
            Collectable collectable = sword.GetComponent<Collectable>();

            if (collectable == null)
            {
                collectable = sword.AddComponent<Collectable>();
            }

            collectable.itemName = "Sword";
            collectable.itemPrefab = sword;
        }

        Inventory playerInventory = player.GetComponent<Inventory>();
        if (playerInventory != null)
        {
            playerInventory.Remove(keyItemName); // Remove the golden key from the player's inventory
        }

        if (questManager != null)
        {
            questManager.CompleteQuest("Golden Key"); // Mark the quest as complete
        }

        // Show dialog with NPC after quest completion
        messageBox.ShowMultilineMessage("Thank you for returning the golden key. Here is your reward: a sword!");
    }
}
