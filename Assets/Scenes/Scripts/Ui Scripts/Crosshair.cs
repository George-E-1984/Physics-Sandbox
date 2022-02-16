using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour

{
    private Animator animator; 
    public PlayerGrab playerGrab;


    // Start is called before the first frame update
    void Start()
    {
       animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGrab.hitGrab.collider != null)
        {
            animator.SetBool("Highlight Grab Transition", true);
            animator.SetBool("Highlight Transition to Idle", false);
            animator.SetBool("Idle Transition to Highlight", false);
        }
        if (playerGrab.isGrabbing == true)
        {
            animator.SetBool("Highlight Transition to Idle", false);
            animator.SetBool("Highlight Grab Transition", false);
            animator.SetBool("Idle Transition to Highlight", true);
        }
        else if (playerGrab.isGrabbingTool == true)
        {
            animator.SetBool("Highlight Transition to Idle", false);
            animator.SetBool("Highlight Grab Transition", false);
            animator.SetBool("Idle Transition to Highlight", true);
        }
        else if(playerGrab.isGrabbing == false && playerGrab.hitGrab.collider == null)
        {
            animator.SetBool("Highlight Grab Transition", false);
            animator.SetBool("Idle Transition to Highlight", false);
            animator.SetBool("Highlight Transition to Idle", true);
        }
        else if(playerGrab.isGrabbingTool == false && playerGrab.hitGrab.collider == null)
        {
            animator.SetBool("Highlight Grab Transition", false);
            animator.SetBool("Idle Transition to Highlight", false);
            animator.SetBool("Highlight Transition to Idle", true);
        }
       
        
    
    }
}
