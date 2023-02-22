using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static bool cabinNode, deerNode, riverNode, gateNode, riverControlNode,
                bearNode, brokenMachineNode, securityTowerNode, birdNode,
                powerGridNode, metalFieldNode, computerNode = false;

    public Button computerNodeButton;

    public GameObject[] mapButtons;
    public GameObject[] mapScenery;
    public GameObject[] undiscovered;

    /* 
    ------  KEY  ------
    mapButtons[0] = cabin
    mapButtons[1] = deer
    mapButtons[2] = river
    mapButtons[3] = gate
    mapButtons[4] = river control
    mapButtons[5] = bear
    mapButtons[6] = broken machine
    mapButtons[7] = security tower
    mapButtons[8] = bird
    mapButtons[9] = power grid
    mapButtons[10] = metal field
    mapButtons[11] = computer
    */

    public void Start()
    {
        cabinNode = true;
    }

    public void Update()
    {
        if(cabinNode)
        {
            mapButtons[0].SetActive(true);
            mapScenery[0].SetActive(true);
            undiscovered[0].SetActive(false);

        }
        if(deerNode)
        {
            undiscovered[0].SetActive(true);
            //mapButtons[1].SetActive(true);
            //mapScenery[1].SetActive(true);
        }
        if(riverNode)
        {
            //mapButtons[2].SetActive(true);
            //mapScenery[2].SetActive(true);
        }
        if(gateNode)
        {
            //mapButtons[3].SetActive(true);
            //mapScenery[3].SetActive(true);
        }
        if(riverControlNode)
        {
            //mapButtons[4].SetActive(true);
            //mapScenery[4].SetActive(true);
        }
        if(bearNode)
        {
            //mapButtons[5].SetActive(true);
            //mapScenery[5].SetActive(true);
        }
        if(brokenMachineNode)
        {
            //mapButtons[6].SetActive(true);
            //mapScenery[6].SetActive(true);
        }
        if(securityTowerNode)
        {
            //mapButtons[7].SetActive(true);
            //mapScenery[7].SetActive(true);
        }
        if(birdNode)
        {
            //mapButtons[8].SetActive(true);
            //mapScenery[8].SetActive(true);
        }
        if(powerGridNode)
        {
            //mapButtons[9].SetActive(true);
            //mapScenery[9].SetActive(true);
        }
        if(metalFieldNode)
        {
            //mapButtons[10].SetActive(true);
            //mapScenery[10].SetActive(true);
        }
        if(computerNode)
        {
            computerNodeButton.interactable = true;
        }
    }
}
