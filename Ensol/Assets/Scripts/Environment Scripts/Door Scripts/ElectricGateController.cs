using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGateController : MonoBehaviour
{
    public GameObject[] gates;
    public float openRot, speed;
    [HideInInspector] public bool opening = false;

    void Update()
    {
        Vector3 currentRot0 = gates[0].transform.localEulerAngles;
        Vector3 currentRot1 = gates[1].transform.localEulerAngles;
        if(opening)
        {
            if(currentRot0.y < openRot)
            {
                gates[0].transform.localEulerAngles = Vector3.Lerp(currentRot0, new Vector3(openRot, currentRot0.y, currentRot0.z), speed * Time.deltaTime);
                gates[1].transform.localEulerAngles = Vector3.Lerp(currentRot1, new Vector3(openRot, currentRot1.y, currentRot1.z), speed * Time.deltaTime);
            }
            else
            {
                Vector3 currentTF0 = gates[0].transform.position;
                Vector3 currentTF1 = gates[1].transform.position;
                if(currentTF0.y > 0)
                {
                    gates[0].transform.position = Vector3.Lerp(currentTF0, new Vector3(currentTF0.x, openRot, currentTF0.z), speed * Time.deltaTime);
                    gates[1].transform.position = Vector3.Lerp(currentTF1, new Vector3(currentTF1.x, openRot, currentTF1.z), speed * Time.deltaTime);
                }
            }
        }
    }

    public void OpenGate()
    {
        opening = true;
    }
}
