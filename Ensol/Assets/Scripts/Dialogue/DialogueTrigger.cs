using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    public Collider dialogue;
    public Collider Mesh;

    private void Update()
    {
        if(DialogueManager.GetInstance().donePlaying == true && dialogue.enabled != true) 
        {
            if (Mesh != null)
            {
                Mesh.enabled = true;
            }
            dialogue.enabled = true;
            DialogueManager.GetInstance().donePlaying = false;
        }

        if(dialogue.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying && DialogueManager.GetInstance().donePlaying == false) 
        {
            print("start dialogue");
            //Debug.Log(inkJSON.text);
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        }
        
    }
}
