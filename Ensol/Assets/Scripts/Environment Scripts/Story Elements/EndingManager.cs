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
        print("ambiance is "+killAmbiance);
        killAmbiance = true;
        print("ambiance is "+killAmbiance);
    }

    public void EndTwo(){
        gameEndingOne = false;
        gameEnding = true;
        killMusic = true;
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
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a >= 1)
                {
                    doneFading = true;
                    //Takes player to credits
                    if (gameEndingOne){
                        PlayerData.prevNode = 11;
                        DPM.SaveGame();
                        SceneManager.LoadScene(sceneName:"CreditScene");
                    } else {
                        killAmbiance = true;
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
