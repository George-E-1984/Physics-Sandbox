using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    [Header("Assignables")] 
    public PlayerData playerData; 
    public CapsuleCollider playerCol;
    public PlayerManager playerManager; 
    public UiData uiData;
    public AudioSource playerAudioSource;
    public GameObject shootOrigin;
    public Transform playerCamera;
    public GameObject groundCheckLocation;
    public LayerMask groundMask;
    public Rigidbody rb;

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
    float multiplier = 0.01f;
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
    public bool isPlayingSound = false; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        Debug.DrawRay(transform.position, Vector3.left, Color.red);
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {             
        if (rb.velocity.magnitude > 2f && isGrounded && !isPlayingSound)
        {
            StartCoroutine(PlayerSound()); 
        } 
        //MyInput();
        ControlDrag();

        if (isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }
        else UnCrouch();
    
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, playerCol.height / 2 + 0.1f); 
        
    }
    public IEnumerator PlayerSound()
    {
        isPlayingSound = true;
        playerAudioSource.pitch = Random.Range(0.9f, 1.1f);
        playerAudioSource.volume = Random.Range(0.9f, 1.1f); 
        playerManager.playerAudioSource.PlayOneShot(playerManager.moveSounds[Random.Range(0, playerManager.moveSounds.Length)]);
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.25f); 
        }
        else 
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingSound = false; 
    }
    public void SetSprintTrue(InputAction.CallbackContext context)
    {
        isSprinting = true; 
    }
    public void SetSprintFalse(InputAction.CallbackContext context)
    {
        isSprinting = false; 
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
        //PlayerJump();
    }

    public void MovePlayer()
    {
        Vector2 inputVector = playerManager.playerInputActions.Player.Movement.ReadValue<Vector2>(); 
        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x; 
        //moveDirection = hit.normal * inputVector.y + transform.right * inputVector.x;
        if (isGrounded)
        {
            if (isSprinting)
            {
                rb.AddForce(moveDirection * playerSpeed * sprintMultiplier, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(moveDirection * playerSpeed, ForceMode.Acceleration);
            }
        }
        else
        {
            rb.AddForce(moveDirection * airMovementSpeed, ForceMode.Acceleration);
        }

        
    
    }

    private void PlayerMoveCamera()
    {
        //mouseX = Input.GetAxisRaw("Mouse X");
        //mouseY = Input.GetAxisRaw("Mouse Y");
        mouseX = playerManager.playerInputActions.Player.CameraMovement.ReadValue<Vector2>().x; 
        mouseY = playerManager.playerInputActions.Player.CameraMovement.ReadValue<Vector2>().y;
        yRotation += mouseX * playerData.sensitivity * multiplier * System.Convert.ToInt16(canLook);
        xRotation -= mouseY * playerData.sensitivity * multiplier * System.Convert.ToInt16(canLook);

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        rb.MoveRotation(Quaternion.Euler(0, yRotation, 0)); 
        //transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void PlayerJumpUnpressed(InputAction.CallbackContext context)
    {
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 2, rb.velocity.z); 
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
    private void OnDrawGizmos() 
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.cyan);
    }

}
