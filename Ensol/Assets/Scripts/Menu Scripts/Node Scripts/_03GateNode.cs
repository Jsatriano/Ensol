using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _03GateNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Start()
    {
        if (CompletedNodes.lNode == 0)
        {
            SpawnPoint.First = true;
        }
        else if (CompletedNodes.lNode == 4)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            print("first is being set from exitfrom");
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.lNode = 3;
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
