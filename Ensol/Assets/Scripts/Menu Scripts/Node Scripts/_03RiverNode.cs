using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _03RiverNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    [Header("Interactable Objects")]
    public GameObject[] footstepsInteractable;
    
    [Header("Interactable Object Interactors")]
    public DialogueTrigger footstepsInteractor;

    [Header("Water Variables")]
    public GameObject water;
    public GameObject[] waterBounds;

    public void Update()
    {
        // if water turned off in river node, disable water and boundaries in this node
        if(_05RiverControlNode.controlsHit && water.activeInHierarchy)
        {
            water.SetActive(false);
            foreach (GameObject waterBound in waterBounds)
            {
                waterBound.SetActive(false);
            }
        }

        /* -------------------- Interactable Handling --------------------- */

        // Footsteps
        if(footstepsInteractor.interacted || PlayerData.footstepsInteracted)
        {
            PlayerData.footstepsInteracted = true;
            footstepsInteractor.interacted = true;
            foreach(GameObject footstep in footstepsInteractable)
            {
                Destroy(footstep.GetComponent<MeshRenderer>().materials[1]);
            }
            
        }

        /* ------------------------------------------------------------------ */
        
        
        
        
        
        if(electricGateController.opening)
        {
            print("unlocked bird node");
            CompletedNodes.birdNode = true;
            print("unlocked bird node");
            CompletedNodes.bearNode = true;
        }

    }
}
