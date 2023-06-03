using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static bool triggered = false;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    public Collider collider;

    //checker to tell other code when an object has been interacted with
    public bool interacted = false;

    private void Update()
    {
        if(DialogueManager.GetInstance().donePlaying == true && collider.enabled != true) 
        {
            StartCoroutine(ColliderReenable());
            if (collider.gameObject.tag == "InteractableOnce")
            {
                collider.gameObject.tag = "Uninteractable";
            }
            interacted = true;
        }

        if(collider.enabled == false && !DialogueManager.GetInstance().dialogueisPlaying && DialogueManager.GetInstance().donePlaying == false) 
        {
            //print("start dialogue");
            //Debug.Log(inkJSON.text);
            if(this.gameObject.name == "screen 1") triggered = true;
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        }
        //(DialogueManager.GetInstance().donePlaying);
        
    }

    public IEnumerator ColliderReenable(){
            yield return new WaitForSeconds(0.3f);
            DialogueManager.GetInstance().donePlaying = false;
            collider.enabled = true;
    }
}
