using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestController : MonoBehaviour
{
    public PauseMenu pauseScript;
    // Start is called before the first frame update
    void Awake()
    {
        pauseScript.InitializePlatestVars();
    }
}
