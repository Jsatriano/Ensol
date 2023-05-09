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
    // script walls to fade in and out when walking inside/outside of a building

    [SerializeField] GameObject walls;

    GameObject mainCam;
    GameObject cursorCamObj;
    Camera cursorCam;
    CinemachineBrain brain;
    CinemachineVirtualCamera vcam;

    [SerializeField] private float zoomed_out = 6.88f;
    [SerializeField] private float increment = 0.02f;
    [SerializeField] private float zoomed_in = 4.88f;

    private bool inside = false;
    private bool outside = false;
    private bool zoom_now = true;

    // list of mesh renderers in cabin exterior prefab
    //private MeshRenderer[] wallMeshes;

    void Awake () {
        mainCam = GameObject.Find("MainCamera");
        cursorCamObj = GameObject.Find("CursorCamera");
        cursorCam = cursorCamObj.GetComponent<Camera>();
        brain = mainCam.GetComponent<CinemachineBrain>();
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        //wallMeshes = walls.GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (zoom_now && vcam) {
            vcam.m_Lens.OrthographicSize = zoomed_in;
            zoom_now = false;
            cursorCam.orthographicSize = zoomed_in;
        }

        // walls.SetActive(true);
        // Color tempColor;
        // foreach (MeshRenderer obj in wallMeshes) {
        //     if (obj.material.HasProperty("_Color")) {
        //         tempColor = obj.material.color;
        //         tempColor.a = 0.0f;
        //         obj.material.color = tempColor;
        //     }
        // }

        // if player walks inside, zoom in the camera
        if (vcam && vcam.m_Lens.OrthographicSize <= zoomed_out && outside) { 
            float newSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize + increment, 1f);
            vcam.m_Lens.OrthographicSize = newSize;
            cursorCam.orthographicSize = newSize;
        }

        // if player walks outside, zoom out the camera
        if (vcam && vcam.m_Lens.OrthographicSize >= zoomed_in && inside) { 
            float newSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize - increment, 1f);
            vcam.m_Lens.OrthographicSize = newSize;
            cursorCam.orthographicSize = newSize;
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

