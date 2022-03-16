using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 
using UnityEngine.Audio; 
using UnityEngine.Events; 
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Assign")]
    public PlayerData playerData; 
    public PlayerMovement playerMovement; 
    public PlayerInteract playerGrab; 
    public ToolbarManager toolbarManager; 
    public GameObject playerRoot; 
    public Camera playerCam;
    public Image fadeImage; 
    public float fadeTime; 
    [Header("Audio Stuff")]
    public AudioMixer audioMixer; 
    public AudioSource playerAudioSource;
    public AudioClip[] moveSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] grabSounds; 
    public AudioClip[] deathSounds; 

    [Header("Info")]
    private Color color; 
    public UnityEvent deathEvent; 
    private RaycastHit hit; 
    public PlayerInputActions playerInputActions;
    void Start()
    {
        color = fadeImage.color; 
        AddSettings(); 
    }
    void Awake() 
    {
        HandleInput(); 
    }
    void Update()
    {
        color.a = Mathf.Lerp(fadeImage.color.a, 0, fadeTime * Time.deltaTime);  
        fadeImage.color = color;   
    }

    public void PlayerDeath()
    {
        playerRoot.transform.position = SceneMaster.instance.respawnPoint.position; 
        deathEvent.Invoke(); 
        playerData.currentPlayerHealth = playerData.MaxPlayerHealth; 
        playerAudioSource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]); 

    }
    public void AddSettings()
    {
        //fov
        playerCam.fieldOfView = playerData.fieldOfView; 
        //health
        playerData.currentPlayerHealth = playerData.MaxPlayerHealth; 
        //volume
        audioMixer.SetFloat("VolParam", Mathf.Log10(playerData.volume) * 20); 
    }

    public void HandleInput()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable(); 
        //jump input 
        playerInputActions.Player.Jump.performed += playerMovement.PlayerJump;  
        //grab input
        playerInputActions.Player.Grabbing.performed += playerGrab.CheckForGrab;
        playerInputActions.Player.Grabbing.canceled += playerGrab.StartReleaseObject; 
        //Taskbar Input 
        playerInputActions.Player.TaskbarScroll.performed += toolbarManager.OnScroll;  
        //Button Interact Input
        playerInputActions.Player.ButtonPress.performed += playerGrab.ButtonPress; 
        playerInputActions.Player.ButtonPress.canceled += playerGrab.ButtonDepress; 
        //Sprint 
        playerInputActions.Player.Sprint.performed += playerMovement.SetSprintTrue;
        playerInputActions.Player.Sprint.canceled += playerMovement.SetSprintFalse;
    }
    public float Remap(float value, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float normal = Mathf.InverseLerp(oldLow, oldHigh, value);
        float newValue = Mathf.Lerp(newLow, newHigh, normal);
        return newValue;
    }

}
