using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutdoorLevelManager : MonoBehaviour 
{
    /*Create level layouts (including floor, all props/objects, and all enemies but NOT including 
    player for now because it fucks up the camera follow targets) and place them in the array from this script.
    Depending on the node selected in the MapScene, it will load that prefab so the player can play the level.
    -Elizabeth
    */
    [HideInInspector] public int node;

    public Object[] nodePrefabs;

    void Start()
    {
        node = NodeSelector.selectedNode;
        Load(node);
    }

    public void Load(int node) {
        int nodeIndex = node - 1;
        if(nodeIndex >= 0 && nodeIndex < nodePrefabs.Length) {
            Instantiate(nodePrefabs[nodeIndex]);
        }
    }

    public void GoBackToMapSelection() {
        SceneManager.LoadScene("MapScene");
    }
}
