using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMaster : MonoBehaviour
{
   #region Singleton
   public static SceneMaster instance; 

   void Awake() 
   {
       instance = this;  
   }
   void Update() 
   {
       
   }

   #endregion

   public GameObject player; 
   public Transform respawnPoint;  
}
