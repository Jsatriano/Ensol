using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    private float timeElapsed; 
    private bool minuteMark = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        timeElapsed = (int) Time.realtimeSinceStartup % 60;
        //print(timeElapsed);

        if (timeElapsed < 1 && minuteMark == false){
            //print ("minute");
            minuteMark = true; //make sure not to update it multiple times a frame
            Steamworks.SteamUserStats.AddStat("Sandbox_Minutes", 1);
            Steamworks.SteamUserStats.StoreStats();
        }
        if (timeElapsed > 1 && minuteMark == true){
            //print ("minute restart");
            minuteMark = false;
        }
        
    }
}
