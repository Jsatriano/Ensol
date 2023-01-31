using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        // will need to change this to the cabin once its created
        SceneManager.LoadScene(sceneName:"SampleScene");
    }

    public void Controls()
    {
        SceneManager.LoadScene(sceneName:"ControlScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
