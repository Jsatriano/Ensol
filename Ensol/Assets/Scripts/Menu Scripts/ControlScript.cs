using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    public PlayerController charController;
    public GameObject keyboardControlText;
    public GameObject controllerControlText;

    // Update is called once per frame
    void Update()
    {
        if (CursorToggle.controller)
        {
            controllerControlText.SetActive(true);
            keyboardControlText.SetActive(false);
        }
        else
        {
            keyboardControlText.SetActive(true);
            controllerControlText.SetActive(false);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(sceneName: "MenuScene");
        }
    }
}
