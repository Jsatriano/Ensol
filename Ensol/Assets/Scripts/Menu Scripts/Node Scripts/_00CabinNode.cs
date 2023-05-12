using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _00CabinNode : MonoBehaviour
{
    [Header("Exitting Variables")]
    public bool pathToDeer = false;
    public ElectricGateController electricGateToGate = null;
    public PathCollider exitOnTriggerEnterEvent;

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
    private PlayerController combatController = null;
    

    private void Start()
    {
        CompletedNodes.prevNode = 0;
        CompletedNodes.lNode = 0;

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
        if(_01DeerNode.weaponPickedUp && !gateTransferCube.activeInHierarchy)
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

        if (pathToDeer)
        {
            CompletedNodes.deerNode = true;
        }

        if(electricGateToGate.opening)
        {
            CompletedNodes.gateNode = true;
        }

        /* -------------------- Interactable Handling --------------------- */

        // Door
        if(doorInteractor.interacted || PlayerData.doorInteracted)
        {
            PlayerData.doorInteracted = true;
            doorInteractor.interacted = true;
            // removes highlight material from mesh
            doorInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            
        }

        // Cloning Pod
        if(podInteractor.interacted || PlayerData.podInteracted)
        {
            PlayerData.podInteracted = true;
            podInteractor.interacted = true;
            // removes highlight material from mesh
            podInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            
        }

        // Window
        if(windowInteractor.interacted || PlayerData.windowInteracted)
        {
            PlayerData.windowInteracted = true;
            windowInteractor.interacted = true;
            // removes highlight material from mesh
            windowInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        }

        // Conveyer
        if(conveyerInteractor.interacted || PlayerData.conveyerInteracted)
        {
            PlayerData.conveyerInteracted = true;
            conveyerInteractor.interacted = true;
            // removes highlight material from mesh
            conveyerInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        }

        // Plush
        if(plushInteractor.interacted || PlayerData.plushInteracted)
        {
            PlayerData.plushInteracted = true;
            plushInteractor.interacted = true;
            // removes highlight material from mesh
            plushInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
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
            combatController = p.GetComponent<PlayerController>();
        }

        /*if (combatController == null)
        {
            print("Cabin Node Script Failed to find player");
        }
        else
        {
            print("Cabin Node Script located Player");
        }*/
    }

    //have to detect collider on another object
    void OnEnable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.AddListener(ExitTriggerMethod);
    }

    void OnDisable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.RemoveListener(ExitTriggerMethod);
    }

    void ExitTriggerMethod(Collider col){
        
        if (col.tag == "Player"){
            pathToDeer = true;
        }
        
    }
}
