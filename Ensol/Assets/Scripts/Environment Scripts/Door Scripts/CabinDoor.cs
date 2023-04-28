using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class CabinDoor : MonoBehaviour
{
    public Collider buttonCol;
    public DoorController doorController;
    private bool opened = false;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;
    public bool interacted = false;

    void Update()
    {
        if (PlayerData.diedToCrackDeer)
        {
            if(!buttonCol.enabled)
            {
                if(PlayerData.hasBroom)
                {
                    // opens door
                    doorController.OpenDoor();
                }
                else
                {
                    buttonCol.enabled = true;
                }
            }
        }
        else
        {
            if(DialogueManager.GetInstance().donePlaying == true && buttonCol.enabled != true) 
            {
                //print("line 21");
                buttonCol.enabled = true;
                DialogueManager.GetInstance().donePlaying = false;
            }

            if(buttonCol.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying && DialogueManager.GetInstance().donePlaying == false) 
            {
               // print("start dialogue");
                //Debug.Log(inkJSON.text);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
            }
            if (DialogueManager.GetInstance().openSesame == true)
            {
                if(!buttonCol.enabled)
                {
                    // opens door
                    if(opened == false)
                    {
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.envCabinDoorOpen, this.transform.position);
                        opened = true;
                    }
                    doorController.OpenDoor();
                    interacted = true;
                }
            }
        }
    }
}
