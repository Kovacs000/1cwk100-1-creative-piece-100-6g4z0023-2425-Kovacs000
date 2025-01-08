using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.0f;
    private int currentPatrolIndex = 0;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isPaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isPaused) MoveToNextPatrolPoint();
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
        float xMovement = Mathf.Round(direction.x);
        float yMovement = Mathf.Round(direction.y);

        anim.SetFloat("xMovement", xMovement);
        anim.SetFloat("yMovement", yMovement);

        bool isWalking = direction.magnitude > 0.1f;
        anim.SetBool("IsWalking", isWalking);
    }

    public void StopPatrol()
    {
        isPaused = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("IsWalking", false);
    }

    public void ResumePatrol()
    {
        isPaused = false;
    }
}
