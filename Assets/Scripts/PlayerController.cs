using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public MessageDisplay messageDisplay;  // Reference to the MessageDisplay script

    public float speed = 4.0f;
    public float sprintMultiplier = 1.5f;
    private float currentSpeed;

    private Vector2 moveInput;
    private float xSpeed;
    private float ySpeed;

    private bool swordEquipped = false; // Keep track of sword equipped status
    public Transform inventoryUI; // Inventory UI (assign in Inspector)

    private Vector2 lastDirection; // Keep track of the last movement direction

    // Declare 'stone' variable for interacting with it
    public GameObject stone;  // This should be assigned in the Inspector or dynamically at runtime

    // Add potion tracking
    private bool potionConsumed = false; // Track if the potion has been consumed

    // Reference to the Message UI (assigned in the inspector)
    public TextMeshProUGUI messageUI;  // This should be assigned in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = speed;

        // Dynamically find the stone object by name (or use tag)
        if (stone == null)
        {
            stone = GameObject.Find("BigRock");  // Replace "BigRock" with your actual object name in the scene
            if (stone == null)
            {
                Debug.LogError("Stone object not found in the scene!");
            }
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
            SetIdleAnimation();
        }

        HandleInventoryInput();
        HandleQuestInteractions(); // Restored the quest interaction method
        HandleEquipInput();
        UpdateWeaponAnimation();

        // Allow explosion animation only after drinking the potion
        if (Input.GetKeyDown(KeyCode.B) && potionConsumed)
        {
            if (stone != null)
            {
                // Immediately trigger the explosion animation
                Animator stoneAnim = stone.GetComponent<Animator>();
                if (stoneAnim != null)
                {
                    stoneAnim.SetTrigger("Explode"); // Trigger explosion animation on the rock itself
                }

                // Hide the original rock and show the explosion effect immediately
                if (stone.transform.childCount >= 2)
                {
                    stone.transform.GetChild(0).gameObject.SetActive(false);  // Hide rock
                    stone.transform.GetChild(1).gameObject.SetActive(true);   // Show explosion effect
                }
                else
                {
                    Debug.LogWarning("Stone object doesn't have enough children!");
                }

                // Start the coroutine to disable the collider and renderer after a delay
                StartCoroutine(DisableColliderAndRendererAfterDelay(0.3f));
            }
            else
            {
                Debug.LogError("Stone object is not assigned or not found!");
            }
        }

        // Check for potion consumption key (P) and call DrinkPotion()
        if (Input.GetKeyDown(KeyCode.P)) // 'P' to drink potion
        {
            DrinkPotion(); // Call method to consume the potion
        }
    }

    // Coroutine to disable the collider and renderer after a delay
    private IEnumerator DisableColliderAndRendererAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the collider and renderer of the stone
        Collider2D stoneCollider = stone.GetComponent<Collider2D>();
        Renderer stoneRenderer = stone.GetComponent<Renderer>();

        if (stoneCollider != null) stoneCollider.enabled = false; // Disable collision
        if (stoneRenderer != null) stoneRenderer.enabled = false; // Disable renderer
    }

    void SetIdleAnimation()
    {
        anim.SetBool("IsIdleUp", false);
        anim.SetBool("IsIdleDown", false);
        anim.SetBool("IsIdleLeft", false);
        anim.SetBool("IsIdleRight", false);

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
        // This is where you'd implement any quest-related logic
    }

    void HandleEquipInput()
    {
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
        UpdateWeaponAnimation();
    }

    void UnequipSword()
    {
        swordEquipped = false; // Set sword as unequipped
        anim.SetBool("IsArmed", false); // Immediately set the animation parameter to unarmed

        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.Add("Sword", 1); // Add one sword back to the inventory
        }
        UpdateWeaponAnimation();
    }

    void UpdateWeaponAnimation()
    {
        anim.SetBool("IsArmed", swordEquipped);
    }

    // New method to consume the potion
    public void DrinkPotion()
    {
        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null && inventory.HasItem("Magic Potion"))
        {
            inventory.Remove("Magic Potion", 1);  // Remove one potion from inventory
            potionConsumed = true;  // Set potionConsumed to true after drinking the potion
            Debug.Log("Potion consumed. Explosion ready!");

            // Display message in MessageUI
            if (messageDisplay != null)
            {
                messageDisplay.ShowMessage("Potion consumed +1 Explosion!", 3f, null); // Display the message on screen
            }
        }
        else
        {
            Debug.Log("No Magic Potion in inventory!");  // If no potion, log the message
        }
    }
}
