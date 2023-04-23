using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public int soundDoor;
    public float openRot, speed;
    private bool opened = false;
    [HideInInspector] public bool opening = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        if(opening)
        {
            if(currentRot.y < openRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openRot, currentRot.z), speed * Time.deltaTime);
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
