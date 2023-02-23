using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public Camera cam;

    private void LateUpdate()
    {
        transform.LookAt(cam.transform.position);

        if (CompletedNodes.deerNode || CompletedNodes.gateNode)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
