using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public PlayerController player;
    private GameObject targetedPickup;
    public GameObject blackOutSquare;
    void Awake() {
        player = gameObject.GetComponent<PlayerController>();
    }

    public void Update()
    {
        if(DialogueManager.GetInstance().dialogueisPlaying == false)
        {
            if(Input.GetButtonDown("Interact") && (player.state == PlayerController.State.MOVING || player.state == PlayerController.State.IDLE)){
                Interact();
            }
        }
    }

    public void Interact()
    {
        //print("interact played anyways");
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in inRangeColliders)
        {
            //print(collider);
            if(collider.gameObject.tag == "InteractablePickup")
            {
               // print("picked up object");
                targetedPickup = collider.gameObject;
                if (collider.gameObject.name == "Weapon Pile")
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.envLootPickup, this.transform.position);
                }
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isPickup", true);
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else{
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                //fade the camera out and in to hide pickup transition
                StartCoroutine(FadeOutAndIn());
                player.state = PlayerController.State.INTERACTIONANIMATION;
            }
            else if(collider.gameObject.tag == "InteractableStory" | collider.gameObject.tag == "InteractableOnce")
            {
               // print("inspected story element");
                collider.enabled = false;
            }
            else if (collider.gameObject.tag == "Interactable")
            {
                //print("inspected interactable");
                collider.enabled = false;
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isHack", true);
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else{
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                player.state = PlayerController.State.INTERACTIONANIMATION;
            }
            else if(collider.gameObject.tag == "Checkpoint" && !collider.gameObject.GetComponent<Checkpoint>().active){
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isPickup", true);
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else{
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                player.state = PlayerController.State.INTERACTIONANIMATION;
                collider.gameObject.GetComponent<Checkpoint>().ActivateCheckpoint();
            }
            else if(collider.gameObject.tag == "Checkpoint" && collider.gameObject.GetComponent<Checkpoint>().active){
                player.state = PlayerController.State.DIALOGUE;
                collider.gameObject.GetComponent<Checkpoint>().UseActiveCheckpoint();
            }
        }
    }

    //anim events for pickups
    private void DeactivatePickup(){
        targetedPickup.SetActive(false);
    }

    //quick fade out and in
    public IEnumerator FadeOutAndIn(){

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        bool fadeBackIn = false;
        float fadeSpeed = 1.1f;

        if(fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a >= 1)
                {
                    //start fading back in
                    fadeToBlack = false;
                    yield return new WaitForSeconds(1f);
                    fadeBackIn = true;
                }
                yield return null;
            }
        }
        if(fadeBackIn)
        {
            while(blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a <= 0)
                {
                    //stop fading
                    fadeBackIn = false;
                }
                yield return null;
            }
        }
    }
}
