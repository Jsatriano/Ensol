using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOnDeath : MonoBehaviour
{
    public GameObject blackOutSquare;
    public NodeSelector nodeSelector;
    private string sceneName;
    private Coroutine fadeRoutine = null;



    void Start() {
        sceneName = SceneManager.GetActiveScene().name;
        
    }

    void Update()
    {

        if(fadeRoutine == null && PlayerData.currHP <= 0 && sceneName != "PlaytestingScene") 
        {
            Time.timeScale = 0.5f;
            fadeRoutine = StartCoroutine(FadeBlackOutSquare());
        }
    }

    public IEnumerator FadeBlackOutSquare()
    {
        yield return new WaitForSeconds(0.65f);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        float fadeSpeed = 1.6f;

        if (fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a >= 1)
                {
                    //Takes player straight back to cabin instead of respawn screen if they haven't gotten the broom yet (crack deer death)
                    if (PlayerData.hasBroom)
                    {
                        SceneManager.LoadScene(sceneName:"RecloneScene");
                        Cursor.visible = true;
                    }
                    else
                    {
                        PlayerData.diedToCrackDeer = true;
                        nodeSelector.node = 1;
                        Time.timeScale = 1f;
                        PlayerData.currHP = -1;
                        nodeSelector.OpenScene();
                    }
                }
                yield return null;
            }
        }
    }

}
