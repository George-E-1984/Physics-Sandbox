using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro; 

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(); 
    public abstract void StopInteract(); 
    public TextMeshPro interactableText; 
}
