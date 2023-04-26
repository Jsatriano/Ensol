using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public int soundDoor;
    private bool opened = false;
    [HideInInspector] public bool opening = false;
    


    // Update is called once per frame
    void Update()
    {
        //Vector3 currentRot = door.transform.localEulerAngles;
        if(opening)
        {
            if(door.transform.localEulerAngles.y  > 270 || door.transform.localEulerAngles.y == 0)
            {
                door.transform.rotation *= Quaternion.AngleAxis(-(90 * Time.deltaTime), Vector3.up);           
            }
        }
        
    }

    public void OpenDoor()
    {
        if (opened == false)
        {
            if (soundDoor == 1)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.envCabinDoorOpen, this.transform.position);
            }
            else if (soundDoor == 2)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.envFenceGateOpen, this.transform.position);
            }
            opened = true;
        }
        opening = true;
    }
}
