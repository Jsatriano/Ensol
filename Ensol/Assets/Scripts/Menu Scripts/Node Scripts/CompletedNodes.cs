using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static int prevNode = 999;

    public static int lNode = 0;

    public static bool cabinNode  = true, deerNode  = true, riverNode  = false, gateNode  = false, riverControlNode  = false,
                bearNode  = false, brokenMachineNode  = false, securityTowerNode  = false, birdNode  = false,
                powerGridNode  = false, metalFieldNode  = false, computerNode = false;

    public static bool[] nodes;            
    public static bool[] firstLoad = new bool[] {
        false, true, true, true, true, true,
        true, true, true, true, false
    };

    public static bool[] completedNodes = new bool[]
    {
        true, false, false, false, false, false,
        false, false, false, false, true
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

        float waitTime = GetWaitTime();

        //UpdateMapIcons();
        DrawMapIcons(PlayerData.prevNode - 1);
        StartCoroutine(ChangeCircleLocation(PlayerData.prevNode-1, PlayerData.currentNode-1, waitTime));
    }

    public void DrawMapIcons(int startNode)
    {
        //Ignores cabin and computer node
        for (int i = 1; i < nodes.Length-1; i++)
        {
            if (nodes[i])
            {
                //Draw X if first time entering node
                if (firstLoad[i])
                {
                    StartCoroutine(SliderToggle(startNode, i, xWaitTime));
                }
                //Draw finished map image + scenery if first time leaving node when it is completed
                else if (firstTransition[i] && completedNodes[i])
                {
                    StartCoroutine(ImageSwapTransition(i));
                    StartCoroutine(SceneryToggle(i));
                }
                //Instantly draw image + scenery if animation has played before
                else if (completedNodes[i])
                {
                    ImageAndScenery(i);
                }
                //Instantly draw X if node hasn't been completed and has been entered before
                else
                {
                    mapButton[i].SetActive(true);
                }
            }
        }
    }

    private float GetWaitTime()
    {
        if (firstTransition[PlayerData.prevNode - 1] && firstLoad[PlayerData.currentNode - 1])
        {
            return circleWaitTime * 7f;
        }
        else if ((!firstTransition[PlayerData.prevNode - 1] && firstLoad[PlayerData.currentNode - 1]) || firstTransition[PlayerData.prevNode - 1] && !firstLoad[PlayerData.currentNode - 1])
        {
            return circleWaitTime * 5f;
        }
        else
        {
            return circleWaitTime;
        }
    }

    // function for activating the X on any node
    private IEnumerator SliderToggle(int startNode, int endNode, float waitTime)
    {
        //No transition time if it is just an x appearing, or if it is the cabin node
        if (!firstTransition[startNode])
        {
            waitTime = 0;
        }
        yield return new WaitForSeconds(waitTime);

        mapButton[endNode].SetActive(true);
        mapSlider[endNode].value = 0;

        //Fades X In
        float interpolator = 0;
        while (mapSlider[endNode].value < 1)
        {
            interpolator += Time.deltaTime * xDrawRate;
            mapSlider[endNode].value = Mathf.Lerp(0, 1, interpolator);
            yield return null;
        }
        firstLoad[endNode] = false;
    }

    // function for swapping the X on any node out for its actual image
    private IEnumerator ImageSwapTransition(int i)
    {
        mapButton[i].SetActive(true);
        //Fades X away
        float interpolator = 1;
        while (mapSlider[i].value > 0)
        {
            interpolator -= Time.deltaTime * xDrawRate;
            mapSlider[i].value = interpolator;
            yield return null;
        }

        //Fade image in
        mapButton[i].GetComponent<Image>().sprite = image[i];
        interpolator = 0;
        while (mapSlider[i].value < 1)
        {
            interpolator += Time.deltaTime * xDrawRate;
            mapSlider[i].value = interpolator;
            yield return null;
        }
        firstTransition[i] = false;
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
        mapButton[i].SetActive(true);
        mapButton[i].GetComponent<Image>().sprite = image[i];
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
