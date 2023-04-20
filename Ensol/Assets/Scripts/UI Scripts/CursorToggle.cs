using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorToggle : MonoBehaviour
{
    public static bool toggleCursor = true;
    public CharController charController = null;
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if((scene.name == "SampleScene" || scene.name == "PlaytestingScene") && charController.state != CharController.State.PAUSED)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }
}
