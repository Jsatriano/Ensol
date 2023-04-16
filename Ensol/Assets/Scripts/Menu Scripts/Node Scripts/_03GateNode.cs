using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _03GateNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Start()
    {
        CompletedNodes.prevNode = 3;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.riverControlNode = true;
        }
    }
}
