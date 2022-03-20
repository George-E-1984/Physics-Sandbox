using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSound : MonoBehaviour
{
    public AudioSource reloadAudioSource; 
    public void PlayAudio(AudioClip audioClip)
    {
        reloadAudioSource.pitch = Random.Range(0.8f, 1.2f); 
        reloadAudioSource.PlayOneShot(audioClip); 
    }
  
}
