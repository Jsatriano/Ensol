using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _05RiverControlNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked security tower node");
            CompletedNodes.securityTowerNode = true;
        }
    }
}
