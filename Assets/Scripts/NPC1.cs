using UnityEngine;

public class NPC1 : MonoBehaviour
{
    private MessageDisplay messageBox;
    public GameObject rewardPrefab;
    public string keyItemName = "Golden Key";
    private bool questComplete = false;
    public float rewardDistance = 2.0f;

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
    }

    void AnswerFunc(bool answer)
    {
        if (answer && !questComplete)
        {
            messageBox.ShowMultilineMessage("Thank you! The blacksmith's house is just beyond the mountain pass. I'll be here when you return.");
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

            if (!questComplete)
            {
                if (playerInventory != null && playerInventory.GetCount(keyItemName) > 0)
                {
                    CompleteQuest(collision.gameObject);
                }
                else
                {
                    messageBox.YesNoMessage(
                        "Oh, hello there! Could you help me? I've lost my golden key. I think I left it near the blacksmith's house. Could you take a look?",
                        AnswerFunc
                    );
                }
            }
            else
            {
                messageBox.ShowMultilineMessage("You've already helped me find my key. Thank you again!");
            }
        }
    }

    private void CompleteQuest(GameObject player)
    {
        questComplete = true;
        messageBox.ShowMultilineMessage("Thank you for finding my key! Here, take this sword as a reward.");

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
            playerInventory.Remove(keyItemName);
        }
    }
}
