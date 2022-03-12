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
    void Start()
    {
        for (int i = 0; i < baseColliders.Length; i++)
        {
            if (i <= plungerColliders.Length)
            {
                Physics.IgnoreCollision(baseColliders[i], plungerColliders[i], true); 
            }
        }
    }
    void Update()
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
    }
}
