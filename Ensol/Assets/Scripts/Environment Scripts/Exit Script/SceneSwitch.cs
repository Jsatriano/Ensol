using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // Harsha
    public static GameObject blackOutSquare {get; private set;}
    void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen"); // Gets black out square game object to pass it through scenes
        }
    }
    void OnTriggerEnter(Collider other) // Check if Player has reached exit area
    {
        StartCoroutine(FadeBlackOutSquare()); 
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
                    SceneManager.LoadScene(sceneName:"MapScene"); 
                    Cursor.visible = true;

                }
                yield return null;
            }
        }
    }
}
