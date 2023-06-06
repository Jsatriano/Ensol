using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float completeRespawnTime;
    [HideInInspector] public List<BT> aliveEnemies = new List<BT>();

    private void Start()
    {
        //Is set to -1 when its the player's first time entering a node, spawns all enemies
        if (PlayerData.timeSinceAtNode[PlayerData.currentNode-1] == -1)
        {
            SpawnEnemies(1);
        }
        else
        {
            //Debug.Log("Time Passed: " + (Time.time - PlayerData.timeSinceAtNode[PlayerData.currentNode]));
            SpawnEnemies(Mathf.Clamp01((Time.time - PlayerData.timeSinceAtNode[PlayerData.currentNode-1]) / completeRespawnTime));
        }       
    }

    private void SpawnEnemies(float percentToSpawn)
    {
        //Debug.Log(percentToSpawn);
        List<Transform> allEnemies = new List<Transform>();

        //Creating a list of all enemies, and making them default off
        foreach (Transform enemy in transform)
        {
            enemy.gameObject.SetActive(false);
            allEnemies.Add(enemy);
        }

        //Debug.Log("Percent Spawn: " + Mathf.FloorToInt(allEnemies.Count * percentToSpawn));
        //Debug.Log("Previously Alive: " + PlayerData.enemiesAliveInNode[PlayerData.currentNode]);
        //Calculate how many enemies to spawn based on respawn timer
        int numSpawning = Mathf.Clamp(Mathf.FloorToInt(allEnemies.Count * percentToSpawn) + PlayerData.enemiesAliveInNode[PlayerData.currentNode-1], 0, allEnemies.Count);

        //Set active randomly selected enemies and fill the public list with them for the gate controller to access
        allEnemies = ShuffleList(allEnemies);
        while (numSpawning > 0)
        {
            allEnemies[numSpawning - 1].gameObject.SetActive(true);
            aliveEnemies.Add(allEnemies[numSpawning - 1].GetComponent<BT>());
            numSpawning--;
        }
    }

    private List<Transform> ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randIndex = Random.Range(i, list.Count);
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
        return list;
    }
}
