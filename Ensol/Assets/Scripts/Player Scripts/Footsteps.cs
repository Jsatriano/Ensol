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
    [Header("0 = player, 1 = deer, 2 = bear, 3 = rabbit, 4 = spider")]
    [Range(0, 5)]
    public int footType;
    public FMODUnity.EventReference[] footstep;
    public bool cogsteper = false;
    [Range(0, 1)]
    public int cogType;
    public FMODUnity.EventReference[] cogstep;
    private FMOD.Studio.EventInstance instance;
    // bool touchingMetal = false;
    public int touchingMetal = 0;
    public int touchingDirt = 0;
    public int touchingStone = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){

        if (other.tag == "FloorMetal"){
            touchingMetal++;
            
        }

        if (other.tag == "FloorDirt"){
            touchingDirt++;
        }

        if (other.tag == "FloorStone")
        {
            touchingStone++;
        }

        if (other.tag == "Floor"){
            if (touchingMetal > 0){
                if (footType == 0){
                    print("playerMetalStep");
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.metalMove, this.transform.position);
                } else {
                    print("deerMetalStep");
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.deerMoveMetal, this.transform.position);
                }
            } else if (touchingDirt > 0){
                print("playerDirtStep");
                AudioManager.instance.PlayOneShot(FMODEvents.instance.dirtMove, this.transform.position);
            } else if (touchingStone > 0){
                print("playerStoneStep");
                AudioManager.instance.PlayOneShot(FMODEvents.instance.stoneMove, this.transform.position);
            } else {
                RuntimeManager.PlayOneShot(footstep[footType], this.transform.position);
                if (footType == 4 && cogsteper == true){
                    RuntimeManager.PlayOneShot(footstep[footType], this.transform.position);
                    RuntimeManager.PlayOneShot(cogstep[cogType], this.transform.position);
                }
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

        if (other.tag == "FloorMetal"){
            touchingMetal--;
        } else if (other.tag == "FloorDirt"){
            touchingDirt--;
        } else if (other.tag == "FloorStone"){
            touchingStone--;
        }
    }

}
