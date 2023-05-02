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

    void Start() {
        keyboardControlText.SetActive(true);
        controllerControlText.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
       /* if(charController.controller)
        {
            keyboardControlText.SetActive(false);
            controllerControlText.SetActive(true);
        }
        else
        {
            keyboardControlText.SetActive(true);
            controllerControlText.SetActive(false);
        }*/

        if(Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(sceneName:"MenuScene");
        }
        
    }

}
