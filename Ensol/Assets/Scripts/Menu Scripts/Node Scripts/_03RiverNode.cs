using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _03RiverNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    [Header("Water Variables")]
    public GameObject water;
    public GameObject[] waterBounds;

    public void Update()
    {
        // if water turned off in river node, disable water and boundaries in this node
        if(_05RiverControlNode.controlsHit && water.activeInHierarchy)
        {
            water.SetActive(false);
            foreach (GameObject waterBound in waterBounds)
            {
                waterBound.SetActive(false);
            }
        }

        if(electricGateController.opening)
        {
            print("unlocked bird node");
            CompletedNodes.birdNode = true;
            print("unlocked bird node");
            CompletedNodes.bearNode = true;
        }
    }
}
