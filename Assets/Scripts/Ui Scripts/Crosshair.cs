using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour

{
    private Animator animator; 
    public PlayerInteraction playerInteraction;


    // Start is called before the first frame update
    void Start()
    {
       animator = gameObject.GetComponent<Animator>();
       playerInteraction = PlayerManager.instance.playerInteraction; 
       
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
