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

    private float lastMovementTime;
    private float movementThreshold = 0.2f;
    private bool wasMovingUp;

    public GameObject goldenKeyPrefab; // Reference to the golden key prefab
    private bool swordEquipped = false; // Keep track of sword equipped status

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
        HandleEquipInput();
        UpdateWeaponAnimation();
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
            Inventory playerInventory = GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.Add("Golden Key", 1);
                MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
                disp.ShowMessage("You picked up a Golden Key!", 2.0f);
            }
            Destroy(goldenKeyPrefab); // Destroy the key object in the world
            goldenKeyPrefab = null; // Set the reference to null to avoid further issues
        }
    }

    void HandleEquipInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Equip button "1"
        {
            if (!swordEquipped) // If we don't have the sword equipped
            {
                Inventory inventory = GetComponent<Inventory>();
                if (inventory != null && inventory.GetCount("Sword") > 0)
                {
                    EquipSword();
                }
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
            swordEquipped = true; // Set the sword as equipped
            inventory.Remove("Sword"); // Remove one sword from the inventory
            anim.SetBool("IsArmed", true); // Set the animation parameter to armed
        }
    }

    void UnequipSword()
    {
        swordEquipped = false; // Set sword as unequipped
        anim.SetBool("IsArmed", false); // Set the animation parameter to unarmed
    }

    // Method to update the weapon animation based on whether the sword is equipped
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
