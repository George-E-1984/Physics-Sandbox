using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.InputSystem; 

public class Button : Interactable
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
    [Header("Sound Effects")]
    public AudioSource buttonAudioSource;
    public AudioClip[] onClickSound;

    [Header("Unity Events")]
    public UnityEvent onButtonPressed; 
    public UnityEvent onButtonPressedOnce; 
    public UnityEvent onButtonUp; 
    public UnityEvent onButtonHold; 

    [Header("Info")]
    public SoftJointLimit configLimit; 
    private bool isButtonDown = false;   
    private PlayerInteract playerGrabScript; 
    private PlayerManager playerManagerScript;
    private bool isPushingButton; 
    private RaycastHit hit; 
    private int timesCalled = 0;
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
        playerGrabScript = SceneMaster.instance.player.GetComponent<PlayerInteract>(); 
        playerManagerScript = SceneMaster.instance.player.GetComponent<PlayerManager>(); 
    }
    void Update()
    {
        HandleButtonState(); 
    }
    public override void Interact()
    {
        ButtonPress(); 
    }
    public override void StopInteract()
    {
        ButtonDepress(); 
    }
    public void ButtonPress()
    { 
        buttonJoint.targetPosition = new Vector3(0, downThreshold, 0);
        isPushingButton = true;   
    }
    public void ButtonDepress()
    { 
        if (isPushingButton)
        {
            buttonJoint.targetPosition = new Vector3(0, 0 ,0); 
            isPushingButton = false; 
        }
    }
    public void HandleButtonState()
    { 
        if (buttonPlunger.transform.localPosition.y <= downThreshold && !isButtonDown)
        {
            buttonAudioSource.pitch = Random.Range(0.9f, 1.1f);
            buttonAudioSource.PlayOneShot(onClickSound[Random.Range(0, onClickSound.Length)]); 
            onButtonPressed.Invoke();  
            print("Button Pressed"); 
            if (timesCalled < 1)
            {
                onButtonPressedOnce.Invoke(); 
                print("Button one press loam"); 
            }
            timesCalled++; 
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
    }
}
