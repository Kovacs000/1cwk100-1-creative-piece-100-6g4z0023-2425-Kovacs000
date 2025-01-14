using UnityEngine;
using TMPro;
using System.Collections;

public class MessageDisplay : MonoBehaviour
{
    public Transform messageUI; // UI element for displaying messages
    private TextMeshProUGUI textObject;

    void Start()
    {
        textObject = messageUI.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // Displays a single message for a given time
    IEnumerator DoMessage(string message, float seconds, System.Action onComplete)
    {
        messageUI.gameObject.SetActive(true);
        textObject.text = message;

        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            if (Input.GetMouseButtonDown(0))  // Left-click to skip
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        messageUI.gameObject.SetActive(false);
        onComplete?.Invoke();  // Call the callback when the message is done
    }

    // Displays a single message for a given time (public method)
    public void ShowMessage(string message, float seconds, System.Action onComplete = null)
    {
        StartCoroutine(DoMessage(message, seconds, onComplete));
    }

    // Displays multiline messages and allows skipping with left-click
    IEnumerator DoMultilineMessage(string message, System.Action onComplete)
    {
        messageUI.gameObject.SetActive(true);
        string[] lines = message.Split('\n');

        foreach (string line in lines)
        {
            textObject.text = line;
            float elapsedTime = 0f;

            while (elapsedTime < 5f)  // Time per line (can be adjusted)
            {
                if (Input.GetMouseButtonDown(0))  // Left-click to skip
                {
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return null;  // This prevents skipping immediately between lines
        }

        messageUI.gameObject.SetActive(false);
        onComplete?.Invoke();  // Call the callback when done
    }

    // Displays multiline messages (public method)
    public void ShowMultilineMessage(string message, System.Action onComplete = null)
    {
        StartCoroutine(DoMultilineMessage(message, onComplete));
    }

    // Displays a Yes/No message and waits for player input
    IEnumerator DoYesNo(string message, System.Action<bool> callback)
    {
        message += "\n(Y/N)";
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        bool answer = false;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.N))  // If player presses N for no
            {
                answer = false;
                break;
            }

            if (Input.GetKeyDown(KeyCode.Y))  // If player presses Y for yes
            {
                answer = true;
                break;
            }
            yield return null;
        }

        messageUI.gameObject.SetActive(false);
        callback(answer);  // Pass the answer (true for Yes, false for No) to the callback
    }

    // Public method to start Yes/No dialogue
    public void YesNoMessage(string message, System.Action<bool> answerFunc)
    {
        StartCoroutine(DoYesNo(message, answerFunc));
    }
}
