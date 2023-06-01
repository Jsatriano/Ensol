using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private float completeRespawnTime;
    public List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        SpawnEnemies();
        PlayerData.timeSinceAtNode[PlayerData.currentNode] = Time.time;
    }

    private void SpawnEnemies()
    {
        List<Transform> allEnemies = new List<Transform>();

        //Creating a list of all enemies, and making them default off
        foreach (Transform enemy in enemiesParent.transform)
        {
            enemy.gameObject.SetActive(false);
            allEnemies.Add(enemy);
        }

        //Calculate how many enemies to spawn based on respawn timer
        int numSpawning = Mathf.FloorToInt(allEnemies.Count * (completeRespawnTime / PlayerData.timeSinceAtNode[PlayerData.currentNode]));

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
