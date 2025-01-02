using System;
using UnityEngine;
using TMPro; // For TextMeshPro

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

    public GameObject goldenKeyPrefab; // Reference to the golden key prefab

    public Transform inventoryUI; // Assign this in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = speed;

        if (inventoryUI == null)
        {
            Debug.LogError("Inventory UI not assigned!");
        }
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
            lastMovementTime = Time.time; // Keep track of when the player last moved
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
            anim.SetBool("IsIdleUp", wasMovingUp);
        }

        HandleInventoryInput();
        HandleQuestInteractions();
    }

    void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI == null) return;

            if (!inventoryUI.gameObject.activeSelf)
            {
                Inventory inventory = GetComponent<Inventory>();
                if (inventory != null)
                {
                    string inv = inventory.GetInventoryString();
                    inventoryUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv;
                    inventoryUI.gameObject.SetActive(true);
                }
            }
            else
            {
                inventoryUI.gameObject.SetActive(false);
            }
        }
    }

    void HandleQuestInteractions()
    {
        if (goldenKeyPrefab != null && Vector2.Distance(transform.position, goldenKeyPrefab.transform.position) < 1f)
        {
            Destroy(goldenKeyPrefab); // Destroy the key after pickup
        }
    }
}
