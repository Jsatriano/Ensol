using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTriggerCheck : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        PlayerData.birdTriggered = true;
    }
}
