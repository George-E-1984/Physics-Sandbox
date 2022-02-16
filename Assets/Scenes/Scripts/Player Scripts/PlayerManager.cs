using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 
using UnityEngine.Audio; 

public class PlayerManager : MonoBehaviour
{
    
    public PlayerData playerData; 
    public GameObject playerRoot; 
    public Camera playerCam;
    public Image fadeImage; 
    public AudioMixer audioMixer; 
    public float fadeTime; 
    private Color color; 
    void Start()
    {
        color = fadeImage.color; 
        AddSettings(); 
    }
    void Update()
    {
        color.a = Mathf.Lerp(fadeImage.color.a, 0, fadeTime * Time.deltaTime);  
        fadeImage.color = color;  
    }

    public void PlayerDeath()
    {
        playerRoot.transform.position = SceneMaster.instance.respawnPoint.position; 
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
    public float Remap(float value, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float normal = Mathf.InverseLerp(oldLow, oldHigh, value);
        float newValue = Mathf.Lerp(newLow, newHigh, normal);
        return newValue;
    }
}
