using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // movement
    float moveSpeed = 6f;
    float crouchSpeed = 2f;
    float inAirSpeed = 3f;
    
    float jumpForce = 7.5f;
    public float movementMultiplier = 10f;
    public float airMultiplier = 5f;
    public float crouchingMultiplier = 5f;

    //sprinting
    float sprintingSpeed = 12f;
    float walkSpeed = 6f;



    float horizontalMovement;
    float verticalMovement;

    //jumping???!?!?!
    float playerHeight = 2f;
    bool isGrounded;

    //Drag
    float groundDrag = 6f;
    float airDrag = 1f;
    float crouchDrag = 9f;

    //crouch
    Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    public Transform playerScale;
    bool isCrouching;


    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    RaycastHit slopeHit;
    Rigidbody rb;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
       // playerScale =  transform.localScale;
    }

    void Update()
    {

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);
        MyInput();
        ControlDrag();
        ControlSpeed();

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            Uncrouch();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }
    

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void Crouch()
    {
        playerScale.localScale = new Vector3(1, 0.5f, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        isCrouching = true;
        
    }

    void Uncrouch()
    {
        playerScale.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        isCrouching = false;
    }

    void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

        if(isGrounded && isCrouching)
        {
            rb.drag = crouchDrag;
        } 
    }

    void ControlSpeed()
    {
        if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintingSpeed, Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, Time.deltaTime);
        }
    }

  
    void FixedUpdate()
    {
         rb.AddForce(Vector3.down * Time.deltaTime * 10);

        MovePlayer();
    }

    void MovePlayer()
    {  

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }  
        else
        {
            rb.AddForce(moveDirection.normalized * inAirSpeed * airMultiplier, ForceMode.Acceleration);
        }  

        if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
    }
}