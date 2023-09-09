using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ink.Runtime;

// JUSTIN
public class _00CabinNode : MonoBehaviour
{
    [Header("Exitting Variables")]
    public ElectricGateController electricGateToGate = null;
    public PathCollider exitOnTriggerEnterEvent;
    private bool pathToDeer = false;

    [Header("Interactable Objects")]
    public GameObject doorInteractable;
    public GameObject podInteractable;
    public GameObject windowInteractable;
    public GameObject conveyerInteractable;
    public GameObject podInteractableText;
    public GameObject windowInteractableText;
    public GameObject conveyerInteractableText;
    public GameObject plushInteractable;
    public GameObject podHealingInteractable;
    public GameObject ash;
    public GameObject ashMedium;
    public GameObject ashLarge;
    
    [Header("Interactable Object Interactors")]
    public CabinDoor doorInteractor;
    public DialogueTrigger podInteractor;
    public DialogueTrigger windowInteractor;
    public DialogueTrigger conveyerInteractor;
    public DialogueTrigger plushInteractor;
 

    [Header("Re-Picking up Weapons")]
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject weaponPile;


    [Header("Other Variables")]
    public GameObject gateTransferCube;
    [SerializeField] private GameObject broom;
    [SerializeField] private GameObject interactableBroom;
    [HideInInspector] public GameObject[] players = null;
    private PlayerController combatController = null;
    public GameObject doorMeower;
    private Story story;
    public TextAsset globals;

    private void Awake() 
    {
        // checks all states the player could be entering the cabin from, and spawns them in accordingly
        if (PlayerData.diedToCrackDeer && PlayerData.currentlyHasBroom == false)
        {
            SpawnPoint.First = true;
            SpawnPoint.Second = false;
        }
        else if (SpawnPoint.Mapped)
        {
            SpawnPoint.First = true;
            SpawnPoint.Second = false;
            SpawnPoint.Mapped = false;
        }
        else if (CompletedNodes.prevNode == 1)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = true;
        }
        else if (CompletedNodes.prevNode == 3)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        story = new Story(globals.text);
    }

    private void Start()
    {
        // search for player gameobject
        if (combatController == null)
        {
            SearchForPlayer();
        }

        // changes prevNode static variable to this node
        CompletedNodes.prevNode = 0;

        //make sure player doesnt get hardlocked from health
        if (PlayerData.currHP < 1){
            PlayerData.currHP = combatController.maxHP;
        }

        // if player is not at max HP, allow pod to heal player on interact
        if (PlayerData.currHP < combatController.maxHP){
            print("Swapping pods");
            podInteractable.SetActive(false);
            podHealingInteractable.SetActive(true);
        }

        // Only have the broom loaded into the scene if the player hasn't picked it up yet
        if (PlayerData.hasBroom)
        {
            broom.SetActive(false);
            interactableBroom.SetActive(false);
        }
        // Only load the interactable broom once the player has died to the crack deer
        else if (PlayerData.diedToCrackDeer)
        {
            interactableBroom.SetActive(true);
            broom.SetActive(false);
        }
        else
        {
            broom.SetActive(true);
            ash.SetActive(false);
            interactableBroom.SetActive(false);
        }

        // change ash amount depending on # of player deaths (more deaths = bigger ash pile)
        if (PlayerData.deaths > 1){
            ashMedium.SetActive(true);
        }
        if (PlayerData.deaths > 2){
            ashLarge.SetActive(true);
        }
        weaponPile.SetActive(false);
        stick.SetActive(false);

        // if player died with upgraded weapon, set upgraded weapons on floor
        if (PlayerData.hasSolarUpgrade)
        {
            if (!PlayerData.currentlyHasSolar)
            {
                weaponPile.SetActive(true);
            }
        }
        // if player died with only the broom weapon, set broom weapon on floor
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
        // enables bottom path's transfer cube
        if(_01DeerNode.weaponPickedUp && !gateTransferCube.activeInHierarchy)
        {
            gateTransferCube.SetActive(true);
        }

        //Player picking up broom with a delay for audio
        if (!broom.activeInHierarchy && !interactableBroom.activeInHierarchy && !PlayerData.hasBroom)
        {

            combatController.PickedUpBroom();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.envBroomPickup, this.transform.position);
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

        // allows player to go to gate node
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
            podInteractableText.tag = "Uninteractable";
        }

        // Window
        if(windowInteractor.interacted || PlayerData.windowInteracted)
        {
            PlayerData.windowInteracted = true;
            windowInteractor.interacted = true;
            // removes highlight material from mesh
            windowInteractable.GetComponent<Renderer>().materials[0].SetFloat("_SetAlpha", 0f);
            // disable object in scene
            windowInteractable.SetActive(false);
            windowInteractableText.tag = "Uninteractable";
        }

        // Conveyer
        if(conveyerInteractor.interacted || PlayerData.conveyerInteracted)
        {
            PlayerData.conveyerInteracted = true;
            conveyerInteractor.interacted = true;
            // removes highlight material from mesh
            conveyerInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
            conveyerInteractableText.tag = "Uninteractable";
        }

        // Plush
        if(plushInteractor.interacted || PlayerData.plushInteracted)
        {
            PlayerData.plushInteracted = true;
            plushInteractor.interacted = true;
            // removes highlight material from mesh
            plushInteractable.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);
        }

        //doorMeower
        if (PlayerData.conveyerInteracted && PlayerData.windowInteracted && PlayerData.podInteracted){
            doorMeower.SetActive(true);
        } else {
            doorMeower.SetActive(false);
        }

        /* ------------------------------------------------------------------ */
        
    }

    // searches for player game object in edge case
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
