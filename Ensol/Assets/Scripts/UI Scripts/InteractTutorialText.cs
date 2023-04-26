using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractTutorialText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Transform textTF;
    private Camera cam;
    [HideInInspector] public bool canBeInteracted;
    public bool interacted;
    private string noText = "";
    [TextArea] public string interactText;
    [SerializeField] private Collider coll;

    private void Start()
    {
        textMesh.text = noText;
        interacted = false;
        canBeInteracted = true;
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        //Turns off text once interacted with
        if (!coll.enabled)
        {
            interacted = true;
            textMesh.text = noText;
        }
        else if (coll.enabled && gameObject.tag != "InteractableOnce")
        {
            interacted = false;
        }

        if (interacted){
            textMesh.text = noText;
        }

        textTF.LookAt(cam.transform.position);

    }

    //Turn on text when player is in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canBeInteracted && !interacted)
        {
            textMesh.text = interactText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            textMesh.text = noText;
        }
    }
}
