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


    private void Awake()
    {
        SpawnPoint.First = true;
    }

    private void Start()
    {
        CompletedNodes.prevNode = 9;
        CompletedNodes.firstLoad[9] = false;
        if (PlayerData.firstGenHit){
            genPanel01.enabled = false;
        }
        if (PlayerData.secondGenHit){
            genPanel02.enabled = false;
        }
        if (PlayerData.thirdGenHit){
            genPanel03.enabled = false;
        }
    }

    public void Update()
    {
        if (PlayerData.spiderKilled >= 1) 
        {
            story = new Story(globals.text);
            story.state.LoadJson(DialogueVariables.saveFile);
            story.EvaluateFunction("killedSpider");
            DialogueVariables.saveFile = story.state.ToJson();
        }

        // first generator
        if(genPanel01.enabled == false && gen1Shutdown == false)
        {
            gen1Shutdown = true;
            PlayerData.firstGenHit = true;
            StartCoroutine(GenShutdown(panObject1));
        }

        // second generator
        if(genPanel02.enabled == false && gen2Shutdown == false)
        {
            gen2Shutdown = true;
            PlayerData.secondGenHit = true;
            StartCoroutine(GenShutdown(panObject2));
        }

        // third generator
        if(genPanel03.enabled == false && gen3Shutdown == false)
        {
            gen3Shutdown = true;
            PlayerData.thirdGenHit = true;
            StartCoroutine(GenShutdown(panObject3));
        }

        //once all 3 generators are active, computerNode = true (this should make the computerNode X appear and the power grid image replace the X)
        if(PlayerData.firstGenHit && PlayerData.secondGenHit && PlayerData.thirdGenHit)
        {
            CompletedNodes.computerNode = true;
            CompletedNodes.completedNodes[9] = true;
        }

    }

    public IEnumerator GenShutdown(GameObject thisGenerator){
        yield return new WaitForSeconds(1f);
        thisGenerator.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
    }
}
