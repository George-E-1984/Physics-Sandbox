using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public ActiveRagdoll activeRagdoll; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            print("nala"); 
            PlayerData playerData = other.gameObject.GetComponent<PlayerMovement>().playerData; 
            other.gameObject.GetComponent<PlayerMovement>().playerData.currentPlayerHealth -= activeRagdoll.activeRagdollObject.attackDamage; 
            if (playerData.currentPlayerHealth <= 0)
            {
                other.gameObject.GetComponent<PlayerManager>().PlayerDeath(); 
            }
            
        }
    }


}
