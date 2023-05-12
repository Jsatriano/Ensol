using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SpiderCogSounds : MonoBehaviour
{
    //Varaible assignment for playing footsteps audio
    //public GameObject footCollider;
    [Header("Footstep Type Assigner")]
    [Header("0 = player, 1 = deer, 2 = bear, 3 = rabbit")]
    [Range(0, 5)]
    public int footType;
    public FMODUnity.EventReference[] footstep;
    [Range(0, 1)]
    public int cogType;
    public FMODUnity.EventReference[] cogstep;
    private FMOD.Studio.EventInstance instance;
    // bool touchingMetal = false;
    int touchingMetal = 0;
    int touchingDirt = 0;
    int touchingStone = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "FloorMetal")
        {
            touchingMetal++;
        }

        if (other.tag == "FloorDirt")
        {
            touchingDirt++;
        }

        if (other.tag == "FloorStone")
        {
            touchingStone++;
        }

        if (other.tag == "Floor")
        {
            if (touchingMetal > 0)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.metalMove, this.transform.position);
            }
            else if (touchingDirt > 0)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.dirtMove, this.transform.position);
            }
            else if (touchingStone > 0)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.stoneMove, this.transform.position);
            }
            else
            {
                RuntimeManager.PlayOneShot(footstep[footType], this.transform.position);
                RuntimeManager.PlayOneShot(cogstep[cogType], this.transform.position);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == "FloorWood")
        {
            touchingMetal--;
        }
        else if (other.tag == "FloorDirt")
        {
            touchingDirt--;
        }
        else if (other.tag == "FloorStone")
        {
            touchingStone--;
        }
    }

}
