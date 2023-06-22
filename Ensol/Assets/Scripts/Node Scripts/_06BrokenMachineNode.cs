using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _06BrokenMachineNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    public GameObject interactableBear;
    public Collider bearUpgradeCollider;
    public GameObject transferCube;

    private Story story;
    public TextAsset globals;

    [Header("Interactable Object Interactors")]
    public DialogueTrigger bearInteractor;

    private void Awake() 
    {
        //determine where to spawn
        if (CompletedNodes.prevNode == 5)
        {
            SpawnPoint.First = true;
            SpawnPoint.Second = false;
        }
        else if (CompletedNodes.prevNode == 10)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = true;
        }
        else if (CompletedNodes.prevNode == 7)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = false;
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
        CompletedNodes.securityTowerNode = true;

        // doesn't spawn interactable bear if player already has upgrade
        if (PlayerData.hasThrowUpgrade)
        {
            interactableBear.SetActive(false);
            transferCube.SetActive(true);
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
        if(bearInteractor.interacted)
        {
            PlayerData.hasThrowUpgrade = true;
            interactableBear.SetActive(false);
            transferCube.SetActive(true);
        }
    }
}
