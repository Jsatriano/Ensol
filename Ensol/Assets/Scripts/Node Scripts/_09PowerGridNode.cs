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

    public GameObject panObject1;
    private bool gen1Shutdown = false;
    public GameObject panObject2;
    private bool gen2Shutdown = false;
    public GameObject panObject3;
    private bool gen3Shutdown = false;
    public Collider genPanel01;
    public Collider genPanel02;
    public Collider genPanel03;
    private EventInstance generator1;
    private EventInstance generator2;
    private EventInstance generator3;


    private void Awake()
    {
        SpawnPoint.First = true;
    }

    private void Start()
    {
        CompletedNodes.prevNode = 9;
        CompletedNodes.firstLoad[9] = false;
        if (!PlayerData.firstGenHit){
            generator1 = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
            generator1.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(genPanel01.gameObject));
            generator1.start();
        } else {
            genPanel01.enabled = false;
        }
        if (!PlayerData.secondGenHit){
            generator2 = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
            generator2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(genPanel02.gameObject));
            generator2.start();
        } else {
            genPanel02.enabled = false;
        }
        if (!PlayerData.thirdGenHit){
            generator3 = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
            generator3.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(genPanel03.gameObject));
            generator3.start();
        } else {
            genPanel03.enabled = false;
        }
    }

    public void Update()
    {
        // first generator
        if(genPanel01.enabled == false && gen1Shutdown == false)
        {
            gen1Shutdown = true;
            PlayerData.firstGenHit = true;
            StartCoroutine(GenShutdown(panObject1, generator1));
        }

        // second generator
        if(genPanel02.enabled == false && gen2Shutdown == false)
        {
            gen2Shutdown = true;
            PlayerData.secondGenHit = true;
            StartCoroutine(GenShutdown(panObject2, generator2));
        }

        // third generator
        if(genPanel03.enabled == false && gen3Shutdown == false)
        {
            gen3Shutdown = true;
            PlayerData.thirdGenHit = true;
            StartCoroutine(GenShutdown(panObject3, generator3));
        }

        //once all 3 generators are active, computerNode = true (this should make the computerNode X appear and the power grid image replace the X)
        if(PlayerData.firstGenHit && PlayerData.secondGenHit && PlayerData.thirdGenHit)
        {
            CompletedNodes.computerNode = true;
            CompletedNodes.completedNodes[9] = true;
        }

    }

    public IEnumerator GenShutdown(GameObject thisGenerator, EventInstance thisGenSound){
        yield return new WaitForSeconds(1f);
        thisGenerator.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.generatorOff, thisGenerator.transform.position);
        thisGenSound.stop(STOP_MODE.ALLOWFADEOUT);
        thisGenSound.release();
    }
}
