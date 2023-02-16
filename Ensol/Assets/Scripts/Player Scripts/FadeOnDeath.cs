using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOnDeath : MonoBehaviour
{
    public GameObject blackOutSquare;
    public PlayerCombatController pcc;

    void Update()
    {

    if(pcc.currHP <= 0) 
        {
            Time.timeScale = 0.5f;
            StartCoroutine(FadeBlackOutSquare(true));
        }
    }
    public IEnumerator FadeBlackOutSquare(bool dead)
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
                    if(dead)
                    {
                        SceneManager.LoadScene(sceneName:"RecloneScene");
                    }
                    else
                    {
                        SceneManager.LoadScene(sceneName:"MapScene"); 
                    }
                    Cursor.visible = true;

                }
                yield return null;
            }
        }
    }
}
