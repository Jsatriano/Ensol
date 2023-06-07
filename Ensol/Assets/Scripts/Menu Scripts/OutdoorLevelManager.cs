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
    public static bool isCheckpointTransition;

    void Start()
    {
        node = NodeSelector.selectedNode;
        print("Selected node is " + node);
        Load(node);
    }

    public void Load(int node) {
        foreach(GameObject point in levelLocations) {
            point.SetActive(false);
        }
        int nodeIndex = node - 1;
        print("nodeIndex is " + nodeIndex);
        if(nodeIndex >= 0 && nodeIndex < nodePrefabs.Length) {
            levelLocations[nodeIndex].SetActive(true);
            Instantiate(nodePrefabs[nodeIndex], levelLocations[nodeIndex].transform);
        }
        /*set player to that Node's spawn point*/
        if(isCheckpointTransition) {
            spawn_point = GameObject.FindWithTag("CheckpointSpawnpoint");
            isCheckpointTransition = false;
            if(PlayerData.currentNode == 1) {
                PlayerData.prevNode = 1;
            }
            else if(PlayerData.currentNode == 6) {
                PlayerData.prevNode = 5;
            }
            else if(PlayerData.currentNode == 10) {
                PlayerData.prevNode = 9;
            }
            else if(PlayerData.currentNode == 12) {
                PlayerData.prevNode = 11;
            }
        }
        else{
            spawn_point = GameObject.FindWithTag("Spawnpoint");
        }
        player.transform.position = spawn_point.transform.position;
        player.transform.rotation = spawn_point.transform.rotation;
    }

    public void GoBackToMapSelection() {
        SceneManager.LoadScene("MapScene");
    }
}
