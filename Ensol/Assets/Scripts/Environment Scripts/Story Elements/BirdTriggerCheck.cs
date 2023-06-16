using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BirdTriggerCheck : MonoBehaviour
{
    public UnityEvent<Collider> birdOnTriggerEnter;

    public void OnTriggerEnter(Collider col)
    {
        if (birdOnTriggerEnter != null){
                birdOnTriggerEnter.Invoke(col);
        }

        if (col.tag == "Player"){
            PlayerData.birdTriggered = true;
        }
    }
}
