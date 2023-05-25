using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static bool First = true;
    public static bool Second = true;
    public GameObject spawnpoint1;
    public GameObject spawnpoint2;
    public GameObject spawnpoint3;
    // Start is called before the first frame update
    void Awake()
    {
        if (First == true) 
        {
            spawnpoint1.SetActive(true);
            spawnpoint2.SetActive(false);
            if (spawnpoint3 != null){
                spawnpoint3.SetActive(false);
            }
        }
        else if (Second == true)
        {
            spawnpoint1.SetActive(false);
            spawnpoint2.SetActive(true);
            if (spawnpoint3 != null){
                spawnpoint3.SetActive(false);
            }
        }
        else
        {
            spawnpoint1.SetActive(false);
            spawnpoint2.SetActive(false);
            spawnpoint3.SetActive(true);
        }
    }
}
