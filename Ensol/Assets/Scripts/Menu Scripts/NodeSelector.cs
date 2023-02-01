using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NodeSelector : MonoBehaviour
{
    //Elizabeth
    
    public static int selectedNode;

    public int node;
    public TextMeshProUGUI nodeText;
    // Start is called before the first frame update
    void Start()
    {
        nodeText.text = "Node " + node.ToString();
    }

    public void OpenScene() {
        selectedNode = node;
        SceneManager.LoadScene("SampleScene");
    }
}
