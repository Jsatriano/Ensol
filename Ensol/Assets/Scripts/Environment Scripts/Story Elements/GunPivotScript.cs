using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPivotScript : MonoBehaviour
{
    [HideInInspector] public bool foundPlayer = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(foundPlayer)
        {
            transform.LookAt(player.transform);
        }
    }
}
