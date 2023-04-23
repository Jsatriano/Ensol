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
        //print("interact played anyways");
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in inRangeColliders)
        {
            //print(collider);
            if(collider.gameObject.tag == "InteractablePickup")
            {
               // print("picked up object");
                collider.gameObject.SetActive(false);
                if (collider.gameObject.name == "broom")
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.envBroomPickup, this.transform.position);
                }
                if (collider.gameObject.name == "Weapon Pile")
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.envLootPickup, this.transform.position);
                }
            }
            else if(collider.gameObject.tag == "InteractableStory" | collider.gameObject.tag == "InteractableOnce")
            {
               // print("inspected story element");
                collider.enabled = false;
            }
            else if (collider.gameObject.tag == "Interactable")
            {
              //  print("inspected interactable");
                collider.enabled = false;
            }
        }
    }
}
