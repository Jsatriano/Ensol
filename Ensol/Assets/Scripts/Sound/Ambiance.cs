using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Ambiance : MonoBehaviour
{

    public EventInstance zoneAmb;
    public EventInstance zoneAmb2;
    public GameObject computer;
    public EndingManager endMNG;

    // Start is called before the first frame update
    void Start()
    {
        zoneAmb = AudioManager.instance.CreateEventInstance(FMODAmbianceEvents.instance.zoneAmbiance); 
        zoneAmb.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(computer.gameObject));
        zoneAmb.start();    
        zoneAmb2 = AudioManager.instance.CreateEventInstance(FMODAmbianceEvents.instance.zoneAmbiance2); 
        zoneAmb2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(computer.gameObject));
        zoneAmb2.start();   
    }

    void Update()
    {
        if (endMNG.killAmbiance){
            zoneAmb.stop(STOP_MODE.ALLOWFADEOUT);
            zoneAmb2.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    void OnDestroy()
    {
        zoneAmb.stop(STOP_MODE.IMMEDIATE);
        zoneAmb.release();
        zoneAmb2.stop(STOP_MODE.IMMEDIATE);
        zoneAmb2.release();
    }
}
