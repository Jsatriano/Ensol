using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _08BirdNode : MonoBehaviour
{
    public static bool transponderPickedUp = false;
    public GameObject bird, birdBody, deadBird, transferCube;
    private GameObject activeDeadBird = null;
    private bool spawningBird = false;
    private Vector3 birdRotation = new Vector3(-90.298f, -77.705f + 180f, -101.751f);
    private Vector3 birdPosition = new Vector3(-0.01194038f, -0.003794938f, 0.0007437048f);
    private PlayerController combatController = null;
    [HideInInspector] public GameObject[] players = null;
    
    private void Start()
    {
        CompletedNodes.prevNode = 8;
        if(PlayerData.hasTransponder) {
            bird.SetActive(false);
        }
        CompletedNodes.firstLoad[8] = false;
    }

    private void Update() {
        if (combatController == null)
        {
            SearchForPlayer();
        }

        if(PlayerData.killedBird && !spawningBird) {
            PlayerData.disableBird = true;
            spawningBird = true;
            StartCoroutine(SpawnDeadBird());
        }

        if(activeDeadBird != null && !PlayerData.hasTransponder) {
            Debug.Log("Bird active: " + activeDeadBird.activeInHierarchy);
            Debug.Log("Has Trans: " + PlayerData.hasTransponder);
            if(!activeDeadBird.activeInHierarchy && !PlayerData.hasTransponder){ 
                PlayerData.hasTransponder = true;
                transferCube.SetActive(true);
                combatController.PickedUpTransponder();
            }
            else {
                transferCube.SetActive(false);
            }
            
        }
        else
        {
            transferCube.SetActive(true);
        }
    }

    private IEnumerator SpawnDeadBird() {
        yield return new WaitForSeconds(0.9f);
        activeDeadBird = Instantiate(deadBird, birdBody.transform.position, birdBody.transform.rotation, bird.transform);
        birdBody.SetActive(false);
        activeDeadBird.transform.Rotate(birdRotation);
        activeDeadBird.transform.position += birdPosition;
        CompletedNodes.completedNodes[8] = true;
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
