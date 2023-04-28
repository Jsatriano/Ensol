using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class ToggleWalls : MonoBehaviour
{
    // Matty
    // script to toggle the walls when walking inside/outside of a building

    // Autumn
    // script Camera to Zoom in and out when walking inside/outside of a building

    public GameObject walls;

    GameObject mainCam;
    CinemachineBrain brain;
    CinemachineVirtualCamera vcam;

    void Awake () {
        mainCam = GameObject.Find("MainCamera");
        brain = mainCam.GetComponent<CinemachineBrain>();
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }
 
    void Update()
    {
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }
    
    void OnTriggerEnter(Collider other) // Check if Player is inside
    {
        walls.SetActive(false);
        if (vcam) {
            vcam.m_Lens.OrthographicSize = 4.66f;
        }
        Debug.Log("walls off");
    }

    void OnTriggerExit(Collider other) // Check if Player is outside
    {
        walls.SetActive(true);
        if (vcam) {
            vcam.m_Lens.OrthographicSize = 6.66f;
        }
        Debug.Log("walls on");
    }

}

