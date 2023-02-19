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
    [HideInInspector] public GameObject spawn_point;

    public Object[] nodePrefabs;
    public GameObject[] grassLayouts;
    public GameObject player;

    void Start()
    {
        node = NodeSelector.selectedNode;
        Load(node);
    }

    public void Load(int node) {

        foreach(GameObject grass in grassLayouts) {
            grass.SetActive(false);
        }
        int nodeIndex = node - 1;
        if(nodeIndex >= 0 && nodeIndex < nodePrefabs.Length) {
            Instantiate(nodePrefabs[nodeIndex]);
            grassLayouts[nodeIndex].SetActive(true);
        }
        /*set player to that Node's spawn point*/
        spawn_point = GameObject.FindWithTag("Spawnpoint");
        player.transform.position = spawn_point.transform.position;
        player.transform.rotation = spawn_point.transform.rotation;
    }

    public void GoBackToMapSelection() {
        SceneManager.LoadScene("MapScene");
    }
}
