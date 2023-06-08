using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTriggerCheck : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player"){
            PlayerData.birdTriggered = true;
        }
    }
}
