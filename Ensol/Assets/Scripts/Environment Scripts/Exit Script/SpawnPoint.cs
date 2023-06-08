using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static bool First = true;
    public static bool Second = false;
    public static bool Mapped = false;
    public GameObject spawnpoint1 = null;
    public GameObject spawnpoint2 = null;
    public GameObject spawnpoint3 = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (spawnpoint2 == null && spawnpoint3 == null)
        {
            spawnpoint1.SetActive(true);
        }
        else if (First == true) 
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
