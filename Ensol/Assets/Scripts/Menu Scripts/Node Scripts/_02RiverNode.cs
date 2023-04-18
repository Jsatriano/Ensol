using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02RiverNode : MonoBehaviour
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

    private void Awake() 
    {
        print("lnode is " + CompletedNodes.lNode);
        if (CompletedNodes.lNode == 1)
        {
            SpawnPoint.First = true;
        }
        else if (CompletedNodes.lNode == 5 || CompletedNodes.lNode == 8)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
            print(SpawnPoint.First);
        }
        CompletedNodes.lNode = 2;
    }

    private void Start()
    {
        
        CompletedNodes.prevNode = 2;
    }

    public void Update()
    {
        // if water turned off in river node, disable water and boundaries in this node
        if(_04RiverControlNode.controlsHit && water.activeInHierarchy)
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
                // removes highlight material from mesh
                footstep.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            }
            
        }

        /* ------------------------------------------------------------------ */
        
        
        
        
        
        if(electricGateController.opening)
        {
            CompletedNodes.birdNode = true;
            CompletedNodes.bearNode = true;
        }

    }
}
