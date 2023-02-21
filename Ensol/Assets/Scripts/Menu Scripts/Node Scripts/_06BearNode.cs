using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _06BearNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked broken machine node");
            CompletedNodes.brokenMachineNode = true;
        }
    }
}
