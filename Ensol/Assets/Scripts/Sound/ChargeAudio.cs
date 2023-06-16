using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class ChargeAudio : MonoBehaviour
{
    private EventInstance charge;
    // Start is called before the first frame update
    void OnEnable()
    {
        //print ("playing charge audio");
        charge = AudioManager.instance.CreateEventInstance(FMODEvents.instance.envStampede);
        charge.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        charge.start();
        
    }


    void OnDisable(){
        charge.stop(STOP_MODE.ALLOWFADEOUT);
        charge.release();
    }
}
