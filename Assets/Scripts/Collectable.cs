using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<Inventory>().Add(itemName, 1);
            MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
            disp.ShowMessage("You picked up a " + itemName, 2.0f);
            Destroy(gameObject);
        }
    }
}
