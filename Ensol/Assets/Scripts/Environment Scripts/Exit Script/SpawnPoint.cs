using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static bool First = true;
    public GameObject spawnpoint1;
    public GameObject spawnpoint2;
    // Start is called before the first frame update
    void Awake()
    {
        print("first is set to ");
        print(First);
        if (First == true) {
            spawnpoint1.SetActive(true);
            spawnpoint2.SetActive(false);
            print("entrance spawn point is on");
        }
        else 
        {
            spawnpoint1.SetActive(false);
            spawnpoint2.SetActive(true);
            print("exit spawn point is on");
        }
    }
}
