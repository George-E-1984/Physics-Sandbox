using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Assignables")] 
    public CapsuleCollider playerCol;
    public PlayerGrab playerGrab;
    public UiData uiData;
    public AudioSource playerAudioSource;
    public GameObject shootOrigin;

    [Header("Variables")]
    [Tooltip("How fast the player moves. Wow!")]
    public float playerSpeed;
    public float jumpForce;
    public float sprintMultiplier;
    public float maxSpeed;
    public float maxSprintSpeed;
    public float groundDrag;
    public float airDrag;
    public Transform PlayerCamera;
    public LayerMask GroundMask;
    public GameObject GroundCheckLocation;
    public bool IsGrounded;
    public bool isSprinting;
    public bool isJumping; 
    public float Sensitivity;
    private float xRot;
    public float crouchSpeed;
    [Tooltip("Speed of player in air, Higher = less speed, Lower = more speed!")] 
    public float airMovementSpeed;
    

    Vector3 PlayerMoveInput;
    Vector2 PlayerMouseInput;

    public bool isCrouching;
    private float standingHeight = 2f;
    private float crouchingHeight = 1f;
    RaycastHit hit; 
    public bool allowedMovement;

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
        if (IsGrounded && Input.GetKey(KeyCode.LeftControl))
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

        PlayerMoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
   
        PlayerJump();

        //Ground Check
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);
     
    }

    private void LateUpdate()
    {
        PlayerMoveCamera();
    }

    private void FixedUpdate()
    {
        PlayerMovementForce();
        ControlDrag();
    }

    private void PlayerMovementForce()
    {
        
        Vector3 MovementVectors = transform.TransformDirection(PlayerMoveInput) * playerSpeed;
      
        rb.AddForce(System.Convert.ToInt32(allowedMovement) * MovementVectors * (playerSpeed + (System.Convert.ToInt32(isSprinting) * sprintMultiplier)));

        if (isSprinting == false && IsGrounded)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else if (isSprinting && IsGrounded)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSprintSpeed);
        }
        else if (IsGrounded == false)
        {
            var clampXZ = new Vector2(rb.velocity.x, rb.velocity.z);
            clampXZ = Vector2.ClampMagnitude(clampXZ, (maxSpeed + (System.Convert.ToInt32(isSprinting) * sprintMultiplier))) / airMovementSpeed;
            rb.velocity = new Vector3(clampXZ.x, rb.velocity.y, clampXZ.y);
        }
       
      
  
    }

    private void PlayerMoveCamera()
    {
        //x rotation because X is the up rotation, whilst Y is the sideways rotation 
        xRot -= PlayerMouseInput.y * Sensitivity;
        xRot = Mathf.Clamp(xRot, -90, 90);
        //Records the input for mouse movement (Horizontal = left or right) and (Vertical = Up or down) its vector 2 because its only X and Y axis. 
        PlayerMouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0, 0);             
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded) 
        {
            rb.AddForce((Vector3.up * jumpForce), ForceMode.Impulse); 
            if (hit.collider.GetComponent<Rigidbody>() == true)
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.down * (jumpForce / 2), ForceMode.Impulse);              
            }
        }
    }
    
    private void Crouch()
    {
        playerCol.height = Mathf.Lerp(playerCol.height, crouchingHeight, Time.deltaTime * crouchSpeed);        
    }

    private void UnCrouch()
    {
        playerCol.height = Mathf.Lerp(playerCol.height, standingHeight, Time.deltaTime * crouchSpeed);
    }

    private void ControlDrag()
    {
        if (IsGrounded) rb.drag = groundDrag;
        else rb.drag = airDrag;
    }

 
}
