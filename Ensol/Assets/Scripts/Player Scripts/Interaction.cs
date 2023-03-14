using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            Interact();
        }
    }

    public void Interact()
    {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 4f);
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
                // if (collider != MeshCollider) 
                // {
                //     print("hello");
                // }
                collider.enabled = false;
            }
        }
    }
}
