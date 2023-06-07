using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGateController : MonoBehaviour
{
    public GameObject[] gates;
    public float openRot, speed;
    [HideInInspector] public bool opening = false;
    private float endY;
    private bool audioPlayed = false;

    void Start()
    {
        opening = false;
        endY = gates[0].transform.position.y - 4.3f;
    }

    void Update()
    {
        Vector3 currentRot0 = gates[0].transform.localEulerAngles;
        Vector3 currentRot1 = gates[1].transform.localEulerAngles;
        Vector3 currentRot2 = gates[2].transform.localEulerAngles;
        Vector3 currentRot3 = gates[3].transform.localEulerAngles;
        if(opening)
        {
            // moves upper gate
            if(currentRot0.x <= openRot - 0.5)
            {
                // rotates upward
                gates[0].transform.localEulerAngles = Vector3.Lerp(currentRot0, new Vector3(openRot, currentRot0.y, currentRot0.z), speed * Time.deltaTime);
                gates[2].transform.localEulerAngles = Vector3.Lerp(currentRot0, new Vector3(openRot, currentRot2.y, currentRot2.z), speed * Time.deltaTime);
            }
            else
            {
                // transforms down
                Vector3 currentTF0 = gates[0].transform.position;
                Vector3 currentTF2 = gates[2].transform.position;
                gates[0].transform.position = Vector3.Lerp(currentTF0, new Vector3(currentTF0.x, endY, currentTF0.z), speed * Time.deltaTime);
                gates[2].transform.position = Vector3.Lerp(currentTF2, new Vector3(currentTF2.x, endY, currentTF2.z), speed * Time.deltaTime);
            }

            // moves lower gate
            if(currentRot1.x <= openRot - 0.5)
            {
                // rotates upward
                gates[1].transform.localEulerAngles = Vector3.Lerp(currentRot1, new Vector3(openRot, currentRot1.y, currentRot1.z), speed * Time.deltaTime);
                gates[3].transform.localEulerAngles = Vector3.Lerp(currentRot1, new Vector3(openRot, currentRot3.y, currentRot3.z), speed * Time.deltaTime);
            }
            else
            {
                // transforms down
                Vector3 currentTF1 = gates[1].transform.position;
                Vector3 currentTF3 = gates[3].transform.position;
                gates[1].transform.position = Vector3.Lerp(currentTF1, new Vector3(currentTF1.x, endY, currentTF1.z), speed * Time.deltaTime);
                gates[3].transform.position = Vector3.Lerp(currentTF3, new Vector3(currentTF3.x, endY, currentTF3.z), speed * Time.deltaTime);
            }
        }
    }

    public void OpenGate()
    {
        StartCoroutine(DelayOpen());
    }

    public IEnumerator DelayOpen()
    {
        yield return new WaitForSeconds(1.5f);
        opening = true;
        if (audioPlayed == false){
            audioPlayed = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.envGateOpen, this.transform.position);
            //AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        }
    }
}
