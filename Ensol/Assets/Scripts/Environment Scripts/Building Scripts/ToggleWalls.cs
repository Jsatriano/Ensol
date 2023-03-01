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

    }
    void OnTriggerEnter(Collider other) // Check if Player is inside
    {
        StartCoroutine(FadeWallsOut());
    }

    void OnTriggerExit(Collider other) // Check if Player is outside
    {
        StartCoroutine(FadeWallsIn());
    }

    public IEnumerator FadeWallsOut() // function to slowly fade the walls out
    {
        object.SetActive(false);
        Debug.Log("walls off");
    }

    public IEnumerator FadeWallsIn() // function to slowly fade the walls in
    {
        object.SetActive(true);
        Debug.Log("walls on");
    }
}

