 using UnityEngine;

public class NPC1 : MonoBehaviour
{
    MessageDisplay messageBox;

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
    }

    void AnswerFunc(bool answer)
    {
        if (answer)
        {
            messageBox.ShowMultilineMessage("Thank you! The blacksmith's house is just beyond the mountain pass. I'll be here when you return.");
            // Implement quest logic here
        }
        else
        {
            messageBox.ShowMultilineMessage("I understand. If you change your mind, the offer still stands.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            messageBox.YesNoMessage("Oh, hello there! Could you help me? I've lost my golden key. I think I left it somewhere near the blacksmith's house... could you take a look??", AnswerFunc);
        }
    }
}