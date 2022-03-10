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
       forceShot.GetComponent<ForceShot>().toolbarManager = player.GetComponentInChildren<ToolbarManager>(); 
   }
   void Update() 
   {
       if (Input.GetKeyDown(KeyCode.P))
       {
           activeRagdoll.RagdollRevive(); 
       }
   }

   #endregion

   public GameObject player; 
   public Transform respawnPoint; 
   public GameObject forceShot;
   public ActiveRagdoll activeRagdoll; 
}
