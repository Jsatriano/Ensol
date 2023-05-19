using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NodeSelector : MonoBehaviour
{
    //Elizabeth
    public static int selectedNode;
    public int node;

    public void OpenScene() {
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        selectedNode = node;
        PlayerData.currentNode = node;
        SceneManager.LoadScene("GameplayScene");
    }
}
