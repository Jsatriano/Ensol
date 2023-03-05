using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _04GateNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked river control node");
            CompletedNodes.riverControlNode = true;
        }
    }
}
