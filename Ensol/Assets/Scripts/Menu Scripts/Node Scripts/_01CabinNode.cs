using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _01CabinNode : MonoBehaviour
{
    public GameObject gateNodeMap;
    public CompletedNodes completedNodes;

    public void CompletedLevel()
    {
        print("completed cabin node");
        completedNodes.gateNode = true;
    }
}
