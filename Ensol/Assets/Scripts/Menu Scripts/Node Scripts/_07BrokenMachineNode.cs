using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _07BrokenMachineNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked metal field node");
            CompletedNodes.metalFieldNode = true;
        }
    }
}
