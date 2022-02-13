using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerManager : MonoBehaviour
{
    
    public PlayerData playerData; 
    public GameObject playerRoot; 
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void PlayerDeath()
    {
        playerRoot.transform.position = SceneMaster.instance.respawnPoint.position; 
        playerData.currentPlayerHealth = playerData.MaxPlayerHealth; 

    }
}
