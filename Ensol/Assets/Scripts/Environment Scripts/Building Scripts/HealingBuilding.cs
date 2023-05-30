using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBuilding : MonoBehaviour
{
    private bool wasUsed = false;
    private bool distributedHealing = false;
    [HideInInspector] public PlayerController player;
    [HideInInspector] public GameObject[] players;
    [HideInInspector] public HealthBar healthBar;
    public Renderer renderer;
    public GameObject healJuice, healSprayVFX;
    public Transform juiceDrainTarget;
    public float drainSpeed = 1f;
    private Vector3 target;

    void Awake() {
        gameObject.tag = "Interactable";
    }
    // Start is called before the first frame update
    void Start()
    {
        SearchForPlayer();
        healSprayVFX.SetActive(false);
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
            PlayerData.currHP = player.maxHP;
            healthBar.SetHealth(PlayerData.currHP);
            Debug.Log("Healing Distributed");
            renderer.materials[1].SetFloat("_SetAlpha", 0);
            // play sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.healUp, this.transform.position);
        }
        if(wasUsed && distributedHealing && healJuice.activeInHierarchy){
            healSprayVFX.SetActive(true);
            if(healJuice.transform.position != juiceDrainTarget.position){
                healJuice.transform.position = Vector3.MoveTowards(healJuice.transform.position, juiceDrainTarget.position, drainSpeed * Time.deltaTime);
            }
            else {
                healJuice.SetActive(false);
                healSprayVFX.SetActive(false);
            }
        }
        
    }

      public void SearchForPlayer() {
        if(players.Length == 0) {
            players = GameObject.FindGameObjectsWithTag("Player");
            healthBar = GameObject.FindGameObjectWithTag("HealthbarUI").GetComponent<HealthBar>();
        }
        foreach(GameObject p in players) {
            player = p.GetComponent<PlayerController>();
        }
    }
}
