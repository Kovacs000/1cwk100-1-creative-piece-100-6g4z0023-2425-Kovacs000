using UnityEngine;
using TMPro;

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

    private bool swordEquipped = false; // Keep track of sword equipped status
    public Transform inventoryUI; // Assign this in the Inspector

    private Vector2 lastDirection; // Keep track of the last movement direction

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
            lastDirection = moveInput; // Store the last direction when moving
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

        if (!isMoving)
        {
            // If not moving, set the appropriate idle animation based on last movement direction
            SetIdleAnimation();
        }

        HandleInventoryInput();
        HandleQuestInteractions();
        HandleEquipInput();
        UpdateWeaponAnimation();
    }

    void SetIdleAnimation()
    {
        // Reset idle animations first to prevent incorrect idle states
        anim.SetBool("IsIdleUp", false);
        anim.SetBool("IsIdleDown", false);
        anim.SetBool("IsIdleLeft", false);
        anim.SetBool("IsIdleRight", false);

        // Set the idle animation based on the last movement direction
        if (lastDirection.y > 0)
        {
            anim.SetBool("IsIdleUp", true);
        }
        else if (lastDirection.y < 0)
        {
            anim.SetBool("IsIdleDown", true);
        }
        else if (lastDirection.x < 0)
        {
            anim.SetBool("IsIdleLeft", true);
        }
        else if (lastDirection.x > 0)
        {
            anim.SetBool("IsIdleRight", true);
        }
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
        // Handle quest interactions (e.g., golden key pickup)
    }

    void HandleEquipInput()
    {
        // If "1" is pressed, toggle sword equip
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!swordEquipped) // If sword is not equipped
            {
                EquipSword();
            }
            else // If sword is already equipped, unequip it
            {
                UnequipSword();
            }
        }
    }

    void EquipSword()
    {
        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null && inventory.GetCount("Sword") > 0)
        {
            swordEquipped = true; // Set sword as equipped
            inventory.Remove("Sword"); // Remove one sword from the inventory
            anim.SetBool("IsArmed", true); // Set the animation parameter to armed
        }
    }

    void UnequipSword()
    {
        swordEquipped = false; // Set sword as unequipped
        anim.SetBool("IsArmed", false); // Set the animation parameter to unarmed
    }

    void UpdateWeaponAnimation()
    {
        if (swordEquipped)
        {
            anim.SetBool("IsArmed", true); // If sword is equipped, show armed animations
        }
        else
        {
            anim.SetBool("IsArmed", false); // If sword is unequipped, show unarmed animations
        }
    }
}