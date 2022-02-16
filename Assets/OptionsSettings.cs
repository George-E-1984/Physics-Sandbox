using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

public class OptionsSettings : MonoBehaviour
{
        
    public PlayerData playerData; 
    public PlayerMovement playerMovement; 
    public Slider sensitivitySlider; 
    public Slider volumeSlider; 
    public Slider fovSlider; 
    public TextMeshProUGUI sensitivityProgressText;
    public TextMeshProUGUI volumeProgressText;
    public TextMeshProUGUI fovProgressText;

    // Start is called before the first frame update
    void Start()
    {
        sensitivitySlider.value = playerData.sensitivity; 
        volumeSlider.value = playerData.volume; 
        fovSlider.value = playerData.fieldOfView; 

    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement)
        {
            playerMovement.AddSettings();
        } 
        //sensitivity
        playerData.sensitivity = sensitivitySlider.value; 
        sensitivityProgressText.text = System.Convert.ToString(sensitivitySlider.normalizedValue * 100) + "%";  
        //volume
        playerData.volume = volumeSlider.value; 
        volumeProgressText.text = System.Convert.ToString(volumeSlider.normalizedValue * 100) + "%";
        //fov
        playerData.fieldOfView = fovSlider.value; 
        fovProgressText.text = System.Convert.ToString(fovSlider.value); 
        
    }

}
