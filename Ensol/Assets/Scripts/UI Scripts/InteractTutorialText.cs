using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractTutorialText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private Transform textTF;
    private Camera cam;
    [HideInInspector] public bool canBeInteracted;
    public bool interacted;
    [SerializeField] private Collider coll;

    private void Start()
    {
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
            text.SetActive(false);
        }
        else if (coll.enabled && gameObject.tag != "InteractableOnce")
        {
            interacted = false;
        }

        if (interacted){
            text.SetActive(false);
        }

        textTF.LookAt(cam.transform.position);

    }

    //Turn on text when player is in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canBeInteracted && !interacted && PlayerData.diedToCrackDeer == false)
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(false);
        }
    }
}
