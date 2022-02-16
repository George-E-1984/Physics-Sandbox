using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couroutines : MonoBehaviour
{
    public float timeBeforeDestroyed; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Nala());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopAllCoroutines();
        }
       
    }

    IEnumerator Nala()
    {
        yield return new WaitForSeconds(timeBeforeDestroyed);
        Destroy(gameObject);
    }

}
