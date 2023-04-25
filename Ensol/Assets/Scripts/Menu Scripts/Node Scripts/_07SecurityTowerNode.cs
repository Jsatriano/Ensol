using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _07SecurityTowerNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Start()
    {
        CompletedNodes.prevNode = 7;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.brokenMachineNode = true;
            CompletedNodes.powerGridNode = true;
        }
    }
}
