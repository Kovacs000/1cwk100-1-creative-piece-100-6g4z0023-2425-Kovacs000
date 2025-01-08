using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Include for Slider support

public class QuestManager : MonoBehaviour
{
    public int totalRocks = 0;
    public int totalTreeBarks = 0;

    private int destroyedRocks = 0;
    private int destroyedTreeBarks = 0;

    public TextMeshProUGUI questDescriptionText;  // UI text element for quest info
    public Slider questProgressSlider;  // Optional: Slider to display progress

    void Start()
    {
        // Calculate total objects at the start of the game
        totalRocks = GameObject.FindGameObjectsWithTag("Rock").Length;
        totalTreeBarks = GameObject.FindGameObjectsWithTag("TreeBark").Length;

        // Initialize UI elements
        UpdateQuestUI();
    }

    public void DestroyRock()
    {
        destroyedRocks++;
        Debug.Log($"Rocks destroyed: {destroyedRocks}/{totalRocks}");

        UpdateQuestUI(); // Update the quest UI

        if (destroyedRocks >= totalRocks && destroyedTreeBarks >= totalTreeBarks)
        {
            Debug.Log("All objectives completed!");
            questDescriptionText.text = "Quest Complete!";
        }
    }

    public void DestroyTreeBark()
    {
        destroyedTreeBarks++;
        Debug.Log($"Tree barks destroyed: {destroyedTreeBarks}/{totalTreeBarks}");

        UpdateQuestUI(); // Update the quest UI

        if (destroyedRocks >= totalRocks && destroyedTreeBarks >= totalTreeBarks)
        {
            Debug.Log("All objectives completed!");
            questDescriptionText.text = "Quest Complete!";
        }
    }

    void UpdateQuestUI()
    {
        // Update the quest description
        questDescriptionText.text = "Quest: Destroy " + totalRocks + " Rocks and " + totalTreeBarks + " Tree Barks";

        // Update progress (optional progress bar)
        float progress = (float)(destroyedRocks + destroyedTreeBarks) / (totalRocks + totalTreeBarks);
        questProgressSlider.value = progress;

        // Update specific progress text if needed (optional)
        questDescriptionText.text += "\nRocks: " + destroyedRocks + "/" + totalRocks;
        questDescriptionText.text += "\nTree Barks: " + destroyedTreeBarks + "/" + totalTreeBarks;
    }
}
