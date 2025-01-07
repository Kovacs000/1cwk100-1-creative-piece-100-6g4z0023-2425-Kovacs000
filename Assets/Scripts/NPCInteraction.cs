using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcMessage = "Hello, traveler! Can you help me?";
    private bool isPlayerNear = false;
    private NPCPatrol npcPatrolScript;
    private MessageDisplay messageDisplay;  // Reference to MessageDisplay script

    void Start()
    {
        npcPatrolScript = GetComponent<NPCPatrol>();
        messageDisplay = FindObjectOfType<MessageDisplay>();  // Find the MessageDisplay component in the scene
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))  // 'E' to interact
        {
            InteractWithNPC();
        }
    }

    private void InteractWithNPC()
    {
        npcPatrolScript.StopPatrol();  // Stop NPC movement
        messageDisplay.ShowMessage(npcMessage, 3f);  // Display the message for 3 seconds

        // Optional: Add logic to wait for interaction to end, then resume patrol
        Invoke("ResumePatrol", 3f);  // Resume patrol after 3 seconds
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;  // Player is in range for interaction
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;  // Player is out of range
        }
    }

    // Method to resume patrol after the interaction
    private void ResumePatrol()
    {
        npcPatrolScript.ResumePatrol();  // Start NPC movement again
    }
}
