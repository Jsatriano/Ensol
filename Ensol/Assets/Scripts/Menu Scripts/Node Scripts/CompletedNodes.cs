using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static int prevNode = 999;

    public static int lNode = 0;

    public static bool cabinNode  = false, deerNode  = false, riverNode  = false, gateNode  = true, riverControlNode  = false,
                bearNode  = false, brokenMachineNode  = false, securityTowerNode  = false, birdNode  = false,
                powerGridNode  = false, metalFieldNode  = false, computerNode = false;

    public static bool[] nodes;            
    public static bool[] firstLoad = new bool[] {
        false, true, true, true, true, true,
        true, true, true, true, false
    };
    public static bool[] firstTransition = new bool[] {
        false, true, true, true, true,
        true, true, true, true, true, false
    };

    public Button computerNodeButton;
    private int lastNode;

    public GameObject youAreHereCircle;
    [SerializeField] private float circleDrawRate;
    [SerializeField] private Image circleSlider;
    [SerializeField] private List<float> circleScales;
    [SerializeField] private float circleWaitTime;

    [SerializeField] private NodeSelector nodeSelector;

    public Sprite[] image;

    public GameObject[] mapButton;
    public GameObject[] mapScenery;
    public Slider[] mapSlider;
    

    /* 
    ------  KEY  ------
    0 = cabin
    1 = deer
    2 = river
    3 = gate
    4 = river control
    5 = bear
    6 = broken machine
    7 = security tower
    8 = bird
    9 = power grid
    10 = metal field
    11 = computer
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
        float waitTime;

        if (firstTransition[PlayerData.prevNode-1] && firstLoad[PlayerData.currentNode-1])
        {
            waitTime = circleWaitTime * 7f;
        }
        else if ((!firstTransition[PlayerData.prevNode - 1] && firstLoad[PlayerData.currentNode - 1]) || firstTransition[PlayerData.prevNode - 1] && !firstLoad[PlayerData.currentNode - 1])
        {
            waitTime = circleWaitTime * 5f;
        }
        else
        {
            waitTime = circleWaitTime;
        }
        StartCoroutine(ChangeCircleLocation(PlayerData.prevNode-1, PlayerData.currentNode-1, waitTime));
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
            if(firstLoad[1])
            {
                StartCoroutine(SliderToggle(1, 0f));
            }
            else
            {
                mapButton[1].SetActive(true);
            }
        }
        if(riverNode)
        {
            // activate river X
            if(firstLoad[2])
            {
                StartCoroutine(SliderToggle(2, 3f));
            }
            else
            {
                mapButton[2].SetActive(true);
            }

            // activate deer button and scenery
            if(firstTransition[1])
            {
                ImageSwapTransition(1);
                SceneryToggle(1);
            }
            else
            {
                ImageAndScenery(1);
            }
        }
        if(gateNode)
        {
            // activate gate X
            if(firstLoad[3])
            {
                StartCoroutine(SliderToggle(3, 3f));
            }
            else
            {
                mapButton[3].SetActive(true);
            }
        }
        if(riverControlNode)
        {
            // activate river control X
            mapButton[4].SetActive(true);
            if(firstLoad[4])
            {
                StartCoroutine(SliderToggle(4, 3f));
            }
            else
            {
                mapButton[4].SetActive(true);
            }

            // activate gate button and scenery
            if(firstTransition[3])
            {
                ImageSwapTransition(3);
                SceneryToggle(3);
            }
            else
            {
                ImageAndScenery(3);
            }
        }
        if(bearNode)
        {
            // activate bear X
            mapButton[5].SetActive(true);
            if(firstLoad[5])
            {
                StartCoroutine(SliderToggle(5, 3f));
            }
            else
            {
                mapButton[5].SetActive(true);
            }

            if(lastNode != 5 || lastNode != 7)
            {
                lastNode = 5;
            }

            // activate river button and scenery
            if(firstTransition[2])
            {
                ImageSwapTransition(2);
                SceneryToggle(2);
            }
            else
            {
                ImageAndScenery(2);
            }
        }
        if(brokenMachineNode)
        {
            // activate broken machine X
            mapButton[6].SetActive(true);
            if(firstLoad[6])
            {
                StartCoroutine(SliderToggle(6, 3f));
            }
            else
            {
                mapButton[6].SetActive(true);
            }

            if(lastNode == 5)
            {

                //activate bear button and scenery
                if(firstTransition[5])
                {
                    ImageSwapTransition(5);
                    SceneryToggle(5);
                }
                else
                {
                    ImageAndScenery(5);
                }
            }
            if(lastNode == 7)
            {
                //activate security tower button and scenery
                if(firstTransition[7])
                {
                    ImageSwapTransition(7);
                    SceneryToggle(7);
                }
                else
                {
                    ImageAndScenery(7);
                }
            }
        }
        if(securityTowerNode)
        {
            // activate security tower X
            mapButton[7].SetActive(true);
            if(firstLoad[7])
            {
                StartCoroutine(SliderToggle(7, 3f));
            }
            else
            {
                mapButton[7].SetActive(true);
            }

            if(lastNode != 5 && lastNode != 7)
            {
                lastNode = 7;
            }

            //activate river control button and scenery
            if(firstTransition[4])
            {
                ImageSwapTransition(4);
                SceneryToggle(4);
            }
            else
            {
                ImageAndScenery(4);
            }
        }
        if(birdNode)
        {
            // activate bird X
            mapButton[8].SetActive(true);
            if(firstLoad[8])
            {
                StartCoroutine(SliderToggle(8, 3f));
            }
            else
            {
                mapButton[8].SetActive(true);
            }

            // activate river button and scenery
            if(firstTransition[2])
            {
                ImageSwapTransition(2);
                SceneryToggle(2);
            }
            else
            {
                ImageAndScenery(2);
            }
        }
        if(powerGridNode)
        {
            // activate power grid X
            mapButton[9].SetActive(true);
            if(firstLoad[9])
            {
                StartCoroutine(SliderToggle(9, 3f));
            }
            else
            {
                mapButton[9].SetActive(true);
            }

            // activate broken machine button and scenery
            if(firstTransition[7])
            {
                ImageSwapTransition(7);
                SceneryToggle(7);
            }
            else
            {
                ImageAndScenery(7);
            }
        }
        if(metalFieldNode)
        {
            // activate metal field X
            mapButton[10].SetActive(true);
            if(firstLoad[10])
            {
                StartCoroutine(SliderToggle(10, 3f));
            }
            else
            {
                mapButton[10].SetActive(true);
            }

            // activate broken machine button and scenery
            if(firstTransition[6])
            {
                ImageSwapTransition(6);
                SceneryToggle(6);
            }
            else
            {
                ImageAndScenery(6);
            }
        }
        if(computerNode)
        {
            computerNodeButton.interactable = true;

            // activate metal field button and scenery
            if(firstTransition[10])
            {
                ImageSwapTransition(10);
                SceneryToggle(10);
            }
            else
            {
                ImageAndScenery(10);
            }
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

            UninteractAll();
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[3].GetComponent<Button>().interactable = true; //gate
        }
        // last at deer
        else if(prevNode == 1)
        {

            //firstLoad[1] = false;

            UninteractAll();
            mapButton[1].GetComponent<Button>().interactable = true; //deer
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[2].GetComponent<Button>().interactable = true; //river
        }
        // last at river
        else if(prevNode == 2)
        {

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

            firstLoad[3] = false;

            UninteractAll();
            mapButton[3].GetComponent<Button>().interactable = true; //gate
            mapButton[0].GetComponent<Button>().interactable = true; //cabin
            mapButton[4].GetComponent<Button>().interactable = true; //river control
        }
        // last at river control
        else if(prevNode == 4)
        {

            firstLoad[4] = false;

            UninteractAll();
            mapButton[4].GetComponent<Button>().interactable = true; //river control
            mapButton[3].GetComponent<Button>().interactable = true; //gate
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
        }
        // last at bear
        else if(prevNode == 5)
        {

            firstLoad[5] = false;

            UninteractAll();
            mapButton[5].GetComponent<Button>().interactable = true; //bear
            mapButton[2].GetComponent<Button>().interactable = true; //river
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
        }
        // last at broken machine
        else if(prevNode == 6)
        {

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

            firstLoad[7] = false;

            UninteractAll();
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
            mapButton[4].GetComponent<Button>().interactable = true; //river control
            mapButton[6].GetComponent<Button>().interactable = true; //broken machine
        }
        // last at bird
        else if(prevNode == 8)
        {

            firstLoad[8] = false;

            UninteractAll();
            mapButton[8].GetComponent<Button>().interactable = true; //bird
            mapButton[2].GetComponent<Button>().interactable = true; //river
        }
        // last at power grid
        else if(prevNode == 9)
        {

            firstLoad[9] = false;

            UninteractAll();
            mapButton[9].GetComponent<Button>().interactable = true; //power grid
            mapButton[7].GetComponent<Button>().interactable = true; //security tower
        }
        // last at metal field
        else if(prevNode == 10)
        {

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
    private IEnumerator SliderToggle(int i, float waitTime)
    {
        if (!firstTransition[PlayerData.prevNode - 1])
        {
            waitTime = 0;
        }

        yield return new WaitForSeconds(waitTime);

        mapButton[i].SetActive(true);

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
        if(mapSlider[i].value >= 0.9f && mapButton[i].GetComponent<Button>().image.sprite == image[i])
        {
            firstTransition[i] = false;
        }

        float lastVal = mapSlider[i].value;

        if(mapButton[i].GetComponent<Button>().image.sprite != image[i])
        {
            if(lastVal > .05)
            {
                mapSlider[i].value = Mathf.Lerp(lastVal, 0f, 1.5f * Time.deltaTime);
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

    // function for fading in the scenery
    private void SceneryToggle(int i)
    {
        // create 2 new colors to lerp from / to
        Color lastColor = mapScenery[i].GetComponent<Image>().color;
        Color newColor = new Color(1f, 1f, 1f, 1f);

        // create the lerped color
        Color lerpedColor = Color.Lerp(lastColor, newColor, 1f * Time.deltaTime);

        // apply the lerped color to game
        mapScenery[i].GetComponent<Image>().color = lerpedColor;
    }

    // function for after a node's completed animation has already been played once before
    private void ImageAndScenery(int i)
    {
        mapButton[i].GetComponent<Button>().image.sprite = image[i];
        mapSlider[i].value = 1f;
        mapScenery[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    private IEnumerator ChangeCircleLocation(int startNode, int endNode, float waitTime)
    {
        circleSlider.fillAmount = 1;
        youAreHereCircle.transform.position = mapButton[startNode].transform.position;
        youAreHereCircle.transform.localScale = circleScales[startNode] * Vector3.one;
        while (circleSlider.fillAmount > 0)
        {
            circleSlider.fillAmount -= circleDrawRate * Time.deltaTime;
            yield return null;
        }
        youAreHereCircle.transform.position = mapButton[endNode].transform.position;
        youAreHereCircle.transform.localScale = circleScales[endNode] * Vector3.one;
        yield return new WaitForSeconds(waitTime);     
        while (circleSlider.fillAmount < 1)
        {
            circleSlider.fillAmount += circleDrawRate * Time.deltaTime;
            yield return null;
        }
        StartCoroutine(LoadNewNode(0.5f));
    }

    private IEnumerator LoadNewNode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        nodeSelector.OpenScene();
    }
}
