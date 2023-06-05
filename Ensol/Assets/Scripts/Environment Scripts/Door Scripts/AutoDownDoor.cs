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



    void Start()
    {

    }


    void Update()
    {

        // if (player is colliding with hitbox)
        //      door moves towards intended position at openSpeed
        // else 
        //      door moves towards rest position
        if (nearDoor == true)
        {
            if (door.transform.position != doorOpenTarget.position)
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, doorOpenTarget.position, openSpeed * Time.deltaTime);
                Debug.Log("OPENING");
            }
        }

        if (nearDoor == false)
        {
            if (door.transform.position != doorRestTarget.position)
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, doorRestTarget.position, openSpeed * Time.deltaTime);
                Debug.Log("CLOSING");
            }
        }
    }


    void OnTriggerEnter(Collider doorRadius)
    {
        nearDoor = true;
        Debug.Log("opening door");
    }

    void OnTriggerExit(Collider doorRadius)
    {
        nearDoor = false;
        Debug.Log("closing door");
    }
}
