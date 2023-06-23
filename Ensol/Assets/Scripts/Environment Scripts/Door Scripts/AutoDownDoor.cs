using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDownDoor : MonoBehaviour
{
    public GameObject door;
    public Transform doorRestTarget;
    public Transform doorOpenTarget;
    public float openSpeed = 4f;
    private bool nearDoor = false;


    void Update()
    {

        // if (player is in hitbox)
        //      door moves towards intended position at openSpeed
        // else 
        //      door moves towards rest position
        if (nearDoor == true)
        {
            if (door.transform.position != doorOpenTarget.position)
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, doorOpenTarget.position, openSpeed * Time.deltaTime);
            }
        }

        if (nearDoor == false)
        {
            if (door.transform.position != doorRestTarget.position)
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, doorRestTarget.position, openSpeed * Time.deltaTime);
            }
        }
    }

    //when nearing door
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player"){
            nearDoor = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.envComputerDoor, door.transform.position);
        }
    }

    //when leaving the door
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player"){
            nearDoor = false;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.envComputerUp, door.transform.position);
        }
    }
}
