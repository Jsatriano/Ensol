using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitch : MonoBehaviour
{
    public static int selectedNode;

    public int node;
    void OnTriggerEnter(Collider other)
    {
        selectedNode = node;
        SceneManager.LoadScene(sceneName:"MapScene");
    }
}
