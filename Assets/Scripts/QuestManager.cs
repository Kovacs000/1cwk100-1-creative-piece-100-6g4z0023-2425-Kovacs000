using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public GameObject questUI; // Reference to the Quest UI panel
    public TextMeshProUGUI questText; // Reference to the TextMeshPro component for quests' descriptions
    public TextMeshProUGUI questProgressText; // Reference to the TextMeshPro component for quests' progress

    public Transform playerTransform; // Add a reference to the player's transform

    private Dictionary<string, string> questDescriptions = new Dictionary<string, string>();
    private Dictionary<string, int> questProgress = new Dictionary<string, int>();
    private Dictionary<string, bool> questCompletion = new Dictionary<string, bool>();
    private Dictionary<string, string> questStatus = new Dictionary<string, string>(); // New dictionary for quest status

    // Quest names as example
    public string goldenKeyQuestName = "Golden Key";
    public string destroyRocksQuestName = "Clearing the path (Rocks)";
    public string destroyTreeBarksQuestName = "Clearing the path (Tree Barks)";
    public float maxInteractionDistance = 5f; // Maximum distance to interact with objects

    void Start()
    {
        // Add quests manually
        questDescriptions[goldenKeyQuestName] = "Retrieve the golden key from the blacksmith's house."; // Initial description
        questDescriptions[destroyRocksQuestName] = "Destroy rocks around the town.";
        questDescriptions[destroyTreeBarksQuestName] = "Destroy tree barks around the town.";

        // Initialize quest progress and completion
        questProgress[goldenKeyQuestName] = 0;  // Golden Key starts with 0/1 progress
        questProgress[destroyRocksQuestName] = 0;
        questProgress[destroyTreeBarksQuestName] = 0;

        questCompletion[goldenKeyQuestName] = false;  // Golden Key quest is not complete at the start
        questCompletion[destroyRocksQuestName] = false;
        questCompletion[destroyTreeBarksQuestName] = false;

        // Initialize quest status
        questStatus[goldenKeyQuestName] = "Not Started";  // Golden Key starts as Not Started
        questStatus[destroyRocksQuestName] = "Not Started";
        questStatus[destroyTreeBarksQuestName] = "Not Started";

        // Ensure the Quest UI is hidden initially
        if (questUI != null)
        {
            questUI.SetActive(false);
        }

        // Update the quest UI initially
        UpdateQuestUI();

        // Debug: Check if all references are assigned
        if (questUI == null) Debug.LogError("questUI is not assigned!");
        if (questText == null) Debug.LogError("questText is not assigned!");
        if (questProgressText == null) Debug.LogError("questProgressText is not assigned!");

        // Ensure playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not assigned!");
        }
    }

    void Update()
    {
        // Check for the 'J' key press to toggle the quest UI
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleQuestUI();
        }
    }

    // Method to toggle the Quest UI visibility
    private void ToggleQuestUI()
    {
        if (questUI != null)
        {
            questUI.SetActive(!questUI.activeSelf);  // Toggle the active state
        }
    }

    // Update the Quest UI with all current quests' statuses
    public void UpdateQuestUI()
    {
        if (questUI != null && questText != null && questProgressText != null)
        {
            string questInfo = "";
            string progressInfo = "";

            // Temporary variable to hold the updated quest descriptions
            Dictionary<string, string> updatedDescriptions = new Dictionary<string, string>(questDescriptions);

            // Loop through each quest and display its current status
            foreach (KeyValuePair<string, string> quest in questDescriptions)
            {
                string questName = quest.Key;
                string description = quest.Value;

                // Update quest description for "Golden Key" based on its status
                if (questName == goldenKeyQuestName)
                {
                    if (questStatus[goldenKeyQuestName] == "In Progress" && questProgress[goldenKeyQuestName] == 1)
                    {
                        // Once the player has collected the key (progress == 1), update description to return the key
                        updatedDescriptions[goldenKeyQuestName] = "Return the golden key to the NPC.";
                    }
                    else if (questStatus[goldenKeyQuestName] == "Not Started")
                    {
                        // If the quest hasn't started, show "Retrieve the golden key" description
                        updatedDescriptions[goldenKeyQuestName] = "Retrieve the golden key from the blacksmith's house.";
                    }
                    else if (questStatus[goldenKeyQuestName] == "Completed")
                    {
                        updatedDescriptions[goldenKeyQuestName] = "Quest completed: Golden Key returned."; // Updated description for completion
                    }
                }

                // Get the status and progress for each quest
                string status = questStatus[questName];
                int progress = questProgress[questName];
                bool isComplete = questCompletion[questName];

                // Update the progress info based on quest status
                if (questName == goldenKeyQuestName)
                {
                    // Golden Key quest should have 1/1 progress
                    progressInfo += $"{questName}: {progress}/1 - {status}\n";
                }
                else if (questName == destroyRocksQuestName)
                {
                    // For the Rock quest, update to 8/8 progress
                    progressInfo += $"{questName}: {progress}/8 - {status}\n";
                }
                else if (questName == destroyTreeBarksQuestName)
                {
                    // For the Tree Bark quest, update to 11/11 progress
                    progressInfo += $"{questName}: {progress}/11 - {status}\n";
                }

                questInfo += $"{questName}: {updatedDescriptions[questName]}\n"; // Use the updated description
            }

            questText.text = questInfo; // Set quest descriptions
            questProgressText.text = progressInfo; // Set quest progress
        }
    }

    // Method to get the status of a quest
    public string GetQuestStatus(string questName)
    {
        if (questStatus.ContainsKey(questName))
        {
            return questStatus[questName];
        }
        return "Unknown"; // If the quest doesn't exist
    }

    // Method to mark a quest as complete
    public void CompleteQuest(string questName)
    {
        if (questCompletion.ContainsKey(questName))
        {
            questCompletion[questName] = true;
            questStatus[questName] = "Completed";  // Update status to "Completed"
            UpdateQuestUI(); // Update the UI after completing the quest
        }
    }

    // Method to start a quest (when the player accepts it)
    public void StartQuest(string questName)
    {
        if (questProgress.ContainsKey(questName))
        {
            questProgress[questName] = 0; // Reset progress to 0 when starting the quest
            questCompletion[questName] = false; // Ensure the quest is marked as not complete
            questStatus[questName] = "In Progress";  // Change status to "In Progress"
            Debug.Log($"Quest {questName} started. Status: {questStatus[questName]}");
            UpdateQuestUI(); // Update the UI after starting the quest
        }
    }

    // Example to handle progress in the QuestManager for specific quests
    public void DestroyRock(Transform rockTransform)
    {
        if (questProgress.ContainsKey(destroyRocksQuestName) && !questCompletion[destroyRocksQuestName])
        {
            if (questStatus[destroyRocksQuestName] == "Not Started")
            {
                questStatus[destroyRocksQuestName] = "In Progress";  // Update status to "In Progress"
                UpdateQuestUI();  // Update UI immediately to show the change
            }

            // Check distance to player
            if (Vector3.Distance(playerTransform.position, rockTransform.position) <= maxInteractionDistance)
            {
                questProgress[destroyRocksQuestName] += 1;  // Increment progress by 1
                if (questProgress[destroyRocksQuestName] >= 8) // Example: quest complete when progress reaches 8
                {
                    CompleteQuest(destroyRocksQuestName);
                }
                UpdateQuestUI();  // Update the UI after progress
            }
            else
            {
                Debug.Log("You are too far to destroy this rock.");
            }
        }
    }

    public void DestroyTreeBark(Transform treeBarkTransform)
    {
        if (questProgress.ContainsKey(destroyTreeBarksQuestName) && !questCompletion[destroyTreeBarksQuestName])
        {
            if (questStatus[destroyTreeBarksQuestName] == "Not Started")
            {
                questStatus[destroyTreeBarksQuestName] = "In Progress";  // Update status to "In Progress"
                UpdateQuestUI();  // Update UI immediately to show the change
            }

            // Check distance to player
            if (Vector3.Distance(playerTransform.position, treeBarkTransform.position) <= maxInteractionDistance)
            {
                questProgress[destroyTreeBarksQuestName] += 1;  // Increment progress by 1
                if (questProgress[destroyTreeBarksQuestName] >= 11) // Example: quest complete when progress reaches 11
                {
                    CompleteQuest(destroyTreeBarksQuestName);
                }
                UpdateQuestUI();  // Update the UI after progress
            }
            else
            {
                Debug.Log("You are too far to destroy this tree bark.");
            }
        }
    }

    // Method to update Golden Key progress when the key is collected
    public void CollectGoldenKey()
    {
        if (!questCompletion[goldenKeyQuestName])
        {
            questProgress[goldenKeyQuestName] = 1; // Set progress to 1/1
            questStatus[goldenKeyQuestName] = "Return the Key"; // Update the status to "Return the Key"
            UpdateQuestUI(); // Update the UI after collecting the key
        }
    }

    public int GetQuestProgress(string questName)
    {
        if (questProgress.ContainsKey(questName))
        {
            return questProgress[questName];
        }
        return 0;
    }

    public bool IsQuestCompleted(string questName)
    {
        if (questCompletion.ContainsKey(questName))
        {
            return questCompletion[questName];
        }
        return false;
    }
}
