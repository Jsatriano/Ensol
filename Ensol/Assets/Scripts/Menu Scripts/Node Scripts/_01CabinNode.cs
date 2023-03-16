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
    

    [Header("Other Variables")]
    public GameObject gateTransferCube;
    [SerializeField] private GameObject broom;
    [SerializeField] private MeshRenderer broomMesh;
    [SerializeField] private Material[] interactMat;
    [SerializeField] private Material[] broomMat;
    [HideInInspector] public GameObject[] players = null;
    private PlayerCombatController combatController = null;
    public GameObject[] cabinInteractables;
    public Collider[] cabinInteractCols;
    public GameObject plush;
    public Collider plushInteractCol;


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
        if (broom.activeInHierarchy == false && broom.activeInHierarchy == false && !PlayerData.hasBroom)
        {
            combatController.PickedUpBroom();
        }

        if(electricGateToDeer.opening)
        {
            print("unlocked deer node");
            CompletedNodes.deerNode = true;
        }

        if(electricGateToGate.opening)
        {
            print("unlocked gate node");
            CompletedNodes.gateNode = true;
        }

        // handles cabin interactables highlight shader removal
        for(int i = 0; i < cabinInteractables.Length; i += 1)
        {
            if(cabinInteractCols[i].enabled == false)
            {
                // removes highlight material from mesh
                Destroy(cabinInteractables[i].GetComponent<MeshRenderer>().materials[1]);
            }
        }

        //handles plush highlight shader removal
        if(plushInteractCol.enabled == false)
        {
            // removes highlight material from mesh
            Destroy(plush.GetComponent<SkinnedMeshRenderer>().materials[1]);
        }
        
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
