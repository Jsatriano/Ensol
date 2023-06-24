using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class GenSounds : MonoBehaviour
{
    private EventInstance generator;
    public GameObject genObject;
    private bool shutdown = false;

    // Start is called before the first frame update
    void Start()
    {
        generator = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);   
        generator.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        generator.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (genObject.active && !shutdown){
            shutdown = true;
            StartCoroutine(AudioShutdown());
        }
    }

    public IEnumerator AudioShutdown(){
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.generatorOff, this.transform.position);
        generator.stop(STOP_MODE.ALLOWFADEOUT);
        generator.release();
    }
}
