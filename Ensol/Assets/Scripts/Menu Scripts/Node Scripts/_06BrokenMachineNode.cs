using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _06BrokenMachineNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private Story story;
    public TextAsset globals;

    private void Awake() 
    {
        //determine where to spawn
        if (CompletedNodes.lNode == 5)
        {
            SpawnPoint.First = true;
        }
        else if (CompletedNodes.lNode == 10)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.lNode = 6;
    }

    private void Start()
    {
        //determine where to spawn part 2
        CompletedNodes.prevNode = 6;
        CompletedNodes.firstLoad[6] = false;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.metalFieldNode = true;
            CompletedNodes.completedNodes[6] = true;
        }
    }
}
