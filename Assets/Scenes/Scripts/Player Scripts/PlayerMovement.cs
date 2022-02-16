using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Assignables")] 
    public PlayerData playerData; 
    public CapsuleCollider playerCol;
    public PlayerGrab playerGrab;
    public UiData uiData;
    public AudioSource playerAudioSource;
    public GameObject shootOrigin;
    public Transform playerCamera;
    public GameObject groundCheckLocation;
    public LayerMask groundMask;


    [Header("Movement")]
    Vector3 moveDirection;
    float verticalMovement;
    float horizontalMovement;
    [Tooltip("How fast the player moves. Wow!")]
    public float playerSpeed;
    [Tooltip("Speed of player in air, Higher = less speed, Lower = more speed!")] 
    public float airMovementSpeed;
    public float maxSpeed;
    public float sprintMultiplier;
    public float maxSprintSpeed;
    public bool isSprinting;
    public bool allowedMovement;

    [Header("Jumping")]
    public bool canJump; 
    public bool isJumping; 
    public bool isGrounded;
    public float jumpForce;

    [Header("Drag")]
    public float groundDrag;
    public float airDrag;

    [Header("Crouching")]
    public float crouchSpeed;
    public bool isCrouching;
    private float standingHeight = 2f;
    private float crouchingHeight = 1f;

    [Header("Cam movement")]
    float mouseX;
    float mouseY;
    float multiplier = 0.1f;
    float xRotation;
    float yRotation;
    [Range(0, 100)]
    public float sensX;
    [Range(0, 100)]
    public float sensY;

    [Header("Info (Do not touch these values)")]
    private float xRot;
    public RaycastHit hit; 
    public bool canLook = true; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        Debug.DrawRay(transform.position, Vector3.left, Color.red);

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {             
        MyInput();
        ControlDrag();

        if (isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }
        else UnCrouch();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        //Ground Check
        Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);
        
        if (hit.collider && hit.collider != playerGrab.grabbedObject)
        {
            isGrounded = true; 
        }
        else 
        {
            isGrounded = false; 
        } 
        
    }

    public void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            canJump = true;
        }
    }

    private void LateUpdate()
    {
        PlayerMoveCamera();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        PlayerJump();
    }

    private void MovePlayer()
    {
        if (isGrounded)
        {
            if (isSprinting)
            {
                rb.AddForce(moveDirection.normalized * playerSpeed * sprintMultiplier, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Acceleration);
            }
        }
        else
        {
            rb.AddForce(moveDirection.normalized * airMovementSpeed, ForceMode.Acceleration);
        }
    }

    private void PlayerMoveCamera()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * playerData.sensitivity * multiplier * System.Convert.ToInt16(canLook);
        xRotation -= mouseY * playerData.sensitivity * multiplier * System.Convert.ToInt16(canLook);

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void PlayerJump()
    {
        if (canJump) 
        {
            canJump = false; 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 

            //WTF is this george?
            if (hit.collider)
            {
                if (hit.collider.GetComponent<Rigidbody>() == true)
                {
                    hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.down * (jumpForce / 2), ForceMode.Impulse);
                }
                             
            }
        }
    }
    
    private void Crouch()
    {
        playerCol.height = Mathf.Lerp(playerCol.height, crouchingHeight, crouchSpeed);        
    }

    private void UnCrouch()
    {
        playerCol.height = Mathf.Lerp(playerCol.height, standingHeight, crouchSpeed);
    }

    private void ControlDrag()
    {
        if (isGrounded) rb.drag = groundDrag;
        else rb.drag = airDrag;
    }

}
