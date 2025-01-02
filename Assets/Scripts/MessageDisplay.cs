using System.Collections;
using UnityEngine;
using TMPro;

public class MessageDisplay : MonoBehaviour
{
    public Transform messageUI; // UI element for displaying messages
    private TextMeshProUGUI textObject;

    void Start()
    {
        textObject = messageUI.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    IEnumerator DoMessage(string message, float seconds)
    {
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        yield return new WaitForSeconds(seconds);
        messageUI.gameObject.SetActive(false);
    }

    public void ShowMessage(string message, float seconds)
    {
        StartCoroutine(DoMessage(message, seconds));
    }

    IEnumerator DoMultilineMessage(string message)
    {
        messageUI.gameObject.SetActive(true);
        string[] lines = message.Split('\n');
        foreach (string line in lines)
        {
            textObject.text = line;
            yield return null;
            while (true)
            {
                if (Input.GetButtonDown("Fire1"))
                    break;
                yield return null;
            }
        }
        messageUI.gameObject.SetActive(false);
    }

    public void ShowMultilineMessage(string message)
    {
        StartCoroutine(DoMultilineMessage(message));
    }

    IEnumerator DoYesNo(string message, System.Action<bool> callback)
    {
        message += "\n(Y/N)";
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        bool answer = false;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                answer = false;
                break;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                answer = true;
                break;
            }
            yield return null;
        }
        messageUI.gameObject.SetActive(false);
        callback(answer);
    }

    public void YesNoMessage(string message, System.Action<bool> answerFunc)
    {
        StartCoroutine(DoYesNo(message, answerFunc));
    }
}
