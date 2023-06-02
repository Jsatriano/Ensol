using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float completeRespawnTime;
    [HideInInspector] public List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        //Is set to -1 when its the player's first time entering a node during a life (all are reset to -1 when the player dies)
        if (PlayerData.timeSinceAtNode[PlayerData.currentNode] == -1)
        {
            PlayerData.timeSinceAtNode[PlayerData.currentNode] = Time.time;
            SpawnEnemies(1);
        }
        else
        {
            PlayerData.timeSinceAtNode[PlayerData.currentNode] = Time.time - PlayerData.timeSinceAtNode[PlayerData.currentNode];
            SpawnEnemies(Mathf.Clamp01(PlayerData.timeSinceAtNode[PlayerData.currentNode] / completeRespawnTime));
        }       
    }

    private void SpawnEnemies(float percentToSpawn)
    {
        Debug.Log(percentToSpawn);
        List<Transform> allEnemies = new List<Transform>();

        //Creating a list of all enemies, and making them default off
        foreach (Transform enemy in transform)
        {
            enemy.gameObject.SetActive(false);
            allEnemies.Add(enemy);
        }

        //Calculate how many enemies to spawn based on respawn timer
        int numSpawning = Mathf.FloorToInt(allEnemies.Count * percentToSpawn);

        //Set active randomly selected enemies and fill the public list with them for the gate controller to access
        allEnemies = ShuffleList(allEnemies);
        while (numSpawning > 0)
        {
            allEnemies[numSpawning - 1].gameObject.SetActive(true);
            aliveEnemies.Add(allEnemies[numSpawning - 1].gameObject);
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
