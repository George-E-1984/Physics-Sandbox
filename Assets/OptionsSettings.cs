using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.Events; 
using UnityEngine.InputSystem; 

public class OptionsSettings : MonoBehaviour
{
        
    public PlayerData playerData; 
    public PlayerManager playerManager; 
    public Slider sensitivitySlider; 
    public Slider volumeSlider; 
    public Slider fovSlider; 
    public TextMeshProUGUI sensitivityProgressText;
    public TextMeshProUGUI volumeProgressText;
    public TextMeshProUGUI fovProgressText;
    public UnityEvent onEscapePressed; 

    [Header("Info")]
    const string sensitivityKey = "Sensitivity"; 
    const string fovKey = "Field of View"; 
    const string volumeKey = "Volume"; 
    PlayerInputActions playerInputActions; 

    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs(); 
    }
    
    void OnEnable() 
    {
        InitiateInputs(true); 
    }

    // Update is called once per frame
    void Update()
    {
        if(playerManager)
        {
            playerManager.AddSettings();
        } 
        else
        {
            playerData.audioMixer.SetFloat("VolParam", Mathf.Log10(playerData.volume) * 20);
        }
        //sensitivity
        playerData.sensitivity = sensitivitySlider.value; 
        sensitivityProgressText.text = System.Convert.ToString(sensitivitySlider.normalizedValue * 100) + "%";  
        //volume
        playerData.volume = volumeSlider.value; 
        volumeProgressText.text = (volumeSlider.value * 100).ToString("F0") + "%";
        //fov
        playerData.fieldOfView = fovSlider.value; 
        fovProgressText.text = System.Convert.ToString(fovSlider.value); 
    }

    public void LoadPrefs()
    {
        playerData.sensitivity = PlayerPrefs.GetFloat(sensitivityKey); 
        playerData.fieldOfView = PlayerPrefs.GetFloat(fovKey); 
        playerData.volume = PlayerPrefs.GetFloat(volumeKey); 
        sensitivitySlider.value = playerData.sensitivity; 
        volumeSlider.value = playerData.volume; 
        fovSlider.value = playerData.fieldOfView;

    }

    public void InitiateInputs(bool setActive)
    {
        playerInputActions = new PlayerInputActions(); 
        playerInputActions.PauseMenu.Backoutofmenu.performed += HandleEscape; 
        if (setActive)
        {
            playerInputActions.PauseMenu.Enable();
        }
        else 
        {
            playerInputActions.PauseMenu.Disable();
        }
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetFloat(sensitivityKey, playerData.sensitivity);
        PlayerPrefs.SetFloat(fovKey, playerData.fieldOfView); 
        PlayerPrefs.SetFloat(volumeKey, playerData.volume); 
    }

    public void HandleEscape(InputAction.CallbackContext context)
    {
        InitiateInputs(false); 
        onEscapePressed.Invoke(); 
    }

}
