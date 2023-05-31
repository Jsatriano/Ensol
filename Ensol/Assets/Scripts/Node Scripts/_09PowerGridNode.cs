using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _09PowerGridNode : MonoBehaviour
{
    //variables for generators being on or off

    private Story story;
    public TextAsset globals;

    public Collider genPanel01;
    public Collider genPanel02;
    public Collider genPanel03;

    private void Start()
    {
        CompletedNodes.prevNode = 9;
        CompletedNodes.firstLoad[9] = false;
    }

    public void Update()
    {
        // first generator
        if(genPanel01.enabled == false)
        {
            PlayerData.firstGenHit = true;
            genPanel01.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        }

        // second generator
        if(genPanel02.enabled == false)
        {
            PlayerData.secondGenHit = true;
            genPanel02.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        }

        // third generator
        if(genPanel03.enabled == false)
        {
            PlayerData.thirdGenHit = true;
            genPanel03.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        }

        //once all 3 generators are active, computerNode = true (this should make the computerNode X appear and the power grid image replace the X)
        if(PlayerData.firstGenHit && PlayerData.secondGenHit && PlayerData.thirdGenHit)
        {
            CompletedNodes.computerNode = true;
            CompletedNodes.completedNodes[9] = true;
        }
    }
}
