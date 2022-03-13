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
    public PlayerGrab playerGrab; 
    public ToolbarManager toolbarManager; 
    public GameObject playerRoot; 
    public Camera playerCam;
    public Image fadeImage; 
    public AudioMixer audioMixer; 
    public float fadeTime; 
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
    }
    public float Remap(float value, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float normal = Mathf.InverseLerp(oldLow, oldHigh, value);
        float newValue = Mathf.Lerp(newLow, newHigh, normal);
        return newValue;
    }
}
