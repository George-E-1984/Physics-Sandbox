using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSum : MonoBehaviour
{
    public int a; 
    // Start is called before the first frame update
    void Start()
    {
          for (int i = 0; i < a; i++)
        {
            print(i + (i + 1));   
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
