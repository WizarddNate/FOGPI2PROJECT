using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Speed and Acceleration")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float decceleration = 10f;
    private float currentSpeed;
    private Vector3 velocity;

    private float gravity = -9.18f;
    bool isGrounded;

    private InputSystem_Actions playerInputActions;
    private Vector3 input;
    public CharacterController characterController;
    public InputAction Click;

    /* Put the next 4 functions in a Game Manager script later*/

    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if (!isGrounded)
        {
            velocity.y = gravity * Time.deltaTime;
        }

        MoveInput();

        Look();
        CalculateSpeed();
        Movement();
    }
    private void FixedUpdate()
    {

    }

    private void MoveInput()
    {
        Vector2 Input = playerInputActions.Player.Move.ReadValue<Vector2>();

        input = new Vector3(Input.x, 0, Input.y);

    }

    private void CalculateSpeed()
    {
        if (input == Vector3.zero && currentSpeed > 0)
        {
            //slowing down
            currentSpeed -= decceleration * Time.deltaTime;
        }
        else if (input != Vector3.zero && currentSpeed < maxSpeed)
        {
            //speeding up
            currentSpeed += acceleration * Time.deltaTime;
        }

        //prevent speed from going past the max speed
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    private void Look()
    {
        if (input == Vector3.zero) return;

        Matrix4x4 isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 multipliedMatrix = isometricMatrix.MultiplyPoint3x4(input);

        Quaternion rotation = Quaternion.LookRotation(multipliedMatrix, Vector3.up);

        //This makes the player rotate in a half circle which i think is kinda ugly. Change to snap rotation?
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void Movement()
    {
        Vector3 moveDirection = transform.forward * currentSpeed * Time.deltaTime + velocity;
        characterController.Move(moveDirection);
    }

}