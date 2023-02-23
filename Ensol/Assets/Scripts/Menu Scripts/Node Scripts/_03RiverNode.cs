using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _03RiverNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked bird node");
            CompletedNodes.birdNode = true;
            print("unlocked bird node");
            CompletedNodes.bearNode = true;
        }
    }
}
