using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _01CabinNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateToDeer = null;
    public ElectricGateController electricGateToGate = null;

    [Header("Interactable Objects")]
    public GameObject doorInteractable;
    public GameObject podInteractable;
    public GameObject windowInteractable;
    public GameObject conveyerInteractable;
    public GameObject plushInteractable;
    
    [Header("Interactable Object Interactors")]
    public CabinDoor doorInteractor;
    public DialogueTrigger podInteractor;
    public DialogueTrigger windowInteractor;
    public DialogueTrigger conveyerInteractor;
    public DialogueTrigger plushInteractor;

    [Header("Broom Stuff")]
    [SerializeField] private MeshRenderer broomMesh;
    [SerializeField] private Material[] interactMat;
    [SerializeField] private Material[] broomMat;

    [Header("Re-Picking up Weapons")]
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject weaponPile;


    [Header("Other Variables")]
    public GameObject gateTransferCube;
    [SerializeField] private GameObject broom;   
    [HideInInspector] public GameObject[] players = null;
    private PlayerCombatController combatController = null;


    private void Start()
    {
        //Only have the broom loaded into the scene if the player hasn't picked it up yet
        if (PlayerData.hasBroom)
        {
            broom.SetActive(false);
        }
        //Only load the interactable broom once the player has died to the crack deer
        else if (PlayerData.diedToCrackDeer)
        {
            broom.tag = "InteractablePickup";
            broomMesh.materials = interactMat;
        }
        else
        {
            broom.tag = "Untagged";
            broomMesh.materials = broomMat;
        }
        weaponPile.SetActive(false);
        stick.SetActive(false);

        if (PlayerData.hasSolarUpgrade)
        {
            if (!PlayerData.currentlyHasSolar)
            {
                weaponPile.SetActive(true);
            }
        }
        else if (PlayerData.hasBroom && !PlayerData.hasSolarUpgrade)
        {
            if (!PlayerData.currentlyHasBroom)
            {
                stick.SetActive(true);
            }
        }
    }

    public void Update()
    {
        if (combatController == null)
        {
            SearchForPlayer();
        }
        if(_02DeerNode.weaponPickedUp && !gateTransferCube.activeInHierarchy)
        {
            gateTransferCube.SetActive(true);
        }
        //Player picking up broom
        if (broom.activeInHierarchy == false && !PlayerData.hasBroom)
        {
            combatController.PickedUpBroom();
        }

        //re-picking up weapon pile
        if (weaponPile.activeInHierarchy == false && !PlayerData.currentlyHasSolar && PlayerData.hasSolarUpgrade && stick.activeInHierarchy == false)
        {
            combatController.PickedUpSolarUpgrade();
        }

        //re-picking up broom stick
        if (stick.activeInHierarchy == false && !PlayerData.currentlyHasBroom && PlayerData.hasBroom && weaponPile.activeInHierarchy == false)
        {        
            combatController.PickedUpBroom();
        }

        if (electricGateToDeer.opening)
        {
            print("unlocked deer node");
            CompletedNodes.deerNode = true;
        }

        if(electricGateToGate.opening)
        {
            print("unlocked gate node");
            CompletedNodes.gateNode = true;
        }

        /* -------------------- Interactable Handling --------------------- */

        // Door
        if(doorInteractor.interacted || PlayerData.doorInteracted)
        {
            PlayerData.doorInteracted = true;
            doorInteractor.interacted = true;
            Destroy(doorInteractable.GetComponent<MeshRenderer>().materials[1]);
            
        }

        // Cloning Pod
        if(podInteractor.interacted || PlayerData.podInteracted)
        {
            PlayerData.podInteracted = true;
            podInteractor.interacted = true;
            Destroy(podInteractable.GetComponent<MeshRenderer>().materials[1]);
            
        }

        // Window
        if(windowInteractor.interacted || PlayerData.windowInteracted)
        {
            PlayerData.windowInteracted = true;
            windowInteractor.interacted = true;
            Destroy(windowInteractable.GetComponent<MeshRenderer>().materials[1]);
        }

        // Conveyer
        if(conveyerInteractor.interacted || PlayerData.conveyerInteracted)
        {
            PlayerData.conveyerInteracted = true;
            conveyerInteractor.interacted = true;
            Destroy(conveyerInteractable.GetComponent<MeshRenderer>().materials[1]);
        }

        // Plush
        if(plushInteractor.interacted || PlayerData.plushInteracted)
        {
            PlayerData.plushInteracted = true;
            plushInteractor.interacted = true;
            Destroy(plushInteractable.GetComponent<MeshRenderer>().materials[1]);
        }

        /* ------------------------------------------------------------------ */
        
    }

    private void SearchForPlayer()
    {
        if (players.Length == 0)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        foreach (GameObject p in players)
        {
            combatController = p.GetComponent<PlayerCombatController>();
        }

        if (combatController == null)
        {
            print("Cabin Node Script Failed to find player");
        }
        else
        {
            print("Cabin Node Script located Player");
        }
    }
}
