using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

public class OptionsSettings : MonoBehaviour
{
    public PlayerData playerData;  
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensPC; 
    public TextMeshProUGUI volumePC; 
    public Slider volumeSlider;  
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
       playerData.sensitivity = sensitivitySlider.value; 
       sensPC.text = System.Convert.ToString(sensitivitySlider.normalizedValue * 100) + "%";  
       playerData.volume = volumeSlider.value; 
       volumePC.text = System.Convert.ToString(volumeSlider.normalizedValue * 100) + "%";
    }

}
