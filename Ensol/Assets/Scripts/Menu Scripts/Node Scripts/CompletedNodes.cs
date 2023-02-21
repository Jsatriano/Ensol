using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedNodes : MonoBehaviour
{
    public bool cabinNode, deerNode, riverNode, gateNode, riverControlNode,
                bearNode, brokenMachineNode, securityTowerNode, birdNode,
                powerGridNode, metalFieldNode, computerNode = false;

    public GameObject[] mapButton;
    
    
    public void Start()
    {
        cabinNode = true;
        deerNode = true;
    }

    public void Update()
    {
        if(cabinNode == true)
        {
            mapButton[0].SetActive(true);
        }
        if(deerNode == true)
        {
            mapButton[1].SetActive(true);
        }
        if(riverNode == true)
        {
            mapButton[2].SetActive(true);
        }
        if(gateNode == true)
        {
            mapButton[3].SetActive(true);
        }
    }
}
