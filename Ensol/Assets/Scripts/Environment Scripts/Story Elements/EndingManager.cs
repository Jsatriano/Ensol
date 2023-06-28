using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class EndingManager : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    public GameObject blackOutSquare;
    public GameObject computerObject;
    public GameObject player;
    public bool killAmbiance = false;
    public bool killMusic = false;
    private string sceneName;
    private Coroutine fadeRoutine = null;
    private bool gameEnding = false;
    private bool gameEndingOne;
    private bool dialoguePlayed = false;
    private bool doneFading = false;
    public DataPersistanceManager DPM;

    // Start is called before the first frame update
    void Start()
    {
        blackOutSquare = GameObject.Find("Black Out Screen");
        sceneName = SceneManager.GetActiveScene().name;  
        DPM = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistanceManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //takes player to credits after exiting dialogue
        if (dialoguePlayed && !DialogueManager.GetInstance().dialogueisPlaying){
            PlayerData.prevNode = 11;
            DPM.SaveGame();
            SceneManager.LoadScene(sceneName:"CreditScene");
        }
        if (gameEnding && doneFading == false){
            StartCoroutine(FadeBlackOutSquare());
        }
        
    }

    public void EndOne(){
        
        gameEndingOne = true;
        gameEnding = true;
        killMusic = true;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.computerShutdown, computerObject.transform.position);
        killAmbiance = true;
        PlayerData.beatenGame = true;
    }

    public void EndTwo(){
        gameEndingOne = false;
        gameEnding = true;
        killMusic = true;
        PlayerData.beatenGame = true;
    }

    public IEnumerator FadeBlackOutSquare()
    {
        yield return new WaitForSeconds(0.45f);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        float fadeSpeed = 0.1f;

        if (fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                player.GetComponent<PlayerController>().state = PlayerController.State.INTERACTIONANIMATION;
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a >= 1)
                {
                    doneFading = true;
                    //Takes player to credits
                    if (gameEndingOne){
                        yield return new WaitForSeconds(2.5f);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.envComputerDoor, player.transform.position);
                        yield return new WaitForSeconds(1.3f);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.envComputerUp, player.transform.position);
                        yield return new WaitForSeconds(1.5f);
                        PlayerData.prevNode = 11;
                        DPM.SaveGame();
                        SceneManager.LoadScene(sceneName:"CreditScene");
                    } else {
                        killAmbiance = true;
                        yield return new WaitForSeconds(2f);
                        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.catMeow, this.transform.position);
                        dialoguePlayed = true;
                    }
                }
                yield return null;
            }
        }
    }
}
