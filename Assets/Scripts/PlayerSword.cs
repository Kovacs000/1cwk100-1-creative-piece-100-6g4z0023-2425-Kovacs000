using UnityEngine;
using System.Collections;  // Required for IEnumerator

public class PlayerSword : MonoBehaviour
{
    public Animator animator;            // Animator reference to control animations
    public float swordSwingDuration = 0.5f; // Time the sword swing animation lasts
    private bool isSwinging = false;     // Prevent multiple swings at the same time

    private int direction = 0;           // Direction of the sword swing (0 = Down, 1 = Right, -1 = Left, 2 = Up)

    void Update()
    {
        // Update direction based on player input (arrow keys)
        UpdatePlayerDirection();

        // Trigger sword swing on left mouse button click, if not already swinging
        if (Input.GetMouseButtonDown(0) && !isSwinging) // Left mouse click (0)
        {
            StartCoroutine(SwordSwing()); // Start the sword swing coroutine
        }
    }

    // Update the player's direction based on arrow key input
    private void UpdatePlayerDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow)) direction = 2;  // Facing Up
        else if (Input.GetKey(KeyCode.RightArrow)) direction = 1; // Facing Right
        else if (Input.GetKey(KeyCode.LeftArrow)) direction = -1; // Facing Left
        else if (Input.GetKey(KeyCode.DownArrow)) direction = 0; // Facing Down
    }

    // Coroutine for the sword swing animation
    private IEnumerator SwordSwing()
    {
        isSwinging = true;  // Prevent multiple swings at once

        // Set the correct direction for the sword swing animation
        animator.SetInteger("Direction", direction);  // Update the Direction parameter in Animator
        animator.SetBool("IsSwinging", true);         // Trigger the sword swing animation

        // Wait for the swing animation to finish based on the duration
        yield return new WaitForSeconds(swordSwingDuration);

        // After the animation finishes, reset IsSwinging
        animator.SetBool("IsSwinging", false);

        isSwinging = false;  // Allow the player to swing again
    }
}
