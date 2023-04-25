using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Footsteps : MonoBehaviour
{
    //Varaible assignment for playing footsteps audio
    //public GameObject footCollider;
    [Header("Footstep Type Assigner")]
    [Header("0 = player, 1 = deer, 2 = bear, 3 = rabbit")]
    [Range(0, 5)]
    public int footType;
    public FMODUnity.EventReference[] footstep;
    private FMOD.Studio.EventInstance instance;
    bool touchingMetal = false;
    bool touchingWood = false;
    int touchingDirt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){

        if (other.tag == "FloorWood"){
            touchingWood = true;
        }

        if (other.tag == "FloorDirt"){
            touchingDirt++;
        }

        if (other.tag == "Floor"){
            if (touchingWood){
                AudioManager.instance.PlayOneShot(FMODEvents.instance.woodMove, this.transform.position);
            } else if (touchingDirt > 0){
                AudioManager.instance.PlayOneShot(FMODEvents.instance.dirtMove, this.transform.position);
            } else {
                RuntimeManager.PlayOneShot(footstep[footType], this.transform.position);
            }
        } 
        /*else if (other.tag == "FloorWood" && footType == 1)
        {
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.woodMove, this.transform.position);
        }
        else if (other.tag == "FloorDirt" && footType == 1)
        {
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.dirtMove, this.transform.position);
        }
        else if (other.tag == "Floor" && footType == 2){
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.deerMove, this.transform.position);
        } else if (other.tag == "Floor" && footType == 3){
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bearMove, this.transform.position);
        } else if (other.tag == "Floor" && footType == 4){
            //touchingGrass = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bunnyMove, this.transform.position);
        }*/
    }

    void OnTriggerExit(Collider other){

        if (other.tag == "FloorWood"){
            touchingWood = false;
        } else if (other.tag == "FloorDirt"){
            touchingDirt--;
        }
    }

}
