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
        messageDisplay = FindObjectOfType<MessageDisplay>();  // Find MessageDisplay component in the scene

        if (questManager == null)
        {
            Debug.LogError("QuestManager reference is not set in NPCInteraction!");
        }
    }

    void Update()
    {
        // Handle player interaction if near NPC
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))  // 'E' to interact
        {
            InteractWithNPC();
        }

        // Handle player response (Yes/No) to quest offer
        if (waitingForResponse)
        {
            if (Input.GetKeyDown(KeyCode.Y)) AcceptQuest();
            else if (Input.GetKeyDown(KeyCode.N)) DeclineQuest();
        }
    }

    void InteractWithNPC()
    {
        npcPatrolScript.StopPatrol(); // Stop NPC patrol for interaction

        if (questManager != null)
        {
            string treeBarksQuestName = questManager.destroyTreeBarksQuestName;
            string rocksQuestName = questManager.destroyRocksQuestName;

            // Get quest progress and completion status from QuestManager
            int treeBarkProgress = questManager.GetQuestProgress(treeBarksQuestName);
            int rockProgress = questManager.GetQuestProgress(rocksQuestName);
            bool treeBarksCompleted = questManager.IsQuestCompleted(treeBarksQuestName);
            bool rocksCompleted = questManager.IsQuestCompleted(rocksQuestName);

            // Check quest completion status
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
                    messageDisplay.ShowMessage(questIntroMessage, 5f, OnQuestIntroComplete);
                    waitingForResponse = true;
                }
            }
        }
    }

    void OnQuestIntroComplete()
    {
        if (waitingForResponse)
        {
            // Prompt for Yes/No after quest intro
            messageDisplay.YesNoMessage(questIntroMessage, AnswerFunc);
        }
    }

    // Handles the player's response (Yes/No) to the quest offer
    void AnswerFunc(bool answer)
    {
        if (answer) AcceptQuest();
        else DeclineQuest();
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
        npcPatrolScript.ResumePatrol();  // Resume NPC patrol after the message
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

    public void CompleteQuest()
    {
        isQuestCompleted = true;
        messageDisplay.ShowMessage(questCompletionMessage, 5f, OnMessageComplete);

        // Mark quests as completed in QuestManager
        questManager.CompleteQuest(questManager.destroyRocksQuestName);
        questManager.CompleteQuest(questManager.destroyTreeBarksQuestName);
    }
}
