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
    public GameObject interactBroom;
    public GameObject normalBroom;
    public GameObject[] players = null;
    private PlayerCombatController combatController = null;

    private void Start()
    {
        //Only have the broom loaded into the scene if the player hasn't picked it up yet
        if (PlayerData.hasBroom)
        {
            interactBroom.SetActive(false);
            normalBroom.SetActive(false);
        }
        //Only load the interactable broom once the player has died to the crack deer
        else if (PlayerData.diedToCrackDeer)
        {
            interactBroom.SetActive(true);
            normalBroom.SetActive(false);
        }
        else
        {
            interactBroom.SetActive(false);
            normalBroom.SetActive(true);
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
        if (normalBroom.activeInHierarchy == false && interactBroom.activeInHierarchy == false && !PlayerData.hasBroom)
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
