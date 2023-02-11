using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public float openRot, speed;
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
        opening = true;
    }
}
