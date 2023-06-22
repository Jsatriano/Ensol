using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class ElectricFieldBuzz : MonoBehaviour
{
    private EventInstance electricArc;
    // Start is called before the first frame update
    void Start()
    {
        electricArc = AudioManager.instance.CreateEventInstance(FMODEvents.instance.envFieldNodeBuzz);
        electricArc.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject)); 
        electricArc.start();
    }

    void OnDisable(){
        electricArc.stop(STOP_MODE.IMMEDIATE);
        electricArc.release();
    }
}
