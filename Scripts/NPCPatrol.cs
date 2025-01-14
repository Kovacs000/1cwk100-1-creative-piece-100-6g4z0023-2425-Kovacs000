using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Patrol points for the NPC to move between
    public float patrolSpeed = 2.0f; // Speed of the NPC's patrol
    private int currentPatrolIndex = 0; // Index to track the current patrol point
    private Rigidbody2D rb; // Rigidbody2D to move the NPC
    private Animator anim; // Animator to handle NPC animations

    private bool isPaused = false; // Flag to check if the patrol is paused

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>();  // Get the Animator component
    }

    void Update()
    {
        // Move the NPC to the next patrol point if not paused
        if (!isPaused)
        {
            MoveToNextPatrolPoint();
        }
    }

    void MoveToNextPatrolPoint()
    {
        // If there are no patrol points, return early
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex]; // Get the current patrol point
        Vector2 direction = (targetPoint.position - transform.position).normalized; // Direction to the target point

        // If the NPC is close to the target point, stop and move to the next point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            rb.velocity = Vector2.zero; // Stop movement
            anim.SetBool("IsWalking", false); // Set walking animation to false
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Move to the next patrol point
            return;
        }

        // Continue moving towards the target point
        rb.velocity = direction * patrolSpeed; // Apply velocity for movement
        UpdateMovementAnimation(direction); // Update animation based on movement
    }

    void UpdateMovementAnimation(Vector2 direction)
    {
        // Set the animation parameters based on the movement direction
        float xMovement = Mathf.Round(direction.x);
        float yMovement = Mathf.Round(direction.y);

        anim.SetFloat("xMovement", xMovement);
        anim.SetFloat("yMovement", yMovement);

        // If the NPC is moving, set the walking animation to true
        bool isWalking = direction.magnitude > 0.1f;
        anim.SetBool("IsWalking", isWalking);
    }

    // Method to stop the patrol (pause the NPC)
    public void StopPatrol()
    {
        isPaused = true; // Set pause flag to true
        rb.velocity = Vector2.zero; // Stop movement
        anim.SetBool("IsWalking", false); // Set walking animation to false
    }

    // Method to resume the patrol (unpause the NPC)
    public void ResumePatrol()
    {
        isPaused = false; // Set pause flag to false
    }
}
