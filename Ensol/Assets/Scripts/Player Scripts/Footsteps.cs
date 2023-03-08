using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    //Varaible assignment for playing footsteps audio
    //public GameObject footCollider;
    [Header("Footstep Type Assigner")]
    [Header("1 = player, 2 = deer, 3 = bear")]
    [Range(1, 3)]
    public int footType;
    //public GameObject floor;
    //public bool touchingGrass = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Floor" && footType == 1){
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWalk, this.transform.position);
        } else if (other.tag == "Floor" && footType == 2){
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.deerMove, this.transform.position);
        }
    }

}
