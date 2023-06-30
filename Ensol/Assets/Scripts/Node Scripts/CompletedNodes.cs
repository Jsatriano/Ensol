using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedNodes : MonoBehaviour
{
    public static int prevNode = 999;

    public static bool cabinNode  = true, deerNode  = true, riverNode  = false, gateNode  = false, riverControlNode  = false,
                bearNode  = false, brokenMachineNode  = false, securityTowerNode  = false, birdNode  = false,
                powerGridNode  = false, metalFieldNode  = false, computerNode = false, computerInterior = false;

    public static bool[] nodes;            
    public static bool[] firstLoad = new bool[] {
        false, false, true, true, true, true,
        true, true, true, true, true, false, false
    };

    public static bool[] completedNodes = new bool[]
    {
        true, false, false, false, false, false,
        false, false, false, false, false, true, true
    };

    public static bool[] firstTransition = new bool[] {
        false, true, true, true, true,
        true, true, true, true, true, true, false, false
    };

    public static bool[] checkpoints = new bool[4];

    [Header("References")]
    public Sprite[] image;
    public GameObject[] mapButton;
    public GameObject[] mapScenery;
    public Slider[] mapSlider;
    [SerializeField] private Button cabinButton;
    [SerializeField] private GameObject homeText;
    public GameObject keyboardText;
    public GameObject controllerText;
    [SerializeField] private Image blackOutSquare;
    public PauseMenu pauseMenu;

    [Header("Circle Data")]
    public GameObject youAreHereCircle;
    [SerializeField] private float circleDrawRate;
    [SerializeField] private Image circleSlider;
    [SerializeField] private List<float> circleScales;
    [SerializeField] private float circleWaitTime;

    [Header("Map Draw Rates")]
    [SerializeField] private float xWaitTime;
    [SerializeField] private float xDrawRate;
    [SerializeField] private float sceneryDrawRate;
    [SerializeField] private NodeSelector nodeSelector; 

    private PlayerInputActions playerInputActions;

    private void Start(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

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
    12 = computer interior
    */

    private void Update(){
        if (CursorToggle.controller){
            homeText = controllerText;
        } else {
            homeText = keyboardText;
        }

        if (homeText.activeInHierarchy && playerInputActions.Player.CabinReturn.triggered){
            pauseMenu.ReturnToCabin();
        }
    }

    //Function called for just looking at the map
    public void LookAtMap()
    {
        blackOutSquare.enabled = false;
        if (PlayerData.currentNode != 1 && PlayerData.currentNode != 13)
        {
            homeText.SetActive(true);
            cabinButton.interactable = true;
        }
        else
        {
            homeText.SetActive(false);
            cabinButton.interactable = false;
        }
        PreDraw();
        PlaceCircle(PlayerData.currentNode - 1);
        //play open map sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.hudMapOpen, this.transform.position);
    }

    //Function called when travelling between nodes to play circle anim and called the nodeSelector
    public void NodeTransferMap()
    {
        blackOutSquare.enabled = false;
        homeText.SetActive(false);
        cabinButton.interactable = false;
        PreDraw();
        StartCoroutine(ChangeCircleLocation(PlayerData.prevNode - 1, PlayerData.currentNode - 1, GetWaitTime()));
    }

    private void PreDraw()
    {
        nodes = new bool[13];
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
        nodes[12] = computerInterior;

        //UpdateMapIcons();
        DrawMapIcons(PlayerData.prevNode - 1);
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
        yield return new WaitForSecondsRealtime(waitTime);

        mapButton[endNode].SetActive(true);
        mapSlider[endNode].value = 0;

        //Fades X In
        float interpolator = 0;
        while (mapSlider[endNode].value < 1)
        {
            interpolator += Time.unscaledDeltaTime * xDrawRate;
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
            interpolator -= Time.unscaledDeltaTime * xDrawRate;
            mapSlider[i].value = interpolator;
            yield return null;
        }

        //Fade image in
        mapButton[i].GetComponent<Image>().sprite = image[i];
        interpolator = 0;
        while (mapSlider[i].value < 1)
        {
            interpolator += Time.unscaledDeltaTime * xDrawRate;
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
            interpolator += Time.unscaledDeltaTime * sceneryDrawRate;
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
        print(endNode);
        //Position circle on start node
        circleSlider.fillAmount = 1;
        youAreHereCircle.transform.position = mapButton[startNode].transform.position;
        youAreHereCircle.transform.localScale = circleScales[startNode] * Vector3.one;

        //Erase circle
        if (circleSlider.fillAmount > 0)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.hudMapErase, this.transform.position);
        }
        while (circleSlider.fillAmount > 0)
        {
            circleSlider.fillAmount -= circleDrawRate * Time.unscaledDeltaTime;
            yield return null;
        }


        //Position circle at end node
        youAreHereCircle.transform.position = mapButton[endNode].transform.position;
        youAreHereCircle.transform.localScale = circleScales[endNode] * Vector3.one;
        yield return new WaitForSecondsRealtime(waitTime);

        //Draw circle
        if (circleSlider.fillAmount < 1)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.hudMapDraw, this.transform.position);
        }
        while (circleSlider.fillAmount < 1)
        {
            circleSlider.fillAmount += circleDrawRate * Time.unscaledDeltaTime;
            yield return null;
        }

        StartCoroutine(LoadNewNode(0.5f));
    }

    private void PlaceCircle(int node)
    {
        youAreHereCircle.transform.position = mapButton[node].transform.position;
        youAreHereCircle.transform.localScale = circleScales[node] * Vector3.one;
        circleSlider.fillAmount = 1;
    }

    public void LoadNode()
    {
        nodeSelector.OpenScene();
    }

    private IEnumerator LoadNewNode(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        StartCoroutine(FadeOutAndTransfer()); 
    }

    private IEnumerator FadeOutAndTransfer()
    {
        blackOutSquare.enabled = true;
        Color objectColor = blackOutSquare.color;
        blackOutSquare.color = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
        float fadeAmount;
        float fadeSpeed = 2f;

        while (blackOutSquare.color.a < 1)
        {
            fadeAmount = blackOutSquare.color.a + (fadeSpeed * Time.unscaledDeltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.color = objectColor;
            yield return null;
        }
        nodeSelector.OpenScene();
    }
}
