using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _11MetalFieldNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public void Update()
    {
        if(electricGateController.opening)
        {
            print("unlocked Computer node");
            CompletedNodes.computerNode = true;
        }
    }
}
