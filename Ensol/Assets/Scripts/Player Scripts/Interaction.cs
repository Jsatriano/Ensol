using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public bool storyElementTouched = false;

    public void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            Interact();
        }
    }

    public void Interact()
    {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var collider in inRangeColliders)
        {
            if(collider.gameObject.tag == "InteractablePickup")
            {
                print("picked up object");
                collider.gameObject.SetActive(false);
            }
            else if(collider.gameObject.tag == "InteractableStory")
            {
                print("inspected story element");
                collider.enabled = false;
            }
        }
    }
}
