using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string questIntroMessage = "I need help clearing rocks and tree barks around town. It's easier with a sword!";
    public string questInProgressMessage = "Please clear the rocks and tree barks for me.";
    public string questCompletionMessage = "Thank you for clearing the debris! Here's your reward.";
    private bool isQuestStarted = false;
    private bool isQuestCompleted = false;
    private bool isPlayerNear = false;

    private NPCPatrol npcPatrolScript;
    private MessageDisplay messageDisplay;

    void Start()
    {
        npcPatrolScript = GetComponent<NPCPatrol>();
        messageDisplay = FindObjectOfType<MessageDisplay>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))  // 'E' to interact
        {
            InteractWithNPC();
        }
    }

    void InteractWithNPC()
    {
        npcPatrolScript.StopPatrol();  // Stop NPC movement

        if (!isQuestStarted)
        {
            // Show quest intro and start the quest
            isQuestStarted = true;
            messageDisplay.ShowMessage(questIntroMessage, 5f);
        }
        else if (!isQuestCompleted)
        {
            // Player still needs to complete the quest
            messageDisplay.ShowMessage(questInProgressMessage, 3f);
        }
        else
        {
            // Quest is completed
            messageDisplay.ShowMessage(questCompletionMessage, 5f);
        }

        // Resume patrol after the message
        Invoke("ResumePatrol", 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    void ResumePatrol()
    {
        npcPatrolScript.ResumePatrol();
    }

    // Called when the quest is completed
    public void CompleteQuest()
    {
        isQuestCompleted = true;
    }
}
