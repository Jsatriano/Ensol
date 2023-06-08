using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class _02RiverNode : MonoBehaviour
{


    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    [Header("Interactable Objects")]
    public GameObject[] footstepsInteractable;
    
    [Header("Interactable Object Interactors")]
    public DialogueTrigger footstepsInteractor;
    public GameObject footstepsDialogue;

    [Header("Water Variables")]
    public GameObject water;
    public GameObject[] waterBounds;

    private void Awake() 
    {
        if (CompletedNodes.prevNode == 1)
        {
            SpawnPoint.First = true;
            SpawnPoint.Second = false;
        }
        else if (CompletedNodes.prevNode == 5 || CompletedNodes.prevNode == 8)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = true;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 2;
    }

    private void Start()
    {
        CompletedNodes.firstLoad[2] = false;


        // if water turned off in river node, disable water and boundaries in this node
        if (PlayerData.controlsHit && water.activeInHierarchy)
        {
            water.SetActive(false);
            footstepsDialogue.SetActive(false);
            foreach (GameObject waterBound in waterBounds)
            {
                waterBound.SetActive(false);
            }
        }
    }

    public void Update()
    {

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
            CompletedNodes.completedNodes[2] = true;
        }

    }
}
