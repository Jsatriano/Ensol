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
    private Coroutine fadeRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        if(blackOutSquare == null){
            blackOutSquare = GameObject.Find("Black Out Screen"); // Gets black out square game object to pass it through scenes
        }
        if (PlayerData.startedGame == false){
            dialogueTrigger.enabled = true;
            finishedFading = false;
            Color blackColor = blackOutSquare.GetComponent<Image>().color;
            blackOutSquare.GetComponent<Image>().color = new Color(blackColor.r, blackColor.g, blackColor.b, 1);
        }
    }

    void Update(){
        if(fadeRoutine == null && PlayerData.startedGame == false && finishedFading == false) 
        {
            Debug.Log("this is happening");
            fadeRoutine = StartCoroutine(ReverseFadeBlackOutSquare());
        }
    }

    public void OnTriggerEnter(Collider hitter){
        
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
        float fadeSpeed = 0.5f;

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
