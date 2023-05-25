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

    // public void LoadData(PData data)
    // {
    //     this.NGworked = data.NGworked;
    //     PlayerData.currHP = data.currHP;
    //     PlayerData.currentlyHasBroom = data.currentlyHasBroom;
    //     PlayerData.currentlyHasSolar = data.currentlyHasSolar;
    // }

    // public void SaveData(ref PData data)
    // {
    //     data.NGworked = this.NGworked;
    //     data.currHP = PlayerData.currHP;
    //     data.currentlyHasBroom = PlayerData.currentlyHasBroom;
    //     data.currentlyHasSolar = PlayerData.currentlyHasSolar;
    // }



    void Start() {
        sceneName = SceneManager.GetActiveScene().name;
        
    }

    void Update()
    {

        if(PlayerData.currHP <= 0 && sceneName != "PlaytestingScene") 
        {
            Time.timeScale = 0.5f;
            StartCoroutine(FadeBlackOutSquare());
        }
    }

    public IEnumerator FadeBlackOutSquare()
    {

        yield return new WaitForSeconds(1f);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        float fadeSpeed = 1.1f;

        if(fadeToBlack)
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
