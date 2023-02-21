using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static bool cabinNode, deerNode, riverNode, gateNode, riverControlNode,
                bearNode, brokenMachineNode, securityTowerNode, birdNode,
                powerGridNode, metalFieldNode, computerNode = false;

    public GameObject[] mapButtons;
    public Button[] editMapButtons;
    
    

    public void Start()
    {
        cabinNode = true;
    }

    public void Update()
    {
        if(cabinNode)
        {
            mapButtons[0].SetActive(true);
        }
        if(deerNode)
        {
            mapButtons[1].SetActive(true);
        }
        if(riverNode)
        {
            mapButtons[2].SetActive(true);
        }
        if(gateNode)
        {
            mapButtons[3].SetActive(true);
        }
        if(riverControlNode)
        {
            mapButtons[4].SetActive(true);
        }
        if(bearNode)
        {
            mapButtons[5].SetActive(true);
        }
        if(brokenMachineNode)
        {
            mapButtons[6].SetActive(true);
        }
        if(securityTowerNode)
        {
            mapButtons[7].SetActive(true);
        }
        if(birdNode)
        {
            mapButtons[8].SetActive(true);
        }
        if(powerGridNode)
        {
            mapButtons[9].SetActive(true);
        }
        if(metalFieldNode)
        {
            mapButtons[10].SetActive(true);
        }
        if(computerNode)
        {
            editMapButtons[11].interactable = true;
        }
    }
}
