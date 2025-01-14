using UnityEngine;

public class PotionNPC : MonoBehaviour
{
    private MessageDisplay messageBox;       // Reference to message box for displaying messages
    private bool hasGivenPotion = false;     // Track if the potion has already been given
    private bool isWaitingForResponse = false;  // Track if the NPC is waiting for a response from the player

    // Potion and drop point references
    public GameObject potionPrefab;          // The potion prefab to be dropped
    public Transform potionDropPoint;        // The point where the potion will appear

    // Messages for NPC interactions
    private string greetingMessage = "Hello, traveler. There is a rock blocking the path below the mountain. I can offer you a potion that grants temporary magic to help destroy it.";
    private string potionOfferMessage = "Would you like to accept this potion? It grants temporary magic to destroy the rock. It’s a powerful potion, but be cautious.";
    private string postPotionMessage = "You accepted the potion. Use it wisely, traveler, as its power fades quickly.";
    private string declinePotionMessage = "It’s your loss. The rock won’t move on its own.";
    private string usedPotionMessage = "I just gave you the potion. Have you used it yet?";

    private BoxCollider2D npcCollider;      // Reference to NPC's collider for interaction detection
    private bool isPlayerInRange = false;   // Track if the player is in range for interaction

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>(); // Find the MessageDisplay object
        npcCollider = GetComponent<BoxCollider2D>(); // Get NPC's BoxCollider2D for trigger interaction
    }

    void Update()
    {
        // Allow interaction when the player presses 'E' and is in range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();  // Start the interaction when player presses 'E'
        }
    }

    // When the player enters the NPC's collider range
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true; // Player is within range to interact
        }
    }

    // When the player exits the NPC's collider range
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false; // Player is no longer in range
        }
    }

    // Start interaction with the NPC
    void StartInteraction()
    {
        if (hasGivenPotion)  // If the potion has already been given
        {
            messageBox.ShowMultilineMessage(usedPotionMessage, EndInteraction);
        }
        else  // If the potion hasn't been given yet
        {
            messageBox.ShowMultilineMessage(greetingMessage, OfferPotion);  // Show greeting message
        }
    }

    // Offer the potion to the player
    void OfferPotion()
    {
        isWaitingForResponse = true;  // Wait for player response (Yes/No)
        messageBox.YesNoMessage(potionOfferMessage, AnswerFunc);  // Ask the player if they want the potion
    }

    // Handle player's Yes/No response
    void AnswerFunc(bool answer)
    {
        if (answer && !hasGivenPotion)  // Player accepted the potion
        {
            GivePotion();
        }
        else if (!answer)  // Player declined the potion
        {
            DeclinePotion();
        }
    }

    // Give the potion to the player
    void GivePotion()
    {
        hasGivenPotion = true;  // Set potion as given

        // Instantiate the potion prefab at the drop point
        if (potionPrefab != null && potionDropPoint != null)
        {
            Instantiate(potionPrefab, potionDropPoint.position, Quaternion.identity);
        }

        // Show the message after giving the potion
        messageBox.ShowMultilineMessage(postPotionMessage, EndInteraction);
    }

    // Handle the case where the player declines the potion
    void DeclinePotion()
    {
        messageBox.ShowMultilineMessage(declinePotionMessage, EndInteraction);
    }

    // End the interaction and reset flags
    void EndInteraction()
    {
        isWaitingForResponse = false;  // Reset waiting for response
    }
}
