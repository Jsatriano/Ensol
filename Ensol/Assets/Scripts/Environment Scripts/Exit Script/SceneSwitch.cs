using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    // Harsha
    public static GameObject blackOutSquare {get; private set;}
    public bool Entrance = true;
    public static bool exitFrom = true;

    void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen"); // Gets black out square game object to pass it through scenes
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

            if(SceneManager.GetActiveScene().name != "PlaytestingScene"){
                StartCoroutine(FadeBlackOutSquare()); 
            }
        }
    }

    public IEnumerator FadeBlackOutSquare() // function to slowly fade the screen to black and load map scene
    {
        yield return new WaitForSeconds(1f);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        int fadeSpeed = 1;

        if(fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a >= 1)
                {
                    Cursor.visible = true;
                    SceneManager.LoadScene(sceneName:"MapScene");

                }
                yield return null;
            }
        }
    }
}
