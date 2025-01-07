using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.0f;
    private int currentPatrolIndex = 0;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned!");
        }
    }

    void Update()
    {
        MoveToNextPatrolPoint();
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("IsWalking", false);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            return;
        }

        rb.velocity = direction * patrolSpeed;
        UpdateMovementAnimation(direction);
    }

    void UpdateMovementAnimation(Vector2 direction)
    {
        // Normalize and round direction to nearest whole number
        float xMovement = Mathf.Round(direction.x);
        float yMovement = Mathf.Round(direction.y);

        // Update animator parameters
        anim.SetFloat("xMovement", xMovement);
        anim.SetFloat("yMovement", yMovement);

        // Determine if NPC is walking
        bool isWalking = direction.magnitude > 0.1f;
        anim.SetBool("IsWalking", isWalking);
    }

    // Method to stop patrol
    public void StopPatrol()
    {
        rb.velocity = Vector2.zero;
    }

    // Method to resume patrol
    public void ResumePatrol()
    {
        MoveToNextPatrolPoint();
    }
}
