using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _06BrokenMachineNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public GameObject interactableBear;
    public GameObject transferCube;

    private Story story;
    public TextAsset globals;

    private void Awake() 
    {
        //determine where to spawn
        if (CompletedNodes.prevNode == 5)
        {
            SpawnPoint.First = true;
        }
        else if (CompletedNodes.prevNode == 10)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 6;
    }

    private void Start()
    {
        //determine where to spawn part 2
        CompletedNodes.firstLoad[6] = false;

        // doesn't spawn interactable bear if player already has upgrade
        if(PlayerData.hasThrowUpgrade)
        {
            interactableBear.SetActive(false);
        }
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            CompletedNodes.metalFieldNode = true;
            CompletedNodes.completedNodes[6] = true;
        }

        // if bear has been interacted with, give player spear throw and unlock gate (if enemies are also killed)
        if(!interactableBear.activeInHierarchy)
        {
            PlayerData.hasThrowUpgrade = true;

            transferCube.SetActive(true);
        }
    }
}
