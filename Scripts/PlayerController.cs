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

    private bool swordEquipped = false; // Track if sword is equipped
    public Transform inventoryUI; // Inventory UI (assign in Inspector)

    private Vector2 lastDirection; // Store last movement direction

    public GameObject stone;  // Stone object for interaction

    private bool potionConsumed = false; // Track potion consumption

    public TextMeshProUGUI messageUI;  // Message UI (assign in Inspector)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = speed;

        // Dynamically find stone object if not set
        if (stone == null)
        {
            stone = GameObject.Find("BigRock");
            if (stone == null)
            {
                Debug.LogError("Stone object not found!");
            }
        }
    }

    void FixedUpdate()
    {
        // Handle movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical).normalized;
        rb.velocity = moveInput * currentSpeed;

        // Update speed for movement animation
        xSpeed = moveInput.x;
        ySpeed = moveInput.y;

        // Store last movement direction
        if (moveInput.magnitude > 0)
        {
            lastDirection = moveInput;
        }
    }

    void Update()
    {
        // Sprinting logic
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? speed * sprintMultiplier : speed;
        anim.SetBool("IsSprinting", isSprinting);

        // Update movement animation
        bool isMoving = moveInput.magnitude > 0;
        anim.SetFloat("xspeed", xSpeed);
        anim.SetFloat("yspeed", ySpeed);
        anim.SetBool("IsMoving", isMoving);

        if (!isMoving) SetIdleAnimation();

        HandleInventoryInput();
        HandleEquipInput();
        UpdateWeaponAnimation();

        // Explosion after potion is consumed
        if (Input.GetKeyDown(KeyCode.B) && potionConsumed)
        {
            TriggerExplosion();
        }

        // Drink potion with key P
        if (Input.GetKeyDown(KeyCode.P)) DrinkPotion();
    }

    // Set idle animation based on last movement direction
    void SetIdleAnimation()
    {
        anim.SetBool("IsIdleUp", false);
        anim.SetBool("IsIdleDown", false);
        anim.SetBool("IsIdleLeft", false);
        anim.SetBool("IsIdleRight", false);

        if (lastDirection.y > 0) anim.SetBool("IsIdleUp", true);
        else if (lastDirection.y < 0) anim.SetBool("IsIdleDown", true);
        else if (lastDirection.x < 0) anim.SetBool("IsIdleLeft", true);
        else if (lastDirection.x > 0) anim.SetBool("IsIdleRight", true);
    }

    // Handle inventory toggle with 'I' key
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

    // Handle equipment toggle for the sword with key 1
    void HandleEquipInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!swordEquipped) EquipSword();
            else UnequipSword();
        }
    }

    // Equip sword if available in inventory
    void EquipSword()
    {
        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null && inventory.GetCount("Sword") > 0)
        {
            swordEquipped = true;
            inventory.Remove("Sword");
            anim.SetBool("IsArmed", true);
        }
        UpdateWeaponAnimation();
    }

    // Unequip sword
    void UnequipSword()
    {
        swordEquipped = false;
        anim.SetBool("IsArmed", false);

        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.Add("Sword", 1);
        }
        UpdateWeaponAnimation();
    }

    // Update weapon animation based on sword equipped
    void UpdateWeaponAnimation()
    {
        anim.SetBool("IsArmed", swordEquipped);
    }

    // Trigger the explosion animation on the stone
    void TriggerExplosion()
    {
        if (stone != null)
        {
            Animator stoneAnim = stone.GetComponent<Animator>();
            if (stoneAnim != null) stoneAnim.SetTrigger("Explode");

            if (stone.transform.childCount >= 2)
            {
                stone.transform.GetChild(0).gameObject.SetActive(false);
                stone.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Stone doesn't have enough children!");
            }

            StartCoroutine(DisableColliderAndRendererAfterDelay(0.3f));
        }
        else
        {
            Debug.LogError("Stone object not found!");
        }
    }

    // Coroutine to disable collider and renderer of stone after delay
    private IEnumerator DisableColliderAndRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D stoneCollider = stone.GetComponent<Collider2D>();
        Renderer stoneRenderer = stone.GetComponent<Renderer>();

        if (stoneCollider != null) stoneCollider.enabled = false;
        if (stoneRenderer != null) stoneRenderer.enabled = false;
    }

    // Drink the potion and enable explosion
    public void DrinkPotion()
    {
        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null && inventory.HasItem("Magic Potion"))
        {
            inventory.Remove("Magic Potion", 1);
            potionConsumed = true;

            if (messageDisplay != null)
            {
                messageDisplay.ShowMessage("Potion consumed +1 Explosion!", 3f, null);
            }
        }
        else
        {
            Debug.Log("No Magic Potion in inventory!");
        }
    }
}
