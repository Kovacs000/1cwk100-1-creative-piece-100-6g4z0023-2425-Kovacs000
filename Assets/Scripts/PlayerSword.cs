using UnityEngine;
using System.Collections;  // This is the missing directive for IEnumerator

public class PlayerSword : MonoBehaviour
{
    public Animator animator;         // Reference to the Animator
    public float swordSwingDuration = 0.5f; // Duration for which the sword swing animation lasts
    private bool isSwinging = false;  // Flag to prevent multiple slashes at once

    private int direction = 0;  // 0 = Down, 1 = Right, -1 = Left, 2 = Up (you can expand if needed)

    void Update()
    {
        // Update direction based on player movement
        UpdatePlayerDirection();

        // Detect left mouse button click (0 = left click) to trigger sword swing
        if (Input.GetMouseButtonDown(0) && !isSwinging) // Left Click is 0
        {
            StartCoroutine(SwordSwing());
        }
    }

    private void UpdatePlayerDirection()
    {
        // This is a simple example; you can adapt this to use velocity or player movement
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = 2; // Facing Up
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 1; // Facing Right
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = -1; // Facing Left
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = 0; // Facing Down
        }

        // Optionally, you can add logic based on the player's velocity or character rotation.
    }

    private IEnumerator SwordSwing()
    {
        isSwinging = true;

        // Set the direction and trigger the sword swing animation
        animator.SetInteger("Direction", direction);  // Update the Direction parameter for correct animation
        animator.SetBool("IsSwinging", true);  // Trigger sword swing animation

        // Wait for the sword swing animation to finish based on the set duration
        yield return new WaitForSeconds(swordSwingDuration);

        // After the swing animation finishes, set IsSwinging to false
        animator.SetBool("IsSwinging", false);

        isSwinging = false;
    }
}
