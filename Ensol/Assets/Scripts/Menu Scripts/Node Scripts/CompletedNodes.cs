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
    private int lastNode;

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
            // activate deer X
            undiscovered[1].SetActive(true);
        }
        if(riverNode)
        {
            // activate river X
            undiscovered[2].SetActive(true);

            // deactivate deer X
            undiscovered[1].SetActive(false);
            // activate deer button and scenery
            mapButtons[1].SetActive(true);
            mapScenery[1].SetActive(true);
        }
        if(gateNode)
        {
            // activate gate X
            undiscovered[3].SetActive(true);
        }
        if(riverControlNode)
        {
            // activate river control X
            undiscovered[4].SetActive(true);

            // deactivate gate X
            undiscovered[3].SetActive(false);
            // activate gate button and scenery
            mapButtons[3].SetActive(true);
            mapScenery[3].SetActive(true);
        }
        if(bearNode)
        {
            // activate bear X
            undiscovered[5].SetActive(true);
            if(lastNode != 5 || lastNode != 7)
            {
                lastNode = 5;
            }

            // deactivate river X
            undiscovered[2].SetActive(false);
            // activate river button and scenery
            mapButtons[2].SetActive(true);
            mapScenery[2].SetActive(true);

        }
        if(brokenMachineNode)
        {
            // activate broken machine X
            undiscovered[6].SetActive(true);

            if(lastNode == 5)
            {
                // deactivate bear X
                undiscovered[5].SetActive(false);
                //activate bear button and scenery
                mapButtons[5].SetActive(true);
                mapScenery[5].SetActive(true);

            }
            if(lastNode == 7)
            {
                // deactivate security tower X
                undiscovered[7].SetActive(false);
                //activate security tower button and scenery
                mapButtons[7].SetActive(true);
                mapScenery[7].SetActive(true);

            }
        }
        if(securityTowerNode)
        {
            // activate security tower X
            undiscovered[7].SetActive(true);
            if(lastNode != 5 && lastNode != 7)
            {
                lastNode = 7;
            }

            //deactivate river control X
            undiscovered[4].SetActive(false);
            //activate river control button and scenery
            mapButtons[4].SetActive(true);
            mapScenery[4].SetActive(true);
        }
        if(birdNode)
        {
            // activate bird X
            undiscovered[8].SetActive(true);

            // deactivate river X
            undiscovered[2].SetActive(false);
            // activate river button and scenery
            mapButtons[8].SetActive(true);
            mapScenery[8].SetActive(true);
        }
        if(powerGridNode)
        {
            // activate power grid X
            undiscovered[9].SetActive(true);

            // deactivate broken machine X
            undiscovered[7].SetActive(false);
            // activate broken machine button and scenery
            mapButtons[7].SetActive(true);
            mapScenery[7].SetActive(true);
        }
        if(metalFieldNode)
        {
            // activate metal field X
            undiscovered[10].SetActive(true);

            // deactivate broken machine X
            undiscovered[6].SetActive(false);
            // activate broken machine button and scenery
            mapButtons[6].SetActive(true);
            mapScenery[6].SetActive(true);
        }
        if(computerNode)
        {
            computerNodeButton.interactable = true;

            // deactivate metal field X
            undiscovered[10].SetActive(false);
            // activate metal field button and scenery
            mapButtons[10].SetActive(true);
            mapScenery[10].SetActive(true);

        }
    }
}
