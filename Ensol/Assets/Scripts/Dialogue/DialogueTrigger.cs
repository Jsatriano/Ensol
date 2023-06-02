using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static bool triggered = false;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    public Collider dialogue;
    public bool interacted = false;
    public bool repeatInteractable = false;

    private void Update()
    {
        if(DialogueManager.GetInstance().donePlaying == true && dialogue.enabled != true) 
        {
            DialogueManager.GetInstance().donePlaying = false;
            if (dialogue.gameObject.tag == "InteractableOnce")
            {
                interacted = true;
            }
            dialogue.enabled = true;
        }

        if(dialogue.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying && DialogueManager.GetInstance().donePlaying == false && interacted == false) 
        {
            print("start dialogue");
            //Debug.Log(inkJSON.text);
            if(this.gameObject.name == "screen 1") triggered = true;
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        }
        //(DialogueManager.GetInstance().donePlaying);
        
    }
}
