using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingspeed = 0.024f;


    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private Coroutine displaylineCoroutine;
    private bool canContinuetoNextLine = false;


    public PlayerController charController;
    public KeyCode _key;

    public bool donePlaying;
    public bool openSesame;


    private Story currentStory;

    public bool dialogueisPlaying { get; private set; }

    private static DialogueManager instance;

    private static DialogueVariables dialogueVariables;

    //for cat meowing dialogue

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than on Dialogue Manager in the scene");
        }
        instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueisPlaying = false;
        donePlaying = false;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        /*if (NodeSelector.selectedNode == 1){
            Debug.Log("should meow");
            meower = GameObject.FindGameObjectWithTag("Meower");
            print(meower);
        }*/

        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueisPlaying)
        {
            return;
        }
        /*Allow e and mouse to continue dialogue if there are no more choices*/
        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0)) && choicesPanel.activeInHierarchy == false && canContinuetoNextLine)
        {
            if (PlayerData.startedGame == false){
                PlayerData.startedGame = true;
            }
            StartCoroutine(Delay());
        }
        
    }

    /*allow mouse or outside source to select option*/
    public void ClickChoice()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay(){
        yield return new WaitForSeconds(0.1f);
        ContinueStory();
    } 

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueisPlaying = true;
        charController.state = PlayerController.State.DIALOGUE;
        dialoguePanel.SetActive(true);
        
        dialogueVariables.StartListening(currentStory);

        //functions called by the dialogue//

        currentStory.BindExternalFunction("openDoor", () => {
            openSesame = true;
        });

        currentStory.BindExternalFunction("meowing", () => {
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.catMeow, meower.transform.position);
        });

        currentStory.BindExternalFunction("hackRiver", () => {
            //hack river
            Debug.Log("hacking river");
        });

        currentStory.BindExternalFunction("endingOne", () => {
            //everything shuts down
            //go to credits
            Debug.Log("ending 1");
        });

        currentStory.BindExternalFunction("endingTwo", () => {
            //player erases memory
            //Plush starting dialogue plays again
            //go to credits
            Debug.Log("ending two");
        });

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        // currentStory.UnbindExternalFunction("openDoor");
        currentStory.UnbindExternalFunction("meowing");
        dialogueisPlaying = false;
        donePlaying = true;
        charController.state = PlayerController.State.IDLE;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
        if (dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
            print("saved");
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        //Check so Lines don't overwrite themselves
        {
            if (displaylineCoroutine != null)
            {
                StopCoroutine(displaylineCoroutine);
            }
            // set text for current dialogue line
            // display choices, if any, for this dialogue line
            displaylineCoroutine = StartCoroutine(textScroll(currentStory.Continue()));

        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        var dialoguePos = dialoguePanel.GetComponent<RectTransform>();
        var curPos = dialoguePos.anchoredPosition;
        if (currentChoices.Count > 0)
        {
            //print("hello from line 137");

            dialoguePos.anchoredPosition = new Vector2(curPos.x, 385.93f);
            choicesPanel.SetActive(true);
        }
        else
        {
            dialoguePos.anchoredPosition = new Vector2(curPos.x, 185.93f);
            choicesPanel.SetActive(false);
        }

        // checks to see if UI can support number of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);

        }

        int index = 0;

        // enables and initializes the choices up to the amount of choices for this dialogue
        foreach(Choice choice in currentChoices)
        {
            
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //print(index);
        // sets unneccesary choices in UI to hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());

    }

    private IEnumerator SelectFirstChoice()
    {
        //print("hello from line 177");
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        //print("made a choice");
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    public static bool GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null" + variableName);
        }
        // return variableValue;
        bool b = (bool) variableValue;
        return b;
    }


    private IEnumerator textScroll(string text)
    {
        print ("start scrolling");
        dialogueText.text = "";
        canContinuetoNextLine = false;
        int timer = 0;
        int timeLimit = 2;
        DisplayChoices();
        hideChoices();
         
          //for each letter one at a time
         foreach (char letter in text.ToCharArray())
         {
            if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0) || Input.GetKeyDown(_key)) && timer >= timeLimit)
            {
                print ("trying to skip");
                dialogueText.text = text;
                break;
            }
            timer += 1;
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingspeed);
         }
         canContinuetoNextLine = true;
         DisplayChoices();

    }
    private void hideChoices()
    {
        foreach (GameObject choicebutton in choices)
        {
            choicebutton.SetActive(false);
        }
    }
}
