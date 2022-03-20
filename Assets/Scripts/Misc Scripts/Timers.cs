using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers; 
using TMPro; 

public class Timers : MonoBehaviour
{
    public TextMeshPro text; 

    [Header("Set in Seconds")]
    public float timerTime; 

    private static System.Timers.Timer aTimer; 
    // Start is called before the first frame update
    void Start()
    {
        aTimer = new System.Timers.Timer(timerTime * 1000);
        aTimer.Elapsed += aTimer_Elapsed; 
    }

    private void aTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        print("Nala"); 
        aTimer.Stop(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            aTimer.Start(); 

        }

        text.text = aTimer.Interval.ToString(); 
    }

    
}
