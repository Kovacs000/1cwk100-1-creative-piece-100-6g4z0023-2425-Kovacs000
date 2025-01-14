using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    // References for the Quest UI and text components
    public GameObject questUI; // The Quest UI panel
    public TextMeshProUGUI questText; // The TextMeshPro component for quest descriptions
    public TextMeshProUGUI questProgressText; // The TextMeshPro component for quest progress

    public Transform playerTransform; // Reference to the player's transform

    // Dictionaries to manage quest data
    private Dictionary<string, string> questDescriptions = new Dictionary<string, string>(); // Quest descriptions
    private Dictionary<string, int> questProgress = new Dictionary<string, int>(); // Quest progress (e.g., how many rocks destroyed)
    private Dictionary<string, bool> questCompletion = new Dictionary<string, bool>(); // Whether quests are completed
    private Dictionary<string, string> questStatus = new Dictionary<string, string>(); // Quest status (Not Started, In Progress, Completed)

    // Quest names for different quests
    public string goldenKeyQuestName = "Golden Key";
    public string destroyRocksQuestName = "Clearing the path (Rocks)";
    public string destroyTreeBarksQuestName = "Clearing the path (Tree Barks)";
    public float maxInteractionDistance = 5f; // Max distance to interact with objects

    void Start()
    {
        // Initialize quest descriptions
        questDescriptions[goldenKeyQuestName] = "Retrieve the golden key from the blacksmith's house.";
        questDescriptions[destroyRocksQuestName] = "Destroy rocks around the town.";
        questDescriptions[destroyTreeBarksQuestName] = "Destroy tree barks around the town.";

        // Initialize quest progress and completion
        questProgress[goldenKeyQuestName] = 0;
        questProgress[destroyRocksQuestName] = 0;
        questProgress[destroyTreeBarksQuestName] = 0;

        questCompletion[goldenKeyQuestName] = false;
        questCompletion[destroyRocksQuestName] = false;
        questCompletion[destroyTreeBarksQuestName] = false;

        // Initialize quest status
        questStatus[goldenKeyQuestName] = "Not Started";
        questStatus[destroyRocksQuestName] = "Not Started";
        questStatus[destroyTreeBarksQuestName] = "Not Started";

        // Ensure the Quest UI is hidden initially
        if (questUI != null)
        {
            questUI.SetActive(false);
        }

        // Update the Quest UI initially
        UpdateQuestUI();

        // Debug: Check if all references are assigned
        ValidateReferences();
    }

    void Update()
    {
        // Toggle the quest UI when pressing 'J'
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleQuestUI();
        }
    }

    // Toggle the visibility of the quest UI
    private void ToggleQuestUI()
    {
        if (questUI != null)
        {
            questUI.SetActive(!questUI.activeSelf);
        }
    }

    // Update the Quest UI with all current quests' statuses
    public void UpdateQuestUI()
    {
        if (questUI != null && questText != null && questProgressText != null)
        {
            string questInfo = "";
            string progressInfo = "";

            // Create a temporary dictionary to hold updated quest descriptions
            var updatedDescriptions = new Dictionary<string, string>(questDescriptions);

            // Loop through each quest and display its current status
            foreach (KeyValuePair<string, string> quest in questDescriptions)
            {
                string questName = quest.Key;
                string description = quest.Value;

                // Update the description for the Golden Key quest based on progress
                if (questName == goldenKeyQuestName)
                {
                    if (questStatus[goldenKeyQuestName] == "In Progress" && questProgress[goldenKeyQuestName] == 1)
                    {
                        updatedDescriptions[goldenKeyQuestName] = "Return the golden key to the NPC.";
                    }
                    else if (questStatus[goldenKeyQuestName] == "Not Started")
                    {
                        updatedDescriptions[goldenKeyQuestName] = "Retrieve the golden key from the blacksmith's house.";
                    }
                    else if (questStatus[goldenKeyQuestName] == "Completed")
                    {
                        updatedDescriptions[goldenKeyQuestName] = "Quest completed: Golden Key returned.";
                    }
                }

                // Get the status and progress for each quest
                string status = questStatus[questName];
                int progress = questProgress[questName];
                bool isComplete = questCompletion[questName];

                // Update progress info based on quest
                progressInfo += $"{questName}: {progress}/{GetMaxProgress(questName)} - {status}\n";
                questInfo += $"{questName}: {updatedDescriptions[questName]}\n";
            }

            // Set the quest UI text
            questText.text = questInfo;
            questProgressText.text = progressInfo;
        }
    }

    // Helper method to get the max progress for each quest
    private int GetMaxProgress(string questName)
    {
        if (questName == goldenKeyQuestName) return 1;
        if (questName == destroyRocksQuestName) return 8;
        if (questName == destroyTreeBarksQuestName) return 11;
        return 0;
    }

    // Validate if all references are properly assigned in the Inspector
    private void ValidateReferences()
    {
        if (questUI == null) Debug.LogError("questUI is not assigned!");
        if (questText == null) Debug.LogError("questText is not assigned!");
        if (questProgressText == null) Debug.LogError("questProgressText is not assigned!");
        if (playerTransform == null) Debug.LogError("Player transform is not assigned!");
    }

    // Get the current status of a quest
    public string GetQuestStatus(string questName)
    {
        if (questStatus.ContainsKey(questName))
        {
            return questStatus[questName];
        }
        return "Unknown";
    }

    // Mark a quest as complete
    public void CompleteQuest(string questName)
    {
        if (questCompletion.ContainsKey(questName))
        {
            questCompletion[questName] = true;
            questStatus[questName] = "Completed";
            UpdateQuestUI();
        }
    }

    // Start a quest
    public void StartQuest(string questName)
    {
        if (questProgress.ContainsKey(questName))
        {
            questProgress[questName] = 0;
            questCompletion[questName] = false;
            questStatus[questName] = "In Progress";
            Debug.Log($"Quest {questName} started. Status: {questStatus[questName]}");
            UpdateQuestUI();
        }
    }

    // Handle destroying rocks in the "Clearing the path (Rocks)" quest
    public void DestroyRock(Transform rockTransform)
    {
        if (questProgress.ContainsKey(destroyRocksQuestName) && !questCompletion[destroyRocksQuestName])
        {
            if (questStatus[destroyRocksQuestName] == "Not Started")
            {
                questStatus[destroyRocksQuestName] = "In Progress";
                UpdateQuestUI();
            }

            // Check the distance to the rock
            if (Vector3.Distance(playerTransform.position, rockTransform.position) <= maxInteractionDistance)
            {
                questProgress[destroyRocksQuestName]++;
                if (questProgress[destroyRocksQuestName] >= 8)
                {
                    CompleteQuest(destroyRocksQuestName);
                }
                UpdateQuestUI();
            }
            else
            {
                Debug.Log("You are too far to destroy this rock.");
            }
        }
    }

    // Handle destroying tree barks in the "Clearing the path (Tree Barks)" quest
    public void DestroyTreeBark(Transform treeBarkTransform)
    {
        if (questProgress.ContainsKey(destroyTreeBarksQuestName) && !questCompletion[destroyTreeBarksQuestName])
        {
            if (questStatus[destroyTreeBarksQuestName] == "Not Started")
            {
                questStatus[destroyTreeBarksQuestName] = "In Progress";
                UpdateQuestUI();
            }

            // Check the distance to the tree bark
            if (Vector3.Distance(playerTransform.position, treeBarkTransform.position) <= maxInteractionDistance)
            {
                questProgress[destroyTreeBarksQuestName]++;
                if (questProgress[destroyTreeBarksQuestName] >= 11)
                {
                    CompleteQuest(destroyTreeBarksQuestName);
                }
                UpdateQuestUI();
            }
            else
            {
                Debug.Log("You are too far to destroy this tree bark.");
            }
        }
    }

    // Handle collecting the Golden Key
    public void CollectGoldenKey()
    {
        if (!questCompletion[goldenKeyQuestName])
        {
            questProgress[goldenKeyQuestName] = 1;
            questStatus[goldenKeyQuestName] = "Return the Key";
            UpdateQuestUI();
        }
    }

    // Get the progress of a specific quest
    public int GetQuestProgress(string questName)
    {
        if (questProgress.ContainsKey(questName))
        {
            return questProgress[questName];
        }
        return 0;
    }

    // Check if a specific quest is completed
    public bool IsQuestCompleted(string questName)
    {
        if (questCompletion.ContainsKey(questName))
        {
            return questCompletion[questName];
        }
        return false;
    }
}
