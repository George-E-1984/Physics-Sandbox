using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_destoryObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
       
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space))
      {
        Destroy(gameObject);
      
      }
   
    }

    private void OnMouseDown() 
    {
      Destroy(gameObject);  
    }

  
}
