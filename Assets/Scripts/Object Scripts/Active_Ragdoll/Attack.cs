using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public ActiveRagdoll activeRagdoll; 
    public AudioSource attackAudioSource; 
    public AudioClip[] attackSounds; 
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
            attackAudioSource.pitch = Random.Range(0.8f, 1.2f); 
            attackAudioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]); 
            PlayerData playerData = other.gameObject.GetComponent<PlayerMovement>().playerData; 
            other.gameObject.GetComponent<PlayerMovement>().playerData.currentPlayerHealth -= activeRagdoll.activeRagdollObject.attackDamage; 
            if (playerData.currentPlayerHealth <= 0)
            {
                other.gameObject.GetComponent<PlayerManager>().PlayerDeath(); 
            }
            
        }
    }


}
