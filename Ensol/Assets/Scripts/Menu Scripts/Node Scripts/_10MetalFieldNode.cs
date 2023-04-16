using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _10MetalFieldNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Start()
    {
        CompletedNodes.prevNode = 10;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.computerNode = true;
        }
    }
}
