using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02DeerNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;
    
    [Header("Weapon Pickup Variables")]
    public GameObject weaponPickup;
    public GameObject dropDeer;
    public static bool weaponPickedUp = false;

    [Header("Deer")]
    public GameObject normalDeer;
    public GameObject crackDeer;

    private bool dropped = false;
    private GameObject inSceneItem = null;

    // timer to stop weaponPickup from insta-dropping
    private float timerLength = 0.25f;
    private float timer = 0f;

    public GameObject[] players = null;
    private PlayerCombatController combatController = null;
    private bool pickedUpUpgrade;

    private void Start()
    {
        //Picks whether the node has the normal or crack deer depending on if the player has picked up the broom
        if (PlayerData.hasBroom)
        {
            normalDeer.SetActive(true);
            crackDeer.SetActive(false);
        }
        else
        {
            normalDeer.SetActive(false);
            crackDeer.SetActive(true);
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (combatController == null)
        {
            SearchForPlayer();
        }

        // spawns weapon pickup item after short delay if dropDeer is dead
        if (timer >= timerLength)
        {
            if (!PlayerData.hasSolarUpgrade && normalDeer.GetComponent<DeerBT>().isAlive == false && dropped == false)
            {
                StartCoroutine(DropAfterDelay(weaponPickup, normalDeer));
                dropped = true;
            }
        }

        // checks if weapon has been spawned, and picked up
        if(inSceneItem != null && inSceneItem.activeInHierarchy == false && !PlayerData.hasSolarUpgrade)
        {
            weaponPickedUp = true;
            combatController.PickedUpSolarUpgrade();
        }

        // unlocks next node if door is opened
        if(electricGateController.opening)
        {
            print("unlocked river node");
            CompletedNodes.riverNode = true;
        }
    }

    public IEnumerator DropAfterDelay(GameObject item, GameObject enemy)
    {
        yield return new WaitForSeconds(1f);

        // spawn item, spawn particles and parent item
        inSceneItem = Instantiate(item, enemy.transform.position, enemy.transform.rotation);
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
