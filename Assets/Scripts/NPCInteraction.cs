using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string questIntroMessage = "I need help clearing rocks and tree barks around town. It's easier with a sword! Will you help me? (Y/N)";
    public string questAcceptedMessage = "Thank you! Please clear the rocks and tree barks for me.";
    public string questDeclinedMessage = "That's okay! Let me know if you change your mind.";
    public string questCompletionMessage = "Thank you for clearing the debris! Here's your reward.";

    private bool isQuestStarted = false;
    private bool isQuestCompleted = false;
    private bool isPlayerNear = false;
    private bool waitingForResponse = false;

    private NPCPatrol npcPatrolScript;
    private MessageDisplay messageDisplay;

    // Reference to the QuestManager
    public QuestManager questManager;

    void Start()
    {
        npcPatrolScript = GetComponent<NPCPatrol>();
        messageDisplay = FindObjectOfType<MessageDisplay>();  // Dynamically find the MessageDisplay component in the scene

        if (questManager == null)
        {
            Debug.LogError("QuestManager reference is not set in NPCInteraction!");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))  // 'E' to interact
        {
            InteractWithNPC();
        }

        if (waitingForResponse)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                AcceptQuest();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                DeclineQuest();
            }
        }
    }

    void InteractWithNPC()
    {
        npcPatrolScript.StopPatrol(); // Stop NPC patrol for interaction

        if (questManager != null)
        {
            // Get quest names from QuestManager
            string treeBarksQuestName = questManager.destroyTreeBarksQuestName;
            string rocksQuestName = questManager.destroyRocksQuestName;

            // Get progress and completion status from QuestManager
            int treeBarkProgress = questManager.GetQuestProgress(treeBarksQuestName);
            int rockProgress = questManager.GetQuestProgress(rocksQuestName);
            bool treeBarksCompleted = questManager.IsQuestCompleted(treeBarksQuestName);
            bool rocksCompleted = questManager.IsQuestCompleted(rocksQuestName);

            // Check if both quests are fully completed
            if (treeBarksCompleted && rocksCompleted && treeBarkProgress == 11 && rockProgress == 8)
            {
                messageDisplay.ShowMessage(questCompletionMessage, 5f, OnMessageComplete);
            }
            else
            {
                // Handle partially completed or unstarted quests
                if (rocksCompleted && !treeBarksCompleted)
                {
                    messageDisplay.ShowMessage("Please clear the remaining tree barks for me.", 3f, OnMessageComplete);
                }
                else if (!rocksCompleted && treeBarksCompleted)
                {
                    messageDisplay.ShowMessage("Please clear the remaining rocks for me.", 3f, OnMessageComplete);
                }
                else if (!waitingForResponse)
                {
                    // Show quest intro if neither quest is completed
                    messageDisplay.ShowMessage(questIntroMessage, 5f, OnQuestIntroComplete);
                    waitingForResponse = true;
                }
            }
        }
    }



    // When the quest intro message is shown, we prompt for Yes/No
    void OnQuestIntroComplete()
    {
        if (waitingForResponse)
        {
            // Show the Yes/No message after quest intro
            messageDisplay.YesNoMessage(questIntroMessage, AnswerFunc);
        }
    }

    // This method handles the player's response to the quest (Y or N)
    void AnswerFunc(bool answer)
    {
        if (answer)  // If the player accepted the quest
        {
            AcceptQuest();
        }
        else  // If the player declined the quest
        {
            DeclineQuest();
        }
    }

    void AcceptQuest()
    {
        isQuestStarted = true;
        waitingForResponse = false;
        messageDisplay.ShowMessage(questAcceptedMessage, 3f, OnMessageComplete);

        // Start the quest in the QuestManager
        questManager.StartQuest(questManager.destroyRocksQuestName);
        questManager.StartQuest(questManager.destroyTreeBarksQuestName);
    }

    void DeclineQuest()
    {
        waitingForResponse = false;
        messageDisplay.ShowMessage(questDeclinedMessage, 3f, OnMessageComplete);
    }

    void OnMessageComplete()
    {
        npcPatrolScript.ResumePatrol();  // Resume NPC patrol after the message is complete
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
            waitingForResponse = false;
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
        messageDisplay.ShowMessage(questCompletionMessage, 5f, OnMessageComplete);

        // Mark the quest as completed in the QuestManager
        questManager.CompleteQuest(questManager.destroyRocksQuestName);
        questManager.CompleteQuest(questManager.destroyTreeBarksQuestName);
    }
}
