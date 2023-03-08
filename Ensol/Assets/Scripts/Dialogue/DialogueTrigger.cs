using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    public Collider c;

    private void Update()
    {
        if(DialogueManager.GetInstance().donePlaying == true && c.enabled != true) 
        {
            print("its done playing");
            c.enabled = true;
            DialogueManager.GetInstance().donePlaying = false;
        }

        if(c.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying) 
        {
            print("start dialogue");
            //Debug.Log(inkJSON.text);
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
        
    }
}
