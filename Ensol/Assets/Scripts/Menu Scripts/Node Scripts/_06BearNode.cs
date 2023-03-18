using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _06BearNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.brokenMachineNode = true;
        }
    }
}
