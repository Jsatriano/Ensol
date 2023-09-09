using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// JUSTIN
public class Interaction : MonoBehaviour
{
    public PlayerController player;
    private GameObject targetedPickup;
    public GameObject blackOutSquare;
    private bool petting = false;
    public bool checkpointInteracting = false;
    private PlayerInputActions playerInputActions;
     

    void Awake() {
        // sets up variables on awake
        player = gameObject.GetComponent<PlayerController>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public void Update()
    {
        // if there is no dialogue playing
        if(DialogueManager.GetInstance().dialogueisPlaying == false)
        {
            // if the player is in the correct state, interact
            if(playerInputActions.Player.Submit.triggered && (player.state == PlayerController.State.MOVING || player.state == PlayerController.State.IDLE)){
                Interact();
            }
            if (petting || checkpointInteracting){
                player.state = PlayerController.State.INTERACTIONANIMATION;
            }
        }
    }

    public void Interact()
    {
        // creates temp sphere around player
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, 0.5f);

        // checks all game objects colliders in the area of the players temp sphere
        foreach (var collider in inRangeColliders)
        {
            // checks if the collider's game object is an interactable pickup
            if(collider.gameObject.tag == "InteractablePickup")
            {
                targetedPickup = collider.gameObject;

                // checks if the interactable is the weapon pile
                if (collider.gameObject.name == "Weapon Pile")
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.envLootPickup, this.transform.position);
                }
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isPickup", true);

                // makes player look towards interacted object
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else
                {
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }

                //fade the camera out and in to hide pickup transition
                StartCoroutine(FadeOutAndIn());
                player.state = PlayerController.State.INTERACTIONANIMATION;
            } 
            // checks if the collider's game object is an interactable crouch
            else if(collider.gameObject.tag == "InteractableCrouch")
            {
                collider.enabled = false;
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isPickup", true);

                // makes player look towards interacted object
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else
                {
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }

                //fade the camera out and in to hide pickup transition
                StartCoroutine(FadeOutAndIn());
                player.state = PlayerController.State.INTERACTIONANIMATION;
            }
            // checks if the collider's game object is an interactable story / once (turns off collider which triggers communication to other scripts)
            else if(collider.gameObject.tag == "InteractableStory" | collider.gameObject.tag == "InteractableOnce")
            {
                collider.enabled = false;
            }
            // checks if the collider's game object is a normal interactable
            else if (collider.gameObject.tag == "Interactable")
            {
                collider.enabled = false;
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isHack", true);

                // makes player look towards interacted object
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else
                {
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                player.state = PlayerController.State.INTERACTIONANIMATION;
            }
            // checks if the collider's game object is a checkpoint, and that the checkpoint has NOT already been interacted with
            else if(collider.gameObject.tag == "Checkpoint" && !collider.gameObject.GetComponent<Checkpoint>().active){
                Transform interactTarget = collider.gameObject.transform.Find("Interact Target");
                player.animator.SetBool("isPickup", true);
                checkpointInteracting = true;

                // makes player look towards interacted object
                if(interactTarget != null) {
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }
                else
                {
                    interactTarget = collider.gameObject.transform;
                    player.transform.LookAt(new Vector3(interactTarget.position.x, player.transform.position.y, interactTarget.position.z));
                }

                // dialogue pop-up
                player.state = PlayerController.State.DIALOGUE;
                collider.gameObject.GetComponent<Checkpoint>().ActivateCheckpoint();
                collider.gameObject.GetComponent<Checkpoint>().UseActiveCheckpoint();
            }
            // checks if the collider's game object is a checkpoint, and that the checkpoint has already been interacted with
            else if(collider.gameObject.tag == "Checkpoint" && collider.gameObject.GetComponent<Checkpoint>().active)
            {
                player.state = PlayerController.State.DIALOGUE;
                collider.gameObject.GetComponent<Checkpoint>().UseActiveCheckpoint();
            }
        }
    }

    //anim events for pickups
    private void DeactivatePickup(){
        if (targetedPickup != null){
            targetedPickup.SetActive(false);
        }
    }

    //anim evens for cat
    private void StartPetting(){
        petting = true;
    }

    private void EndPetting(){
        petting = false;
    }

    //anim events for hacking
    private void HackSounds(){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.panelShortCircuit, this.transform.position);
    }

    //quick fade out and in
    public IEnumerator FadeOutAndIn(){

        // sets up image for fading in and out of a black screen
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        bool fadeBackIn = false;
        float fadeSpeed = 1.1f;

        // fades to transparent -> black
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
        // fades black -> transparent
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
