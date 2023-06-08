using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class WaterSounds : MonoBehaviour
{
    private EventInstance river;
    // Start is called before the first frame update
    void Start()
    {
        if(_04RiverControlNode.riverOn == true)
        {
            river = AudioManager.instance.CreateEventInstance(FMODEvents.instance.envRiver);
            river.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            river.start();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable(){
        river.stop(STOP_MODE.ALLOWFADEOUT);
        river.release();
    }
}
