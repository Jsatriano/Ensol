using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBuilding : MonoBehaviour
{
    private bool wasUsed = false;
    private bool distributedHealing = false;
    [HideInInspector] public PlayerCombatController pcc;
    [HideInInspector] public CharController player;
    [HideInInspector] public GameObject[] players;
    public Renderer renderer;

    void Awake() {
        gameObject.tag = "Interactable";
    }
    // Start is called before the first frame update
    void Start()
    {
        SearchForPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) {
            SearchForPlayer();
        }

        if(!gameObject.GetComponent<Collider>().enabled) {
            wasUsed = true;
        }

        if(wasUsed && !distributedHealing) {
            distributedHealing = true;
            PlayerData.currHP = pcc.maxHP;
            healthBar.SetHealth(PlayerData.currHP);
            Debug.Log("Healing Distributed");
            renderer.materials[1].SetFloat("_SetAlpha", 0);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.healUp, this.transform.position);
        }
        
    }

      public void SearchForPlayer() {
        if(players.Length == 0) {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        foreach(GameObject p in players) {
            player = p.GetComponent<CharController>();
            pcc = p.GetComponent<PlayerCombatController>();
        }
    }
}
