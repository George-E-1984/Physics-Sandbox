using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSettings : MonoBehaviour
{
    public OptionsSettings optionsSettings; 
    void Start()
    {
        optionsSettings.LoadPrefs(); 
        if(optionsSettings.playerManager)
        {
            optionsSettings.playerManager.AddSettings();
        } 
        else
        {
            optionsSettings.playerData.audioMixer.SetFloat("VolParam", Mathf.Log10(optionsSettings.playerData.volume) * 20);
        }
    }
}
