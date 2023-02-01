using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RecloneScreen : MonoBehaviour
{
    public GameObject recloneButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(recloneButton);
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        // will need to change this to the cabin once its created
        SceneManager.LoadScene(sceneName:"SampleScene");
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName:"MenuScene");
    }
}
