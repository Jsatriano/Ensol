using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _01CabinNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;
    public DoorController doorController = null;

    [Header("Other Variables")]
    public GameObject gateTransferCube;

    public void Update()
    {
        if(_02DeerNode.weaponPickedUp && !gateTransferCube.activeInHierarchy)
        {
            gateTransferCube.SetActive(true);
        }

        if(doorController.opening)
        {
            print("unlocked deer node");
            CompletedNodes.deerNode = true;
        }

        if(electricGateController.opening)
        {
            print("unlocked gate node");
            CompletedNodes.gateNode = true;
        }
    }
}
