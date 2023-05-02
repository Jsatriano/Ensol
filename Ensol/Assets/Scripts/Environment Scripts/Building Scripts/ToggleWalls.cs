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

    private float zoomed_out = 6.88f;
    private float increment = 0.02f;
    private float zoomed_in = 4.88f;

    private bool inside = false;
    private bool outside = false;

    void Awake () {
        mainCam = GameObject.Find("MainCamera");
        brain = mainCam.GetComponent<CinemachineBrain>();
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }
 
    void Update()
    {
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;

        // if player walks inside, zoom in the camera
        if (vcam && vcam.m_Lens.OrthographicSize <= zoomed_out && outside) { 
            vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize + increment, 1f); 
        }

        // if player walks outside, zoom out the camera
        if (vcam && vcam.m_Lens.OrthographicSize >= zoomed_in && inside) { 
            vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize - increment, 1f); 
        }

    }
    
    void OnTriggerEnter(Collider other) // Check if Player is inside
    {
        walls.SetActive(false);
        inside = true;
        outside = false;
        Debug.Log("walls off");
    }

    void OnTriggerExit(Collider other) // Check if Player is outside
    {
        walls.SetActive(true);
        inside = false;
        outside = true;
        Debug.Log("walls on");
    }
}

