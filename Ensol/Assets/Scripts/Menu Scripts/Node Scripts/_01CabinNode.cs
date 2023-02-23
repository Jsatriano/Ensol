using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _01CabinNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;
    public DoorController doorController = null;

    public void Update()
    {
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
