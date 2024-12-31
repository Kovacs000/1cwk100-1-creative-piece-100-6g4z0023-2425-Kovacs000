using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 4.0f;
    public float sprintMultiplier = 1.5f;
    private float currentSpeed;

    private Vector2 moveInput;
    private float xSpeed;
    private float ySpeed;

    private float lastMovementTime;
    private float movementThreshold = 0.2f;
    private bool wasMovingUp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = speed;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical).normalized;
        rb.velocity = moveInput * currentSpeed;

        xSpeed = moveInput.x;
        ySpeed = moveInput.y;

        if (moveInput.magnitude > 0)
        {
            lastMovementTime = Time.time;
        }
    }

    void Update()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        anim.SetBool("IsSprinting", isSprinting);

        bool isMoving = moveInput.magnitude > 0;

        anim.SetFloat("xspeed", xSpeed);
        anim.SetFloat("yspeed", ySpeed);
        anim.SetBool("IsMoving", isMoving);

        wasMovingUp = ySpeed > 0 && (Time.time - lastMovementTime <= movementThreshold);

        if (!isMoving)
        {
            anim.Play(wasMovingUp ? "IdleUp" : "IdleDown");
        }
    }
}
