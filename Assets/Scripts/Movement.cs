using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 PlayerMovement;
    private Vector2 PlayerMouse;
    private float xRot;

    [SerializeField] private Transform PlayerCam;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask FloorMask;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speed;
    [SerializeField] private float Sens;
    [SerializeField] private  float Jump;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update() {
        PlayerMovement = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MoveCharacter();
        MoveCam();
    }
    void MoveCharacter()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovement) * speed ;
        rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(GroundCheck.position, 0.1f, FloorMask))
            {
                rb.AddForce(Vector3.up * Jump, ForceMode.Impulse);
            }
        }
    }

    void MoveCam()
    {
        xRot -= PlayerMouse.y * Sens;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.Rotate(0f, PlayerMouse.x * Sens, 0f);
        PlayerCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}