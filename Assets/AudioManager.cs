using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    
    void Start()
    {
        
    }
    void Awake() 
    {
        instance = this; 
    }

    
    void Update()
    {
        
    }

    [Header("Assign")]
    public AudioMixer masterAudioMixer; 

    #endregion
}
