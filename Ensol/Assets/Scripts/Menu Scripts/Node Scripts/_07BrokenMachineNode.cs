using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _07BrokenMachineNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.metalFieldNode = true;
        }
    }
}
