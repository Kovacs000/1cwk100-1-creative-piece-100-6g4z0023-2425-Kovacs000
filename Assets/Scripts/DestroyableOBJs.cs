using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public string objectType; // "Rock" or "TreeBark"
    private QuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    void OnMouseDown()  // Or any interaction mechanism like collision or proximity
    {
        if (questManager != null)
        {
            if (objectType == "Rock")
            {
                questManager.DestroyRock(); // Increment rock count
            }
            else if (objectType == "TreeBark")
            {
                questManager.DestroyTreeBark(); // Increment tree bark count
            }
        }

        // Only destroy this object
        Destroy(gameObject);
    }
}
