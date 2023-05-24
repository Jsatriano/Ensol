using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _09PowerGridNode : MonoBehaviour
{
    //variables for generators being on or off

    private Story story;
    public TextAsset globals;


    private void Start()
    {
        CompletedNodes.prevNode = 9;
        CompletedNodes.firstLoad[9] = false;
    }

    public void Update()
    {
        //once all 3 generators are active, computerNode = true (this should make the computerNode X appear and the power grid image replace the X)
        /*
        if (...)
        {
            CompletedNodes.computerNode = true;
        }
        */
    }
}
