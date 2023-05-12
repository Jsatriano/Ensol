using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static int prevNode = 999;

    public static int lNode = 0;

    public static bool cabinNode  = false, deerNode  = false, riverNode  = false, gateNode  = false, riverControlNode  = false,
                bearNode  = false, brokenMachineNode  = false, securityTowerNode  = false, birdNode  = false,
                powerGridNode  = false, metalFieldNode  = false, computerNode = false;

    public static bool[] nodes;            
    public static bool[] firstLoad = new bool[] {
        true, true, true, true, true,
        true, true, true, true, true,
    };

    public Button computerNodeButton;
    private int lastNode;

    public GameObject youAreHereCircle;

    public Sprite[] image;

    public GameObject[] mapButton;
    public GameObject[] mapScenery;
    public Slider[] mapSlider;
    

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
        nodes = new bool[12];
        nodes[0] = cabinNode;
        nodes[1] = deerNode;
        nodes[2] = riverNode;
        nodes[3] = gateNode;
        nodes[4] = riverControlNode;
        nodes[5] = bearNode;
        nodes[6] = brokenMachineNode;
        nodes[7] = securityTowerNode;
        nodes[8] = birdNode;
        nodes[9] = powerGridNode;
        nodes[10] = metalFieldNode;
        nodes[11] = computerNode;
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
            if(firstLoad[1])
            {
                SliderToggle(1);
            }
        }
        if(riverNode)
        {
            // activate river X
            mapButton[2].SetActive(true);
            if(firstLoad[2])
            {
                SliderToggle(2);
            }

            // activate deer button and scenery
            ImageSwapTransition(1);
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
            if(firstLoad[4])
            {
                SliderToggle(4);
            }

            // activate gate button and scenery
            ImageSwapTransition(3);
            mapScenery[3].SetActive(true);
        }
        if(bearNode)
        {
            // activate bear X
            mapButton[5].SetActive(true);
            if(firstLoad[5])
            {
                SliderToggle(5);
            }

            if(lastNode != 5 || lastNode != 7)
            {
                lastNode = 5;
            }

            // activate river button and scenery
            ImageSwapTransition(2);
            mapScenery[2].SetActive(true);

        }
        if(brokenMachineNode)
        {
            // activate broken machine X
            mapButton[6].SetActive(true);
            if(firstLoad[6])
            {
                SliderToggle(6);
            }

            if(lastNode == 5)
            {

                //activate bear button and scenery
                ImageSwapTransition(5);
                mapScenery[5].SetActive(true);

            }
            if(lastNode == 7)
            {
                //activate security tower button and scenery
                ImageSwapTransition(7);
                mapScenery[7].SetActive(true);

            }
        }
        if(securityTowerNode)
        {
            // activate security tower X
            mapButton[7].SetActive(true);
            if(firstLoad[7])
            {
                SliderToggle(7);
            }

            if(lastNode != 5 && lastNode != 7)
            {
                lastNode = 7;
            }

            //activate river control button and scenery
            ImageSwapTransition(4);
            mapScenery[4].SetActive(true);
        }
        if(birdNode)
        {
            // activate bird X
            mapButton[8].SetActive(true);
            if(firstLoad[8])
            {
                SliderToggle(8);
            }

            // activate river button and scenery
            ImageSwapTransition(2);
            mapScenery[2].SetActive(true);
        }
        if(powerGridNode)
        {
            // activate power grid X
            mapButton[9].SetActive(true);
            if(firstLoad[9])
            {
                SliderToggle(9);
            }

            // activate broken machine button and scenery
            ImageSwapTransition(7);
            mapScenery[7].SetActive(true);
        }
        if(metalFieldNode)
        {
            // activate metal field X
            mapButton[10].SetActive(true);
            if(firstLoad[10])
            {
                SliderToggle(10);
            }

            // activate broken machine button and scenery
            ImageSwapTransition(6);
            mapScenery[6].SetActive(true);
        }
        if(computerNode)
        {
            computerNodeButton.interactable = true;

            // activate metal field button and scenery
            ImageSwapTransition(10);
            mapScenery[10].SetActive(true);
        }




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
            TransformYouAreHereCircle(0, 2.4f, 2.4f, 2.4f);

            UninteractAll();
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[3].GetComponent<Button>().interactable = true; //gate
        }
        // last at deer
        else if(prevNode == 1)
        {
            TransformYouAreHereCircle(1, 1.25f, 1.25f, 1.25f);

            //firstLoad[1] = false;

            UninteractAll();
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[2].GetComponent<Button>().interactable = true; //river
        }
        // last at river
        else if(prevNode == 2)
        {
            TransformYouAreHereCircle(2, 1.4f, 1.4f, 1.4f);

            firstLoad[2] = false;

            UninteractAll();
            mapButton[2].GetComponent<Button>().interactable = true; //river
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[8].GetComponent<Button>().interactable = true; //bird
        }
        // last at gate
        else if(prevNode == 3)
        {
            TransformYouAreHereCircle(3, 1.4f, 1.4f, 1.4f);

            firstLoad[3] = false;

            UninteractAll();
            mapButton[3].GetComponent<Button>().interactable = true; //gate
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[4].GetComponent<Button>().interactable = true; //river control
        }
        // last at river control
        else if(prevNode == 4)
        {
            TransformYouAreHereCircle(4, 1.4f, 1.4f, 1.4f);

            firstLoad[4] = false;

            UninteractAll();
            mapButton[4].GetComponent<Button>().interactable = true; //river control
            mapButton[3].GetComponent<Button>().interactable = true; //gate
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
        }
        // last at bear
        else if(prevNode == 5)
        {
            TransformYouAreHereCircle(5, 1.3f, 1.3f, 1.3f);

            firstLoad[5] = false;

            UninteractAll();
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[2].GetComponent<Button>().interactable = true; //river
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
        }
        // last at broken machine
        else if(prevNode == 6)
        {
            TransformYouAreHereCircle(6, 1.4f, 1.4f, 1.4f);

            firstLoad[6] = false;

            UninteractAll();
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
            mapButton[10].GetComponent<Button>().interactable = true;//metal field
        }
        // last at security tower
        else if(prevNode == 7)
        {
            TransformYouAreHereCircle(7, 1.3f, 1.3f, 1.3f);

            firstLoad[7] = false;

            UninteractAll();
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
            mapButton[4].GetComponent<Button>().interactable = true; //river control
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
        }
        // last at bird
        else if(prevNode == 8)
        {
            TransformYouAreHereCircle(8, 1.3f, 1.3f, 1.3f);

            firstLoad[8] = false;

            UninteractAll();
            mapButton[8].GetComponent<Button>().interactable = true; //bird
            mapButton[2].GetComponent<Button>().interactable = true; //river
        }
        // last at power grid
        else if(prevNode == 9)
        {
            TransformYouAreHereCircle(9, 1.6f, 1.6f, 1.6f);

            firstLoad[9] = false;

            UninteractAll();
            mapButton[9].GetComponent<Button>().interactable = true; //power grid
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
        }
        // last at metal field
        else if(prevNode == 10)
        {
            TransformYouAreHereCircle(10, 1.6f, 1.6f, 1.6f);

            firstLoad[10] = false;

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
            TransformYouAreHereCircle(11, 2.55f, 2.55f, 2.55f);

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

    private void TransformYouAreHereCircle(int i, float x, float y, float z)
    {
        youAreHereCircle.transform.position = mapButton[i].transform.position;
        youAreHereCircle.transform.localScale = new Vector3(x, y, z);
    }

    // function for activating the X on any node
    private void SliderToggle(int i)
    {
        float lastVal = mapSlider[i].value;
        if(lastVal == 1f)
        {
            lastVal = 0f;
        }

        mapSlider[i].value = Mathf.Lerp(lastVal, 0.99f, 1f * Time.deltaTime);
    }

    // function for swapping the X on any node out for its actual image
    private void ImageSwapTransition(int i)
    {
        float lastVal = mapSlider[i].value;
        print(lastVal);

        if(mapButton[i].GetComponent<Button>().image.sprite != image[i])
        {
            if(lastVal > .05)
            {
                mapSlider[i].value = Mathf.Lerp(lastVal, 0f, 1f * Time.deltaTime);
            }
            else
            {
                mapButton[i].GetComponent<Button>().image.sprite = image[i];
            }
        }
        else
        {
            mapSlider[i].value = Mathf.Lerp(lastVal, 1f, 1f * Time.deltaTime);
        }
    }
}
