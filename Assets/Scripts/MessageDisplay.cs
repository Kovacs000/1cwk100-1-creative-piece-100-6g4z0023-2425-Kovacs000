using UnityEngine;
using TMPro;
using System.Collections;

public class MessageDisplay : MonoBehaviour
{
    public Transform messageUI; // UI element for displaying messages
    private TextMeshProUGUI textObject;

    void Start()
    {
        // Get the reference to the TextMeshProUGUI component
        textObject = messageUI.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // Coroutine to display a single message for a specified duration
    IEnumerator DoMessage(string message, float seconds, System.Action onComplete)
    {
        messageUI.gameObject.SetActive(true);
        textObject.text = message;

        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            if (Input.GetMouseButtonDown(0))  // Check for left-click to skip message
                break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        messageUI.gameObject.SetActive(false);
        onComplete?.Invoke(); // Trigger the completion callback
    }

    // Public method to show a single message for a specified time
    public void ShowMessage(string message, float seconds, System.Action onComplete = null)
    {
        StartCoroutine(DoMessage(message, seconds, onComplete));
    }

    // Coroutine to display multiline messages with skip option
    IEnumerator DoMultilineMessage(string message, System.Action onComplete)
    {
        messageUI.gameObject.SetActive(true);
        string[] lines = message.Split('\n'); // Split message by newline

        foreach (string line in lines)
        {
            textObject.text = line;
            float elapsedTime = 0f;

            while (elapsedTime < 5f)  // Adjust time per line if needed
            {
                if (Input.GetMouseButtonDown(0))  // Check for left-click to skip line
                    break;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return null;  // Prevent immediate skipping between lines
        }

        messageUI.gameObject.SetActive(false);
        onComplete?.Invoke(); // Trigger the completion callback
    }

    // Public method to show multiline message
    public void ShowMultilineMessage(string message, System.Action onComplete = null)
    {
        StartCoroutine(DoMultilineMessage(message, onComplete));
    }

    // Coroutine to display Yes/No options and wait for user input
    IEnumerator DoYesNo(string message, System.Action<bool> callback)
    {
        message += "\n(Y/N)";  // Add options to the message
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        bool answer = false;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.N))  // Check if the player presses 'N' for No
            {
                answer = false;
                break;
            }

            if (Input.GetKeyDown(KeyCode.Y))  // Check if the player presses 'Y' for Yes
            {
                answer = true;
                break;
            }

            yield return null;
        }

        messageUI.gameObject.SetActive(false);
        callback(answer);  // Return the player's answer (true for Yes, false for No)
    }

    // Public method to start the Yes/No dialogue
    public void YesNoMessage(string message, System.Action<bool> answerFunc)
    {
        StartCoroutine(DoYesNo(message, answerFunc));
    }
}
