using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToggleWalls : MonoBehaviour
{
    // Matty
    // script to toggle the walls when walking inside/outside of a building
    void Start()
    {
        var object : GameObject;
    }
    void OnTriggerEnter(Collider other) // Check if Player is inside
    {
        FadeWallsOut();
    }

    void OnTriggerExit(Collider other) // Check if Player is outside
    {
        FadeWallsIn();
    }

    public FadeWallsOut() // function to slowly fade the walls out
    {
        object.SetActive(false);
        Debug.Log("walls off");
    }

    public FadeWallsIn() // function to slowly fade the walls in
    {
        object.SetActive(true);
        Debug.Log("walls on");
    }
}

