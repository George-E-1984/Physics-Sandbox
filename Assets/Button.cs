using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class Button : MonoBehaviour
{
    [Header("Assign")]
    public GameObject buttonBase; 
    public Collider[] baseColliders; 
    public GameObject buttonPlunger; 
    public Collider[] plungerColliders; 
    public ConfigurableJoint buttonJoint;

    [Header("Variables")]
    public float downThreshold; 
    public float upThreshold; 

    [Header("Unity Events")]
    public UnityEvent onButtonPressed; 
    public UnityEvent onButtonUp; 
    public UnityEvent onButtonHold; 

    [Header("Info")]
    public SoftJointLimit configLimit; 
    private bool isButtonDown = false;   
    private PlayerGrab playerGrabScript; 
    private PlayerManager playerManagerScript;
    private bool isPushingButton; 
    private RaycastHit hit; 
    void Start()
    {
        int baseCol = 0; 
        int plungerCol = 0; 
        foreach (Collider baseCollider in baseColliders)
        {
            foreach (Collider plungerCollider in plungerColliders)
            {
               Physics.IgnoreCollision(baseColliders[baseCol], plungerColliders[plungerCol]);
               plungerCol++; 
            }
            baseCol++; 
        }
        playerGrabScript = SceneMaster.instance.player.GetComponent<PlayerGrab>(); 
        playerManagerScript = SceneMaster.instance.player.GetComponent<PlayerManager>(); 
    }
    async void Update()
    {
        // if (Input.GetMouseButtonDown(1))
        // {
        //     Physics.Raycast(playerManagerScript.playerCam.transform.position, playerManagerScript.playerCam.transform.forward, out hit, 10f);
        //     {
        //         if (hit.collider)
        //         {
        //             if (hit.collider.tag == "Button")
        //             {
        //                 print("Pushing button"); 
        //                 buttonJoint.targetPosition = new Vector3(0, buttonJoint.linearLimit.limit, 0);
        //                 isPushingButton = true; 
        //             }
        //         }
        //     }
        // }
        // else if (Input.GetMouseButtonUp(1) && isPushingButton)
        // {
        //     buttonJoint.targetPosition = new Vector3(0, 0, 0);
        //     isPushingButton = false; 
        // }
        HandleButtonState(); 
    }

    void HandleButtonState()
    {
        if (buttonPlunger.transform.localPosition.y <= downThreshold && !isButtonDown)
        {
            onButtonPressed.Invoke();  
            print("Button Pressed"); 
            isButtonDown = true; 
        }
        else if (buttonPlunger.transform.localPosition.y <= downThreshold && isButtonDown)
        {
            onButtonHold.Invoke(); 
            print("Button Held");
        }   
        else if (buttonPlunger.transform.localPosition.y >= upThreshold && isButtonDown)
        {
            isButtonDown = false; 
            onButtonUp.Invoke();
            print("Button up");
        }
        
        void ButtonPush()
        {
            Physics.Raycast(playerManagerScript.playerCam.transform.position, playerManagerScript.playerCam.transform.forward, out hit, 10f);
            {
                if (hit.collider)
                {
                    if (hit.collider.tag == "Button")
                    {
                        print("Pushing button"); 
                        buttonJoint.targetPosition = new Vector3(0, buttonJoint.linearLimit.limit, 0);
                        isPushingButton = true; 
                    }
                }
            }
        }

    }
}
