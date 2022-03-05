using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartActivation : MonoBehaviour
{
    public enum options {disableOnStart, activateOnStart}; 
    public options setting; 
    public GameObject[] objectsToHandle; 
    void Start()
    {
        if (setting == options.activateOnStart)
        {
           ObjectActivation(true); 
        }
        else 
        {
            ObjectActivation(false); 
        }
    }

    void ObjectActivation(bool activate)
    {
        for (int i = 0; i < objectsToHandle.Length; i++)
        {
           objectsToHandle[i].SetActive(activate);
        }
    }
}
