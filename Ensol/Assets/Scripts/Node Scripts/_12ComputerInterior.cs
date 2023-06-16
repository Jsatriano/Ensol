using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _12ComputerInterior : MonoBehaviour
{
    private PlayerController player;
    private Animator playerAnimator;

    [SerializeField] private GameObject doorBar;
    [SerializeField] private Transform lookTarget;

    public static bool doorBarred = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData.prevNode = 11;
        PlayerData.currentNode = 12; //10 and 11 because we want load game to start outside the building
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        doorBar.SetActive(false);
        doorBarred = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doorBarred) {
            doorBar.SetActive(true);
        }
        else if(!doorBarred && player.state != PlayerController.State.INTERACTIONANIMATION){
            print("barring door");
            player.transform.LookAt(new Vector3(lookTarget.position.x, player.transform.position.y, lookTarget.position.z));
            player.state = PlayerController.State.INTERACTIONANIMATION;
            playerAnimator.SetBool("isClosingDoor", true);
        }
    }
}
