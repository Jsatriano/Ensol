using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _01DeerNode : MonoBehaviour
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
    [SerializeField] private Transform deerSpawnPoint;
    private GameObject deer;

    private bool dropped = false;
    private GameObject inSceneItem = null;

    // timer to stop weaponPickup from insta-dropping
    private float timerLength = 0.25f;
    private float timer = 0f;

    [HideInInspector] public GameObject[] players = null;
    private PlayerController combatController = null;
    private bool pickedUpUpgrade;

    [Header("Weapon Upgrade")]
    [SerializeField] private GameObject deadDear;
    [SerializeField] private GameObject guttedDeer;

    [Header("Other Variables")]
    public GameObject transferCube;
    private Story story;
    public TextAsset globals;


    private void Awake() 
    {

        if (CompletedNodes.prevNode == 0)
        {
            SpawnPoint.First = true;
        }
        else if (CompletedNodes.prevNode == 2)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 1;

        //Picks whether the node has the normal or crack deer depending on if the player has picked up the broom
        if (PlayerData.hasBroom)
        {
            deer = Instantiate(normalDeer, deerSpawnPoint);
        }
        else
        {
            deer = Instantiate(crackDeer, deerSpawnPoint);
        }
    }

    private void Start()
    {
        
        CompletedNodes.firstLoad[1] = false;
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
            if (!PlayerData.hasSolarUpgrade && PlayerData.hasBroom && deer.GetComponent<DeerBT>().isAlive == false && dropped == false && deer.GetComponentInChildren<DeerAnimation>().finishedDeathAnim)
            {
                if (PlayerData.deerKilled >= 1) 
                {
                    story = new Story(globals.text);
                    story.state.LoadJson(DialogueVariables.saveFile);
                    story.EvaluateFunction("killedDeer");
                    DialogueVariables.saveFile = story.state.ToJson();
                }
                
                ReplaceDeadDeer(deadDear, deer);
                dropped = true;
            }
        }

        // checks if weapon has been spawned, and picked up
        if(inSceneItem != null && inSceneItem.activeInHierarchy == false && !PlayerData.hasSolarUpgrade)
        {
            weaponPickedUp = true;
            combatController.PickedUpSolarUpgrade();
            Instantiate(guttedDeer, inSceneItem.transform.position, inSceneItem.transform.rotation);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.envLootPickup, guttedDeer.transform.position);
            //transferCube.SetActive(true);
        }

        if(PlayerData.hasSolarUpgrade)
        {
            transferCube.SetActive(true);
        }

        // unlocks next node if door is opened
        if(electricGateController.opening)
        {
            CompletedNodes.riverNode = true;
            CompletedNodes.completedNodes[1] = true;
        }
    }

    private void ReplaceDeadDeer(GameObject item, GameObject enemy)
    {
        // spawn item, spawn particles and parent item
        Vector3 offset = new Vector3(-.0529f, 0.7787f, -.22846f);
        enemy.SetActive(false);
        inSceneItem = Instantiate(item, enemy.transform.position + offset, enemy.transform.rotation);
        inSceneItem.transform.Rotate(-90, 0, 0);    
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
    }
}
