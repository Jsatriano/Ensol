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
        arrow.SetActive(false);
    }

    void Update()
    {
        if((scene.name == "SampleScene" || scene.name == "PlaytestingScene") && pauseMenu.activeInHierarchy == false && dialoguebox.activeInHierarchy == false)
        {
            Cursor.visible = false;
            if (arrow != null && PlayerData.hasBroom){
                arrow.SetActive(true);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            if (arrow != null){
                arrow.SetActive(false);
            }
            Cursor.visible = true;
        }
    }
}
