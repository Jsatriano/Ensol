using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestController : MonoBehaviour
{
    public PauseMenu pauseScript;
    // Start is called before the first frame update
    void Start()
    {
        pauseScript.InitializePlatestVars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
