using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static int prevNode = 999;

    public static bool cabinNode, deerNode, riverNode, gateNode, riverControlNode,
                bearNode, brokenMachineNode, securityTowerNode, birdNode,
                powerGridNode, metalFieldNode, computerNode = false;

    public Button computerNodeButton;
    private int lastNode;

    public Sprite[] image;

    public GameObject[] mapButton;
    public GameObject[] mapScenery;

    /* 
    ------  KEY  ------
    mapButton[0] = cabin
    mapButton[1] = deer
    mapButton[2] = river
    mapButton[3] = gate
    mapButton[4] = river control
    mapButton[5] = bear
    mapButton[6] = broken machine
    mapButton[7] = security tower
    mapButton[8] = bird
    mapButton[9] = power grid
    mapButton[10] = metal field
    mapButton[11] = computer
    */

    public void Start()
    {
        cabinNode = true;
    }

    public void Update()
    {
        if(cabinNode)
        {
            mapButton[0].SetActive(true);
            mapScenery[0].SetActive(true);

        }
        if(deerNode)
        {
            // activate deer X
            mapButton[1].SetActive(true);
        }
        if(riverNode)
        {
            // activate river X
            mapButton[2].SetActive(true);

            // activate deer button and scenery
            mapButton[1].GetComponent<Button>().image.sprite = image[1];
            mapScenery[1].SetActive(true);
        }
        if(gateNode)
        {
            // activate gate X
            mapButton[3].SetActive(true);
        }
        if(riverControlNode)
        {
            // activate river control X
            mapButton[4].SetActive(true);

            // activate gate button and scenery
            mapButton[3].GetComponent<Button>().image.sprite = image[3];
            mapScenery[3].SetActive(true);
        }
        if(bearNode)
        {
            // activate bear X
            mapButton[5].SetActive(true);
            if(lastNode != 5 || lastNode != 7)
            {
                lastNode = 5;
            }

            // activate river button and scenery
            mapButton[2].GetComponent<Button>().image.sprite = image[2];
            mapScenery[2].SetActive(true);

        }
        if(brokenMachineNode)
        {
            // activate broken machine X
            mapButton[6].SetActive(true);

            if(lastNode == 5)
            {

                //activate bear button and scenery
                mapButton[5].GetComponent<Button>().image.sprite = image[5];
                mapScenery[5].SetActive(true);

            }
            if(lastNode == 7)
            {
                //activate security tower button and scenery
                mapButton[7].GetComponent<Button>().image.sprite = image[7];
                mapScenery[7].SetActive(true);

            }
        }
        if(securityTowerNode)
        {
            // activate security tower X
            mapButton[7].SetActive(true);
            if(lastNode != 5 && lastNode != 7)
            {
                lastNode = 7;
            }

            //activate river control button and scenery
            mapButton[4].GetComponent<Button>().image.sprite = image[4];
            mapScenery[4].SetActive(true);
        }
        if(birdNode)
        {
            // activate bird X
            mapButton[8].SetActive(true);

            // activate river button and scenery
            mapButton[2].GetComponent<Button>().image.sprite = image[2];
            mapScenery[2].SetActive(true);
        }
        if(powerGridNode)
        {
            // activate power grid X
            mapButton[9].SetActive(true);

            // activate broken machine button and scenery
            mapButton[7].GetComponent<Button>().image.sprite = image[7];
            mapScenery[7].SetActive(true);
        }
        if(metalFieldNode)
        {
            // activate metal field X
            mapButton[10].SetActive(true);

            // activate broken machine button and scenery
            mapButton[6].GetComponent<Button>().image.sprite = image[6];
            mapScenery[6].SetActive(true);
        }
        if(computerNode)
        {
            computerNodeButton.interactable = true;

            // activate metal field button and scenery
            mapButton[10].GetComponent<Button>().image.sprite = image[10];
            mapScenery[10].SetActive(true);

        }
        /* 
    ------  KEY  ------
    mapButton[0] = cabin
    mapButton[1] = deer
    mapButton[2] = river
    mapButton[3] = gate
    mapButton[4] = river control
    mapButton[5] = bear
    mapButton[6] = broken machine
    mapButton[7] = security tower
    mapButton[8] = bird
    mapButton[9] = power grid
    mapButton[10] = metal field
    mapButton[11] = computer
    */

        // ----------- CHECK WHAT NODE WE ARE AT, AND WHERE WE CAN GO -------------
        // no node yet visited
        if(prevNode == 999)
        {
            UninteractAll();
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
        }
        // last at cabin
        else if(prevNode == 0)
        {
            UninteractAll();
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[3].GetComponent<Button>().interactable = true; //gate
        }
        // last at deer
        else if(prevNode == 1)
        {
            UninteractAll();
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[2].GetComponent<Button>().interactable = true; //river
        }
        // last at river
        else if(prevNode == 2)
        {
            UninteractAll();
            mapButton[2].GetComponent<Button>().interactable = true; //river
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[8].GetComponent<Button>().interactable = true; //bird
        }
        // last at gate
        else if(prevNode == 3)
        {
            UninteractAll();
            mapButton[3].GetComponent<Button>().interactable = true; //gate
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[4].GetComponent<Button>().interactable = true; //river control
        }
        // last at river control
        else if(prevNode == 4)
        {
            UninteractAll();
            mapButton[4].GetComponent<Button>().interactable = true; //river control
            mapButton[3].GetComponent<Button>().interactable = true; //gate
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
        }
        // last at bear
        else if(prevNode == 5)
        {
            UninteractAll();
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[2].GetComponent<Button>().interactable = true; //river
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
        }
        // last at broken machine
        else if(prevNode == 6)
        {
            UninteractAll();
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
            mapButton[10].GetComponent<Button>().interactable = true;//metal field
        }
        // last at security tower
        else if(prevNode == 7)
        {
            UninteractAll();
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
            mapButton[4].GetComponent<Button>().interactable = true; //river control
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
        }
        // last at bird
        else if(prevNode == 8)
        {
            UninteractAll();
            mapButton[8].GetComponent<Button>().interactable = true; //bird
            mapButton[2].GetComponent<Button>().interactable = true; //river
        }
        // last at power grid
        else if(prevNode == 9)
        {
            UninteractAll();
            mapButton[9].GetComponent<Button>().interactable = true; //power grid
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
        }
        // last at metal field
        else if(prevNode == 10)
        {
            UninteractAll();
            mapButton[10].GetComponent<Button>().interactable = true;//metal field
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
            if(computerNode)
            {
                mapButton[11].GetComponent<Button>().interactable = true;//computer
            }
        }
        // last at computer
        else if(prevNode == 11)
        {
            UninteractAll();
            mapButton[11].GetComponent<Button>().interactable = true;//computer
            mapButton[10].GetComponent<Button>().interactable = true;//metal field
        }
        
    
    }

    // resets interactable buttons
    private void UninteractAll()
    {
        foreach (GameObject button in mapButton)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }
}
