using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PlayerManager : MonoBehaviour
{
    
    public PlayerData playerData; 
    public GameObject playerRoot; 
    public Image fadeImage; 
    public float fadeTime; 
    private Color color; 
    void Start()
    {
        color = fadeImage.color; 
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
    
}
