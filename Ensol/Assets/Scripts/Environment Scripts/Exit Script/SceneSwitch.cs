using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    // Harsha
    public Image blackOutSquare;
    public bool Entrance = true;
    public static bool exitFrom = true;
    [SerializeField] private int nodeDestination;
    private PauseMenu pauseMenu = null;
    private Coroutine fadeToBlack = null;

    void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen").GetComponent<Image>(); // Gets black out square game object to pass it through scenes
        }
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("UI").GetComponent<PauseMenu>();
        }
    }
    void OnTriggerEnter(Collider other) // Check if Player has reached exit area
    {
        if (other.gameObject.tag == "Player")
        {
            if (Entrance == true) 
            {
                exitFrom = true;
            }
            else 
            {
                exitFrom = false;
            }

            if(fadeToBlack == null && SceneManager.GetActiveScene().name != "PlaytestingScene"){
                fadeToBlack = StartCoroutine(FadeBlackOutSquare()); 
            }
        }
    }

    public IEnumerator FadeBlackOutSquare() // function to slowly fade the screen to black and load map scene
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
        PlayerData.currentNode = nodeDestination;
        pauseMenu.OpenMapForNodeTransfer();      
    }
}
