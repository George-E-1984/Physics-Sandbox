using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using UnityEngine.InputSystem; 

public class PauseMenu : MonoBehaviour
{ 
    [Header("Assignables")]
    public GameObject pauseMenuObj; 
    public GameObject playerUi; 
    public PlayerMovement playerMovement;
    public OptionsSettings optionsSettings;  
    public SceneDealer sceneDealer; 

    [Header("Info")]
    private bool isMenuOpen = false;

    
    void Start()
    {
        
    }

    
    void Update()
    {
    }

    public void StartPause(InputAction.CallbackContext context)
    {
        if (!isMenuOpen)
        {
            HandlePause(true);     
            PlayerManager.instance.HandleInput(false); 
        }
    }

    public void HandlePause(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0; 
            playerMovement.canLook = false; 
            pauseMenuObj.SetActive(true); 
            playerUi.SetActive(false); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
        }
        else
        {
            Time.timeScale = 1; 
            playerMovement.canLook = true;
            pauseMenuObj.SetActive(false); 
            playerUi.SetActive(true); 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }


    }

    public void HandleQuit()
    {
        sceneDealer.sceneToTransitionToName = "Menu"; 
        Time.timeScale = 1; 
        optionsSettings.SavePrefs(); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Loading_Screen"); 
    }
}
