using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class CabinDoor : MonoBehaviour
{
    public Collider collider;
    public DoorController doorController;
    private bool opened = false;
    public GameObject openText;

    //checker to tell other code when an object has been interacted with
    public bool interacted = false;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private TextAsset cannotLeave;

    void Update()
    {
        if (PlayerData.diedToCrackDeer)
        {
            if(!collider.enabled)
            {
                if(PlayerData.hasBroom)
                {
                    // opens door
                    gameObject.tag = "Uninteractable";
                    doorController.OpenDoor();
                    openText.SetActive(false);
                }
                else
                {
                    DialogueManager.GetInstance().EnterDialogueMode(cannotLeave);
                    StartCoroutine(ColliderReenable());
                }
            }
        }
        else
        {
            if(DialogueManager.GetInstance().donePlaying == true && collider.enabled != true) 
            {
                //print("line 21");
                StartCoroutine(ColliderReenable());
            }

            if(collider.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying && DialogueManager.GetInstance().donePlaying == false) 
            {
               // print("start dialogue");
                //Debug.Log(inkJSON.text);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
            }
            if (DialogueManager.GetInstance().openSesame == true)
            {
                if(!collider.enabled)
                {
                    // opens door
                    if(opened == false)
                    {
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.envCabinDoorOpen, this.transform.position);
                        opened = true;
                    }
                    gameObject.tag = "Uninteractable";
                    doorController.OpenDoor();
                    openText.SetActive(false);
                    interacted = true;
                }
            }
        }
    }

    public IEnumerator ColliderReenable(){
            yield return new WaitForSeconds(0.3f);
            DialogueManager.GetInstance().donePlaying = false;
            collider.enabled = true;
    }
}
