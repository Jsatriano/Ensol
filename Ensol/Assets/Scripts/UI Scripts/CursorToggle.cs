using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorToggle : MonoBehaviour
{
    public static bool toggleCursor = true;
    public GameObject pauseMenu = null;
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if((scene.name == "SampleScene" || scene.name == "PlaytestingScene") && pauseMenu.activeInHierarchy == false)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
