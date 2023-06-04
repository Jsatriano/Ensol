using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using FMOD.Studio;

public class _09PowerGridNode : MonoBehaviour
{
    //variables for generators being on or off

    private Story story;
    public TextAsset globals;

    public Collider genPanel01;
    public Collider genPanel02;
    public Collider genPanel03;
    private EventInstance generator1;
    private EventInstance generator2;
    private EventInstance generator3;


    private void Awake()
    {
        generator1 = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
        generator1.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(genPanel01.gameObject));
        generator2 = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
        generator2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(genPanel02.gameObject));
        generator3 = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
        generator3.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(genPanel03.gameObject));
    }

    private void Start()
    {
        CompletedNodes.prevNode = 9;
        CompletedNodes.firstLoad[9] = false;
        generator1.start();
        generator2.start();
        generator3.start();
    }

    public void Update()
    {
        // first generator
        if(genPanel01.enabled == false)
        {
            PlayerData.firstGenHit = true;
            genPanel01.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.generatorOff, this.transform.position);
            generator1.stop(STOP_MODE.ALLOWFADEOUT);
            generator1.release();
        }

        // second generator
        if(genPanel02.enabled == false)
        {
            PlayerData.secondGenHit = true;
            genPanel02.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.generatorOff, this.transform.position);
            generator2.stop(STOP_MODE.ALLOWFADEOUT);
            generator2.release();
        }

        // third generator
        if(genPanel03.enabled == false)
        {
            PlayerData.thirdGenHit = true;
            genPanel03.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.generatorOff, this.transform.position);
            generator3.stop(STOP_MODE.ALLOWFADEOUT);
            generator3.release();
        }

        //once all 3 generators are active, computerNode = true (this should make the computerNode X appear and the power grid image replace the X)
        if(PlayerData.firstGenHit && PlayerData.secondGenHit && PlayerData.thirdGenHit)
        {
            CompletedNodes.computerNode = true;
            CompletedNodes.completedNodes[9] = true;
        }
    }
}
