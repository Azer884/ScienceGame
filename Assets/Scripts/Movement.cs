using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private PlayerStats stats; 
    private Vector3 PlayerMovement;
    private Vector2 PlayerMouse;
    private float xRot;
    private bool isCrouching = false;

    [SerializeField] private Transform PlayerCam;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask FloorMask;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float Sens;
    [HideInInspector] public bool isSwimming = false;

    // Swimming
    [SerializeField] private float swimSpeed = 3f;
    [SerializeField] private Vector2 swimmingColliderSize = new Vector2(1f, 0.5f);
    private Vector2 originalColliderSize;
    private CapsuleCollider playerCollider;

    // Sprinting
    [SerializeField] private float sprintSpeed = 7f;
    private float originalSpeed;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCollider = GetComponentInChildren<CapsuleCollider>();
        originalColliderSize = new Vector2(playerCollider.radius, playerCollider.height);
        originalSpeed = stats.speed;
    }

    void Update()
    {
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        MoveCam();

        if (!isSwimming)
        {
            PlayerMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.useGravity = true;
            playerCollider.height = originalColliderSize.y;
            playerCollider.radius = originalColliderSize.x;

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
            {
                Crouch();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
            {
                StandUp();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Sprint();
            }
            else
            {
                StopSprinting();
            }

            MoveCharacter();
        }
        else
        {
            HandleSwimming();
        }
    }

    void HandleSwimming()
    {
        if (rb.useGravity)
        {
            rb.useGravity = false;
        }

        playerCollider.height = swimmingColliderSize.y;
        playerCollider.radius = swimmingColliderSize.x;

        Vector3 swimMovement = Vector3.zero;
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            swimMovement += PlayerCam.forward;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            swimMovement -= PlayerCam.forward;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            swimMovement += PlayerCam.right;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            swimMovement -= PlayerCam.right;
        }

        // Move up when space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            swimMovement += Vector3.up;
        }

        // Move down when left shift is pressed (optional)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            swimMovement -= Vector3.up;
        }

        transform.position += swimMovement.normalized * swimSpeed * Time.deltaTime;
    }

    void MoveCharacter()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovement) * stats.speed;
        rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);
    }

    void MoveCam()
    {
        xRot -= PlayerMouse.y * Sens;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.Rotate(0f, PlayerMouse.x * Sens, 0f);
        PlayerCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    void Jump()
    {
        if (Physics.CheckSphere(GroundCheck.position, 0.1f, FloorMask))
        {
            rb.AddForce(Vector3.up * stats.jump, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        playerCollider.height = crouchHeight;
        isCrouching = true;
    }

    void StandUp()
    {
        playerCollider.height = originalColliderSize.y;
        isCrouching = false;
    }

    void Sprint()
    {
        stats.speed = sprintSpeed;
    }

    void StopSprinting()
    {
        stats.speed = originalSpeed;
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
