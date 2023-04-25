using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _06BrokenMachineNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Start()
    {
        CompletedNodes.prevNode = 6;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.metalFieldNode = true;
        }
    }
}
