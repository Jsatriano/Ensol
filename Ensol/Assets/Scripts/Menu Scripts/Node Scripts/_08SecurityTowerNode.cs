using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _08SecurityTowerNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked broken machine node");
            CompletedNodes.brokenMachineNode = true;
            
            print("unlocked power grid node");
            CompletedNodes.powerGridNode = true;
        }
    }
}
