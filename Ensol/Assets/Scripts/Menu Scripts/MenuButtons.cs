using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    public GameObject playButton, controlsButton, exitButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
        Cursor.visible = true;
    }

    public void StartGame()
    {
        // will need to change this to the cabin once its created
        SceneManager.LoadScene(sceneName:"MapScene");
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
