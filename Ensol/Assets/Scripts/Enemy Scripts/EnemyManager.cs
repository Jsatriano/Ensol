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
        List<Transform> deadEnemies = new List<Transform>();
        List<GameObject> prevAliveEnemies = new List<GameObject>();

        //Creating a list of all enemies, and making them default off
        foreach (Transform enemy in transform)
        {            
            if (PlayerData.enemiesAliveInNode[PlayerData.currentNode-1].Contains(enemy.name))
            {
                prevAliveEnemies.Add(enemy.gameObject);
                enemy.gameObject.SetActive(true);
                aliveEnemies.Add(enemy.GetComponent<BT>());
            }
            else
            {
                enemy.gameObject.SetActive(false);
                deadEnemies.Add(enemy);
            }
        }

        int numSpawning = Mathf.Clamp(Mathf.FloorToInt((deadEnemies.Count + prevAliveEnemies.Count) * percentToSpawn), 0, deadEnemies.Count);

        if (PlayerData.currentNode == 2 && (PlayerData.enemiesAliveInNode[PlayerData.currentNode - 1].Contains("Crack Deer Variant(Clone)")) && aliveEnemies.Count == 0) 
        {
            numSpawning = 1;
        }

        //Debug.Log("Percent Spawn: " + Mathf.FloorToInt(allEnemies.Count * percentToSpawn));
        //Debug.Log("Previously Alive: " + PlayerData.enemiesAliveInNode[PlayerData.currentNode]);
        //Calculate how many enemies to spawn based on respawn timer

        

        //Set active randomly selected enemies and fill the public list with them for the gate controller to access
        deadEnemies = ShuffleList(deadEnemies);
        while (numSpawning > 0)
        {
            deadEnemies[numSpawning - 1].gameObject.SetActive(true);
            aliveEnemies.Add(deadEnemies[numSpawning - 1].GetComponent<BT>());
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
