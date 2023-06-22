using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorCutsceneTrigger : MonoBehaviour
{
    public _11ComputerNode nodeScript;
    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            nodeScript.StartCutsceneDialogue();
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
