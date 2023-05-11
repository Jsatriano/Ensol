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

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;


    public PlayerController charController;

    public bool donePlaying;
    public bool openSesame;


    private Story currentStory;

    public bool dialogueisPlaying { get; private set; }

    private static DialogueManager instance;

    private static DialogueVariables dialogueVariables;

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
        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0)) && choicesPanel.activeInHierarchy == false)
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
        yield return new WaitForSeconds(0.0001f);
        ContinueStory();
    } 

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueisPlaying = true;
        charController.state = PlayerController.State.DIALOGUE;
        dialoguePanel.SetActive(true);
        
        dialogueVariables.StartListening(currentStory);

        currentStory.BindExternalFunction("openDoor", () => {
            Debug.Log("opening Door!!!!!!!!!!!!!!!!!!");
            openSesame = true;
        });

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        currentStory.UnbindExternalFunction("openDoor");
        dialogueisPlaying = false;
        donePlaying = true;
        charController.state = PlayerController.State.IDLE;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
        dialogueText.text = "";
        if (dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
            print("saved");
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //print("continue story tried to continue");
            // set text for current dialogue line
            dialogueText.text = currentStory.Continue();
            // display choices, if any, for this dialogue line
            DisplayChoices();
        }
        else
        {
            //print("continue story tried to exit");
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
}
