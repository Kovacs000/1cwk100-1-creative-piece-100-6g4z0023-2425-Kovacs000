using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public GameObject itemPrefab;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Inventory playerInventory = coll.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.Add(itemName, 1);
                MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
                disp.ShowMessage("You picked up a " + itemName, 2.0f);
                Destroy(gameObject);
            }
        }
    }
}
