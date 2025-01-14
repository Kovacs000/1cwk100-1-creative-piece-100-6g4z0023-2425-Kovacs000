using UnityEngine;

public class PotionNPC : MonoBehaviour
{
    private MessageDisplay messageBox;
    private bool hasGivenPotion = false;
    private bool isWaitingForResponse = false;

    // Potion and Drop Point references
    public GameObject potionPrefab;   // The potion prefab that will be dropped
    public Transform potionDropPoint; // The invisible drop point for the potion

    // Messages for NPC interactions
    private string greetingMessage = "Hello, traveler. There is a rock blocking the path below the mountain. I can offer you a potion that grants temporary magic to help destroy it.";
    private string potionOfferMessage = "Would you like to accept this potion that grants temporary magic to destroy the rock? It is a powerful potion, but be careful in how you use it.";
    private string postPotionMessage = "Ah, so you have accepted it. Don't waste it though. Once used, its power will fade quickly, and you will need to return to me if you want more. Use it wisely, traveler.";
    private string declinePotionMessage = "It is your loss. The rock won't move itself, and the path will remain blocked. I can't say I didn't offer you a way through, though. The decision is yours.";
    private string usedPotionMessage = "I just gave you the potion. Have you used it yet?";

    private BoxCollider2D npcCollider;  // Reference to the NPC's BoxCollider2D
    private bool isPlayerInRange = false; // To track if the player is in range of the NPC

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
        npcCollider = GetComponent<BoxCollider2D>();  // Get the NPC's BoxCollider2D
    }

    void Update()
    {
        // Allow interaction only if the player is within range and presses 'E'
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true; // Player is in range
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false; // Player is out of range
        }
    }

    void StartInteraction()
    {
        if (hasGivenPotion) // If the potion has been given, just show the usedPotionMessage
        {
            messageBox.ShowMultilineMessage(usedPotionMessage, () =>
            {
                EndInteraction();
            });
        }
        else
        {
            // Show the greeting message if potion hasn't been given
            messageBox.ShowMultilineMessage(greetingMessage, () =>
            {
                // After greeting, show the potion offer
                OfferPotion();
            });
        }
    }

    void OfferPotion()
    {
        // Ask the player for Y/N response regarding the potion offer
        isWaitingForResponse = true;
        messageBox.YesNoMessage(potionOfferMessage, AnswerFunc);
    }

    // This function handles the player's response (yes or no)
    void AnswerFunc(bool answer)
    {
        if (answer && !hasGivenPotion)  // If player accepts and has not received the potion
        {
            GivePotion();
        }
        else if (!answer)  // If player declines
        {
            DeclinePotion();
        }
    }

    // If the player accepts the potion
    void GivePotion()
    {
        hasGivenPotion = true;

        // Instantiate the potion at the drop point
        if (potionPrefab != null && potionDropPoint != null)
        {
            Instantiate(potionPrefab, potionDropPoint.position, Quaternion.identity);
        }

        // Show the post potion message
        messageBox.ShowMultilineMessage(postPotionMessage, () =>
        {
            EndInteraction();
        });
    }

    // If the player declines the potion
    void DeclinePotion()
    {
        messageBox.ShowMultilineMessage(declinePotionMessage, () =>
        {
            EndInteraction();
        });
    }

    // End the conversation and reset interaction flags
    void EndInteraction()
    {
        isWaitingForResponse = false;
    }
}
