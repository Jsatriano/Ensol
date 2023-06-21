using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BehaviorTree;

public class SceneSwitch : MonoBehaviour
{
    // Harsha
    public Image blackOutSquare;
    public bool Entrance = true;
    public static bool exitFrom = true;
    [SerializeField] private int nodeDestination;
    private PauseMenu pauseMenu = null;
    private Coroutine fadeToBlack = null;
    [SerializeField] private Transform enemyParent = null;

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
        float fadeSpeed = 1f;
        if(nodeDestination == 13) {
            fadeSpeed = 3f;
        }
        while(blackOutSquare.color.a < 1)
        {      
            fadeAmount += fadeSpeed * Time.unscaledDeltaTime;
            blackOutSquare.color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }
        Cursor.visible = true;
        SetEnemiesDefeated();
        SetTimeAtNode();
        PlayerData.prevNode = PlayerData.currentNode;
        PlayerData.currentNode = nodeDestination;   
        //no map cutscene for entering computer interior
        if (nodeDestination == 13)
        {
            pauseMenu.InstantlyTransferNode();
        }
        else
        {
            pauseMenu.OpenMapForNodeTransfer();      
        }
    }

    public void SetTimeAtNode()
    {
        if (PlayerData.timeSinceAtNode[PlayerData.currentNode] == -1)
        {
            PlayerData.timeSinceAtNode[PlayerData.currentNode-1] = Time.time;
        }
        else
        {
            PlayerData.timeSinceAtNode[PlayerData.currentNode-1] = Time.time;
        }
    }

    public void SetEnemiesDefeated()
    {
        if (!enemyParent)
        {
            return;
        }

        List<string> aliveEnemies = new List<string>();
        foreach (Transform enemy in enemyParent)
        {
            if (enemy.gameObject.activeSelf && enemy.GetComponent<BT>().isAlive)
            {
                aliveEnemies.Add(enemy.name);
            }
        }
        PlayerData.enemiesAliveInNode[PlayerData.currentNode-1] = aliveEnemies;
    }
}
