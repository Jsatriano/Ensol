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

    [SerializeField] private float xWaitTime;
    [SerializeField] private float xDrawRate;
    [SerializeField] private float sceneryDrawRate;

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

        //PlayerData.prevNode = 2;
        //PlayerData.currentNode = 3;

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
        

        UpdateMapIcons();
        StartCoroutine(ChangeCircleLocation(PlayerData.prevNode-1, PlayerData.currentNode-1, waitTime));
    }

    public void UpdateMapIcons()
    {
        if (cabinNode)
        {
            mapButton[0].SetActive(true);
            mapScenery[0].SetActive(true);

        }
        if (deerNode)
        {
            // activate deer X
            if (firstLoad[1])
            {
                StartCoroutine(SliderToggle(1, 0f));
            }
            else
            {
                mapButton[1].SetActive(true);
            }
        }
        if (riverNode)
        {
            // activate river X
            if (firstLoad[2])
            {
                StartCoroutine(SliderToggle(2, xWaitTime));
            }
            else
            {
                mapButton[2].SetActive(true);
            }

            // activate deer button and scenery
            if (firstTransition[1])
            {
                StartCoroutine(ImageSwapTransition(1));
                StartCoroutine(SceneryToggle(1));
            }
            else
            {
                ImageAndScenery(1);
            }
        }
        if (gateNode)
        {
            // activate gate X
            if (firstLoad[3])
            {
                StartCoroutine(SliderToggle(3, xWaitTime));
            }
            else
            {
                mapButton[3].SetActive(true);
            }
        }
        if (riverControlNode)
        {
            if (firstLoad[4])
            {
                StartCoroutine(SliderToggle(4, xWaitTime));
            }
            else
            {
                mapButton[4].SetActive(true);
            }

            // activate gate button and scenery
            if (firstTransition[3])
            {
                StartCoroutine(ImageSwapTransition(3));
                StartCoroutine(SceneryToggle(3));
            }
            else
            {
                ImageAndScenery(3);
            }
        }
        if (bearNode)
        {
            if (firstLoad[5])
            {
                StartCoroutine(SliderToggle(5, xWaitTime));
            }
            else
            {
                mapButton[5].SetActive(true);
            }

            if (lastNode != 5 || lastNode != 7)
            {
                lastNode = 5;
            }

            // activate river button and scenery
            if (firstTransition[2])
            {
                StartCoroutine(ImageSwapTransition(2));
                StartCoroutine(SceneryToggle(2));
            }
            else
            {
                ImageAndScenery(2);
            }
        }
        if (brokenMachineNode)
        {
            if (firstLoad[6])
            {
                StartCoroutine(SliderToggle(6, xWaitTime));
            }
            else
            {
                mapButton[6].SetActive(true);
            }

            if (lastNode == 5)
            {

                //activate bear button and scenery
                if (firstTransition[5])
                {
                    StartCoroutine(ImageSwapTransition(5));
                    StartCoroutine(SceneryToggle(5));
                }
                else
                {
                    ImageAndScenery(5);
                }
            }
            if (lastNode == 7)
            {
                //activate security tower button and scenery
                if (firstTransition[7])
                {
                    StartCoroutine(ImageSwapTransition(7));
                    StartCoroutine(SceneryToggle(7));
                }
                else
                {
                    ImageAndScenery(7);
                }
            }
        }
        if (securityTowerNode)
        {
            if (firstLoad[7])
            {
                StartCoroutine(SliderToggle(7, xWaitTime));
            }
            else
            {
                mapButton[7].SetActive(true);
            }

            if (lastNode != 5 && lastNode != 7)
            {
                lastNode = 7;
            }

            //activate river control button and scenery
            if (firstTransition[4])
            {
                StartCoroutine(ImageSwapTransition(4));
                StartCoroutine(SceneryToggle(4));
            }
            else
            {
                ImageAndScenery(4);
            }
        }
        if (birdNode)
        {
            if (firstLoad[8])
            {
                StartCoroutine(SliderToggle(8, xWaitTime));
            }
            else
            {
                mapButton[8].SetActive(true);
            }

            // activate river button and scenery
            if (firstTransition[2])
            {
                StartCoroutine(ImageSwapTransition(2));
                StartCoroutine(SceneryToggle(2));
            }
            else
            {
                ImageAndScenery(2);
            }
        }
        if (powerGridNode)
        {
            if (firstLoad[9])
            {
                StartCoroutine(SliderToggle(9, xWaitTime));
            }
            else
            {
                mapButton[9].SetActive(true);
            }

            // activate broken machine button and scenery
            if (firstTransition[7])
            {
                StartCoroutine(ImageSwapTransition(7));
                StartCoroutine(SceneryToggle(7));
            }
            else
            {
                ImageAndScenery(7);
            }
        }
        if (metalFieldNode)
        {
            if (firstLoad[10])
            {
                StartCoroutine(SliderToggle(10, xWaitTime));
            }
            else
            {
                mapButton[10].SetActive(true);
            }

            // activate broken machine button and scenery
            if (firstTransition[6])
            {
                StartCoroutine(ImageSwapTransition(6));
                StartCoroutine(SceneryToggle(6));
            }
            else
            {
                ImageAndScenery(6);
            }
        }
        if (computerNode)
        {
            computerNodeButton.interactable = true;

            // activate metal field button and scenery
            if (firstTransition[10])
            {
                StartCoroutine(ImageSwapTransition(10));
                StartCoroutine(SceneryToggle(10));
            }
            else
            {
                ImageAndScenery(10);
            }
        }




        // ----------- CHECK WHAT NODE WE ARE AT, AND WHERE WE CAN GO -------------

        // last at cabin
        else if (PlayerData.prevNode == 0)
        {

        }
        // last at deer
        else if (PlayerData.prevNode == 1)
        {

            firstLoad[1] = false;

        }
        // last at river
        else if (PlayerData.prevNode == 2)
        {

            firstLoad[2] = false;

        }
        // last at gate
        else if (PlayerData.prevNode == 3)
        {

            firstLoad[3] = false;

        }
        // last at river control
        else if (PlayerData.prevNode == 4)
        {

            firstLoad[4] = false;

        }
        // last at bear
        else if (PlayerData.prevNode == 5)
        {

            firstLoad[5] = false;
        }
        // last at broken machine
        else if (PlayerData.prevNode == 6)
        {

            firstLoad[6] = false;

        }
        // last at security tower
        else if (PlayerData.prevNode == 7)
        {

            firstLoad[7] = false;

        }
        // last at bird
        else if (PlayerData.prevNode == 8)
        {

            firstLoad[8] = false;

        }
        // last at power grid
        else if (PlayerData.prevNode == 9)
        {

            firstLoad[9] = false;

        }
        // last at metal field
        else if (PlayerData.prevNode == 10)
        {

            firstLoad[10] = false;

        }
        // last at computer
        else if (PlayerData.prevNode == 11)
        {

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

    // function for activating the X on any node
    private IEnumerator SliderToggle(int i, float waitTime)
    {
        if (!firstTransition[PlayerData.prevNode - 1])
        {
            waitTime = 0;
        }
        yield return new WaitForSeconds(waitTime);

        mapButton[i].SetActive(true);
        mapSlider[i].value = 0;

        //Fades X In
        float interpolator = 0;
        while (mapSlider[i].value < 1)
        {
            interpolator += Time.deltaTime * xDrawRate;
            mapSlider[i].value = Mathf.Lerp(0, 1, interpolator);
            yield return null;
        }
    }

    // function for swapping the X on any node out for its actual image
    private IEnumerator ImageSwapTransition(int i)
    {
        //Fades X away
        float interpolator = 1;
        while (mapSlider[i].value > 0)
        {
            interpolator -= Time.deltaTime * xDrawRate;
            mapSlider[i].value = interpolator;
            yield return null;
        }

        //Fade image in
        mapButton[i].GetComponent<Button>().image.sprite = image[i];
        interpolator = 0;
        while (mapSlider[i].value < 1)
        {
            interpolator += Time.deltaTime * xDrawRate;
            mapSlider[i].value = interpolator;
            yield return null;
        } 
    }

    // function for fading in the scenery
    private IEnumerator SceneryToggle(int i)
    {
        Image image = mapScenery[i].GetComponent<Image>();

        // create 2 new colors to lerp from / to
        Color startColor = new Color(0f, 0f, 0f, 0f);
        Color endColor = new Color(1f, 1f, 1f, 1f);

        //Fade the scenery in
        float interpolator = 0;
        image.color = startColor;
        while (image.color.a < endColor.a)
        {
            interpolator += Time.deltaTime * sceneryDrawRate;
            image.color = Color.Lerp(startColor, endColor, interpolator);
            yield return null;
        }
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
        //Position circle on start node
        circleSlider.fillAmount = 1;
        youAreHereCircle.transform.position = mapButton[startNode].transform.position;
        youAreHereCircle.transform.localScale = circleScales[startNode] * Vector3.one;

        //Erase circle
        while (circleSlider.fillAmount > 0)
        {
            circleSlider.fillAmount -= circleDrawRate * Time.deltaTime;
            yield return null;
        }

        //Position circle at end node
        youAreHereCircle.transform.position = mapButton[endNode].transform.position;
        youAreHereCircle.transform.localScale = circleScales[endNode] * Vector3.one;
        yield return new WaitForSeconds(waitTime);     

        //Draw circle
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
