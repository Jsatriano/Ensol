using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorToggle : MonoBehaviour
{
    public static bool toggleCursor = true;
    public GameObject pauseMenu = null;
    public GameObject arrow = null;
    public GameObject dialoguebox = null;
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "SampleScene" || scene.name == "PlaytestingScene"){
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        if((scene.name == "SampleScene" && !dialoguebox.activeInHierarchy) || scene.name == "PlaytestingScene" && !pauseMenu.activeInHierarchy)
        {
            Cursor.visible = false;
            if ((scene.name == "SampleScene" && PlayerData.currentlyHasBroom) || scene.name == "PlaytestingScene"){
                arrow.SetActive(true);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            if (scene.name == "SampleScene" || scene.name == "PlaytestingScene"){
                arrow.SetActive(false);
            }
            Cursor.visible = true;
        }
    }
}
