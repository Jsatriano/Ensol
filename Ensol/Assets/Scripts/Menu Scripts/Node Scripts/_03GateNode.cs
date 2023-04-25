using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _03GateNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private void Awake() 
    {
        print("lnode is " + CompletedNodes.lNode);
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
            SpawnPoint.First = SceneSwitch.exitFrom;
            print(SpawnPoint.First);
        }
        CompletedNodes.lNode = 3;
    }

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
