using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _05BearNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Start()
    {
        CompletedNodes.prevNode = 5;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.brokenMachineNode = true;
        }
    }
}
