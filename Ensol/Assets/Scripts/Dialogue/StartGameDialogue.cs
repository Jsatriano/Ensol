using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameDialogue : MonoBehaviour
{
    public Collider dialogueCollider;
    public DialogueTrigger dialogueTrigger;
    public GameObject blackOutSquare;
    public bool finishedFading;
    private bool clickedDialogue = false;
    private bool died = false;
    private float fadeSpeed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen"); // Gets black out square game object to pass it through scenes
        }
        //check if started game and play acompanying dialogue and fade sequence
        if (PlayerData.startedGame == false){
            dialogueTrigger.enabled = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.catMeow, this.transform.position);
            finishedFading = false;
            Color blackColor = blackOutSquare.GetComponent<Image>().color;
            blackOutSquare.GetComponent<Image>().color = new Color(blackColor.r, blackColor.g, blackColor.b, 1);
            //print("case1");
        }
        //check if player is respawning after a death
        if(PlayerData.startedGame == true && PlayerData.currentlyHasBroom == false){
            finishedFading = false;
            Color blackColor = blackOutSquare.GetComponent<Image>().color;
            blackOutSquare.GetComponent<Image>().color = new Color(blackColor.r, blackColor.g, blackColor.b, 1);
            fadeSpeed = 0.8f;
            //died = true;
            //print("case2");
        }
    }

    void Update(){
        if(PlayerData.startedGame == true && finishedFading == false && clickedDialogue == false) 
        {
            clickedDialogue = true;
            StartCoroutine(ReverseFadeBlackOutSquare());
            //print("resolution1");
        } /*else if (finishedFading == false && died == true){
            died = false;
            StartCoroutine(ReverseFadeBlackOutSquare());
            print("resolution2");
        }*/
    }

    public void OnTriggerEnter(Collider hitter){

        //trigger the dialogue
        if (hitter.tag == "Player" && PlayerData.startedGame == false){
            //DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            dialogueCollider.enabled = false;

        }
        
    }

    public IEnumerator ReverseFadeBlackOutSquare()
    {

        yield return new WaitForSeconds(1f);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;

        if(fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            finishedFading = true;
        }
    }
}
