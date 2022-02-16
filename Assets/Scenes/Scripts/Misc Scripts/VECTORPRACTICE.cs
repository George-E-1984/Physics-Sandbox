using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VECTORPRACTICE : MonoBehaviour
{

    public Vector3 pos;
    public Vector2 PlayerMouseInput;

    public float playerSpeed;
    private Rigidbody Rb;
    
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

     




        PlayerMovement();


   
    }

    void PlayerMovement()
    {
        Vector3 MoveVector = transform.TransformDirection(pos) * playerSpeed;
        Rb.velocity = new Vector3(MoveVector.x, Rb.velocity.y, MoveVector.z);


    }

    void MouseMovement()
    {
      




    }
}
