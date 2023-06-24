using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorToggle : MonoBehaviour
{
    public static bool toggleCursor = true;
    public GameObject pauseMenu = null;
    public GameObject map = null;
    public GameObject checkpointUI = null;
    public GameObject arrow = null;
    public GameObject dialoguebox = null;
    private Scene scene;

    // public static float horizontal;
    // public static float vertical;
    public static bool controller = false;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "GameplayScene" || scene.name == "PlaytestingScene"){
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        // checks if Mouse is moving
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && (Input.GetAxis("Horizontal_Controller") == 0 || Input.GetAxis("Vertical_Controller") == 0))
        {
            // horizontal = Input.GetAxisRaw("Horizontal");
            // vertical = Input.GetAxisRaw("Vertical");
            controller = false;
        }
        // checks if Controller moving
        else if ((Input.GetAxis("Horizontal_Controller") != 0 || Input.GetAxis("Vertical_Controller") != 0) && (Input.GetAxis("Horizontal") == 0 || Input.GetAxis("Vertical") == 0))
        {
            // print("controller is moving !!!!!!!!!!!!");
            // horizontal = Input.GetAxisRaw("Horizontal_Controller");
            // vertical = Input.GetAxisRaw("Vertical_Controller");
            controller = true;
        }
        
        if (scene.name == "CreditScene"){
            Cursor.visible = false;
        }
         else if(((scene.name == "GameplayScene" && !dialoguebox.activeInHierarchy) || scene.name == "PlaytestingScene") && !pauseMenu.activeInHierarchy && !map.activeInHierarchy && !checkpointUI.activeInHierarchy)
        {
            Cursor.visible = false;
            if ((scene.name == "GameplayScene" && PlayerData.currentlyHasBroom && PlayerData.currentNode != 12) || scene.name == "PlaytestingScene"){
                arrow.SetActive(true);
            } else {
                arrow.SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            if (scene.name == "GameplayScene" || scene.name == "PlaytestingScene"){
                arrow.SetActive(false);
            }
            Cursor.visible = true;
        }
    }
}
