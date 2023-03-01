using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleWalls : MonoBehaviour
{
    // Matty
    // script to toggle the walls when walking inside/outside of a building

    public static GameObject walls;

    
    void OnTriggerEnter(Collider other) // Check if Player is inside
    {
        walls.SetActive(false);
        Debug.Log("walls off");
    }

    void OnTriggerExit(Collider other) // Check if Player is outside
    {
        walls.SetActive(true);
        Debug.Log("walls on");
    }

}

