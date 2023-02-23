using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02DeerNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked river node");
            CompletedNodes.riverNode = true;
        }
    }
}
