using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player options")]
    public int MaxPlayerHealth; 
    public int currentPlayerHealth;
    [Header("Player menu options")]
    public float sensitivity; 
    public float volume; 
    public float fieldOfView; 
    public GameObject playerPrefab;   
}



