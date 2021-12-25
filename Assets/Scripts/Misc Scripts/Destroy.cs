using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject brokenObject;
    public GameObject Object;
    public float objectHealth; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(objectHealth == 0f)
        {
            
            Destroy(gameObject); 
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(objectHealth > 0 )
        {
            objectHealth--;
        }
        
    }
}
