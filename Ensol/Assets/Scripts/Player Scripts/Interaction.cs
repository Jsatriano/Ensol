using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public void Update()
    {
        if(DialogueManager.GetInstance().dialogueisPlaying == false)
        {
            if(Input.GetButtonDown("Interact")){
                Interact();
            }
        }
    }

    public void Interact()
    {
        print("interact played anyways");
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in inRangeColliders)
        {
            print(collider);
            if(collider.gameObject.tag == "InteractablePickup")
            {
                print("picked up object");
                collider.gameObject.SetActive(false);
            }
            else if(collider.gameObject.tag == "InteractableStory" | collider.gameObject.tag == "InteractableOnce")
            {
                print("inspected story element");
                collider.enabled = false;
            }
            else if (collider.gameObject.tag == "Interactable")
            {
                print("inspected interactable");
                collider.enabled = false;
            }
        }
    }
}
