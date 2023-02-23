using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTutorialText : MonoBehaviour
{
    public Transform text;
    private Camera cam;
    public bool opened;

    private void Start()
    {
        opened = false;
    }


    private void LateUpdate()
    {
        if (opened)
        {
            text.gameObject.SetActive(false);
        }
        if (cam == null)
        {
            GameObject[] camera = GameObject.FindGameObjectsWithTag("MainCamera");
            if (camera.Length > 0)
            {
                cam = camera[0].GetComponent<Camera>();
            }
        }
        else
        {
            text.LookAt(cam.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            text.gameObject.SetActive(false);
        }
    }
}
