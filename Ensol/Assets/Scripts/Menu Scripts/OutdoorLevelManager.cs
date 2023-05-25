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
    public GameObject[] levelLocations;
    public GameObject player;
    // public DataPersistanceManager DataManager;

    void Start()
    {
        node = NodeSelector.selectedNode;
        //DataManager.SaveGame();
        Load(node);
    }

    public void Load(int node) {

        foreach(GameObject point in levelLocations) {
            point.SetActive(false);
        }
        int nodeIndex = node - 1;
        if(nodeIndex >= 0 && nodeIndex < nodePrefabs.Length) {
            levelLocations[nodeIndex].SetActive(true);
            Instantiate(nodePrefabs[nodeIndex], levelLocations[nodeIndex].transform);
        }
        /*set player to that Node's spawn point*/
        spawn_point = GameObject.FindWithTag("Spawnpoint");
        player.transform.position = spawn_point.transform.position;
        player.transform.rotation = spawn_point.transform.rotation;
    }

    public void GoBackToMapSelection() {
        //DataManager.SaveGame();
        SceneManager.LoadScene("MapScene");
    }
}
