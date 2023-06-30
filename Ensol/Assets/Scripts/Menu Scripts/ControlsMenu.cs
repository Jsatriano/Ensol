using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private GameObject keyboardControls;
    [SerializeField] private GameObject controllerControls;


    private void Update()
    {
        if (CursorToggle.controller)
        {
            controllerControls.SetActive(true);
            keyboardControls.SetActive(false);
        }
        else
        {
            keyboardControls.SetActive(true);
            controllerControls.SetActive(false);
        }
    }
}
