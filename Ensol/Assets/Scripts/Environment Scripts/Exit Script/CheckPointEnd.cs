using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPointEnd : MonoBehaviour
{
    // Harsha
    public Image blackOutSquare;
    private PauseMenu pauseMenu = null;
    private Coroutine fadeToBlack = null;

    public void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen").GetComponent<Image>(); // Gets black out square game object to pass it through scenes
        }
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("UI").GetComponent<PauseMenu>();
        }
    }

    public void Update()
    {
        if(Input.GetButtonDown("Interact")) 
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in inRangeColliders)
        {
            if(collider.gameObject.tag == "CheckPointEND")
            {
                if(fadeToBlack == null && SceneManager.GetActiveScene().name != "PlaytestingScene")
                {
                    fadeToBlack = StartCoroutine(FadeBlackOutSquare()); 
                }
            }
        }
    } 

    // void OnTriggerEnter(Collider other) // Check if Player has reached exit area
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         if(fadeToBlack == null && SceneManager.GetActiveScene().name != "PlaytestingScene"){
    //             fadeToBlack = StartCoroutine(FadeBlackOutSquare()); 
    //         }
    //     }
    // }

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
        PlayerData.currentNode = 1;
        pauseMenu.OpenMapForNodeTransfer();      
    }
}
