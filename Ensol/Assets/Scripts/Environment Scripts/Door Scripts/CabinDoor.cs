using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class CabinDoor : MonoBehaviour
{
    public Collider buttonCol;
    public DoorController doorController;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;
    public bool interacted = false;

    void Update()
    {
        if (interacted == false)
        {
            if(DialogueManager.GetInstance().donePlaying == true && buttonCol.enabled != true) 
            {
                print("line 21");
                buttonCol.enabled = true;
                DialogueManager.GetInstance().donePlaying = false;
            }

            if(buttonCol.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying && DialogueManager.GetInstance().donePlaying == false) 
            {
                print("start dialogue");
                //Debug.Log(inkJSON.text);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
            }
            if (DialogueManager.GetInstance().openSesame == true)
            {
                if(!buttonCol.enabled)
                {
                    // opens door
                    doorController.OpenDoor();
                    interacted = true;
                }
            }
        }
        else
        {
            if(!buttonCol.enabled)
            {
                // opens door
                doorController.OpenDoor();
            }
        }
        
    }
}
