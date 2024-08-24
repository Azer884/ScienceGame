using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    private PlayerInput inputActions;
    private CharacterController controller;

    [SerializeField] private Camera cam;
    [SerializeField] public float lookSensitivity = 1.0f;
    private float xRotation = 0f;

    // Movement Vars
    private Vector3 velocity;
    public float gravity = -9.81f;
    private bool grounded;
    private float speedMultiplier = 1.0f;

    // Crouch Vars
    private float initHeight;
    private Vector3 initCenter;
    [SerializeField] private float crouchHeight;
    [SerializeField] private Vector3 crouchCenter;
    [SerializeField] private Transform standingCameraPosition;
    [SerializeField] private Transform crouchingCameraPosition;
    public PlayerStats[] stats;
    public int index;
    public PotionLists potions;
    private Dictionary<string, int> potionIndexMap = new Dictionary<string, int>
    {
        {"Attack Speed Potion", 1},
        {"Defense Potion", 2},
        {"Health Potion", 3},
        {"Jump Boost Potion", 4},
        {"Scale Down Potion", 5},
        {"Scale Up Potion", 6},
        {"Speed Potion", 7},
        {"Strength Potion", 8},
        {"Scale Enemy Down Potion", 9},
        {"Scale Enemy Up Potion", 10},
        {"Freezing Potion", 11},
        {"Enemy Untouchable Potion", 12},
        {"Poison Potion", 13},
        {"Help Potion", 14},
        {"Second Life Potion", 15},
        {"Untouchable Potion", 16}
    };

    // Swimming
    public bool isSwimming;
    [SerializeField] private float swimSpeed = 3f;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        initHeight = controller.height;
        initCenter = controller.center;
        
        
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Update()
    {
        if (GetComponent<TeleportationController>().isInArena && inputActions.PlayerControls.Use.triggered)
        {
            ChangePlayerStats(potions.stringLists.Last());
            if (new[] { 11, 12, 13, 14, 15, 16 }.Contains(index))
            {
                index = 0;
            }
            Debug.Log("workinggg");
            transform.localScale *= stats[index].scale;
        }
        DoMovement();
        DoLooking();
        DoCrouch();

    }

    private void ChangePlayerStats(string potionName)
    {
        if (potionIndexMap.TryGetValue(potionName, out int newIndex))
        {
            index = newIndex;
        }
    }

    private void DoLooking()
    {
        Vector2 looking = GetPlayerLook();
        float lookX = looking.x * lookSensitivity * Time.deltaTime;
        float lookY = looking.y * lookSensitivity * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookX);
    }

    private void DoMovement()
    {
        if (!isSwimming)
        {
            controller.height = initHeight;
            controller.center = initCenter;
            grounded = controller.isGrounded;
            if (grounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            Vector2 movement = GetPlayerMovement();
            speedMultiplier = inputActions.PlayerControls.Run.ReadValue<float>() > 0 && movement.y > 0 ? 2.0f : 1.0f;

            Vector3 move = transform.right * movement.x + transform.forward * movement.y;
            controller.Move(move * stats[index].speed * speedMultiplier * Time.deltaTime);

            // Jumping
            if (grounded && inputActions.PlayerControls.Jump.triggered)
            {
                velocity.y = Mathf.Sqrt(stats[index].jump * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            controller.height = crouchHeight;
            HandleSwimming();
        }
    }

    private void HandleSwimming()
    {
        Vector3 swimMovement = Vector3.zero;

        Vector2 movement = GetPlayerMovement();
        swimMovement += cam.transform.right * movement.x + cam.transform.forward * movement.y;

        if (inputActions.PlayerControls.Jump.ReadValue<float>() > 0)
        {
            swimMovement += Vector3.up;
        }

        if (inputActions.PlayerControls.Crouch.ReadValue<float>() > 0)
        {
            swimMovement -= Vector3.up;
        }

        controller.Move(swimMovement * swimSpeed * Time.deltaTime);
    }

    private void DoCrouch()
    {
        if (!isSwimming)
        {
            if (inputActions.PlayerControls.Crouch.ReadValue<float>() > 0)
            {
                controller.height = crouchHeight;
                controller.center = crouchCenter;
                cam.transform.position = Vector3.Lerp(cam.transform.position, crouchingCameraPosition.position, Time.deltaTime * 10f);
            }
            else
            {
                if (!Physics.Raycast(transform.position, Vector3.up, 2.0f))
                {
                    controller.height = initHeight;
                    controller.center = initCenter;
                    cam.transform.position = standingCameraPosition.position;
                }
            }
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return inputActions.PlayerControls.Move.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerLook()
    {
        return inputActions.PlayerControls.Look.ReadValue<Vector2>();
    }
}
