using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPointStart : MonoBehaviour
{
    public Image blackOutSquare;
    private PauseMenu pauseMenu = null;
    private Coroutine fadeToBlack = null;
    [SerializeField] private GameObject checkpointChoices;
    [SerializeField] private GameObject[] points;
    public static bool[] travelPoints = new bool[] {
        false, true
        // false, false, true, true, true, true,
        // true, true, true, true, false
    };

    public void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen").GetComponent<Image>(); // Gets black out square game object to pass it through scenes
        }
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("UI").GetComponent<PauseMenu>();
        }
        checkpointChoices.SetActive(false);
    }

    // public void Update()
    // {
    //     if(Input.GetButtonDown("Interact")) 
    //     {
    //         fastTravel();
    //     }
    // }

    public void fastTravel()
    {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in inRangeColliders)
        {
            if(collider.gameObject.tag == "CheckPointSTART")
            {
                checkpointChoices.SetActive(true);
                for (int i = 1; i < travelPoints.Length; i++)
                {
                    if (travelPoints[i] == true)
                    {
                        points[i-1].gameObject.SetActive(true);
                    }
                } 
            }
        }
    } 

    public void GoHere(int checkpointIndex)
    {
        if(fadeToBlack == null && SceneManager.GetActiveScene().name != "PlaytestingScene")
        {
            fadeToBlack = StartCoroutine(FadeBlackOutSquare(checkpointIndex)); 
        }
    }

    public IEnumerator FadeBlackOutSquare(int node) // function to slowly fade the screen to black and load map scene
    {
        yield return new WaitForSecondsRealtime(1f);
        Color objectColor = blackOutSquare.color;
        float fadeAmount = 0;
        int fadeSpeed = 1;
        while(blackOutSquare.color.a < 1)
        {      
            fadeAmount += fadeSpeed * Time.unscaledDeltaTime;
            blackOutSquare.color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }
        Cursor.visible = true;
        PlayerData.prevNode = PlayerData.currentNode;
        PlayerData.currentNode = node;
        pauseMenu.OpenMapForNodeTransfer();      
    }
}
