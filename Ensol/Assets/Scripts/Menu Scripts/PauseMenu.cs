using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

// ELIZABETH | JUSTIN | JOSEPH | HARSHA
public class PauseMenu : MonoBehaviour
{
    public enum MenuState
    {
        UNPAUSED,
        PAUSED,
        OPTIONS,
        CONTROLS,
        MAP_OPEN,
        MAP_TRANSFER,
        CHECKPOINT
    }

    private Interaction interactionScript;
    public GameObject pauseMenu;
    public Image blackOutSquare;
    public DataPersistanceManager dataManager;
    public GameObject resumeButton;
    public GameObject resumeButtonPT;
    public GameObject playtestMenu;
    public bool amInPlaytestScene = false;
    public GameObject[] enemyPrefabs;
    public PlayerController combatController;
    public static bool isPaused;
    public Transform enemySpawnPoint;
    public NodeSelector nodeSelector;
    [HideInInspector] public MenuState menuState;
    [HideInInspector] public DataPersistanceManager DPM;
    private PlayerInputActions playerInputActions;

    [Header("Map")]
    [SerializeField] private GameObject mapUI;
    [SerializeField] private CompletedNodes completedNodes;

    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject controlsMenu;
    public GameObject firstOption;

    [Header("Checkpoint Menu")]
    [SerializeField] private GameObject checkpointMenu;
    [SerializeField] private GameObject[] checkpointButtons;
    public TextMeshProUGUI checkpointDescription;
    public GameObject cPKeyboard;
    public GameObject cPController;



    private void Start()
    {
        // Set up variables on scene start
        menuState = MenuState.UNPAUSED;
        interactionScript = GameObject.FindWithTag("Player").GetComponent<Interaction>();
        DPM = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistanceManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        isPaused = false;
        Time.timeScale = 1f;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        if (PlayerData.startedGame)
        {
            StartCoroutine(FadeIn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // switch case for each setting in menu
        switch (menuState)
        {
            // unpaused case
            case MenuState.UNPAUSED:
                if (playerInputActions.Player.Cancel.triggered)
                {
                    PauseUnpause();
                }
                else if (playerInputActions.Player.Map.triggered && PlayerData.windowInteracted)
                {
                    OpenMap();
                }
                break;

            // paused case
            case MenuState.PAUSED:
                if (playerInputActions.Player.Cancel.triggered)
                {
                    PauseUnpause();
                }
                break;

            // options case
            case MenuState.OPTIONS:
                if (playerInputActions.Player.Cancel.triggered)
                {
                    OpenCloseOptions();
                }
                break;

            // controls case
            case MenuState.CONTROLS:
                if (playerInputActions.Player.Cancel.triggered)
                {
                    OpenCloseControls();
                }
                break;

            // map opened case
            case MenuState.MAP_OPEN:
                if ((playerInputActions.Player.Map.triggered) || playerInputActions.Player.Cancel.triggered)
                {
                    CloseMap();
                }
                break;

            // map transer case
            case MenuState.MAP_TRANSFER:
                break;

            // menu checkpoint case
            case MenuState.CHECKPOINT:
                if((playerInputActions.Player.Cancel.triggered)) {
                    CloseCheckpointMenu();
                }
                break;
        }
        
        // sets controls for either controller or keyboard
        if (checkpointMenu.activeInHierarchy && CursorToggle.controller){
            cPController.SetActive(true);
            cPKeyboard.SetActive(false);
        } else if (checkpointMenu.activeInHierarchy){
            cPController.SetActive(false);
            cPKeyboard.SetActive(true);
        }
    }

    // checkpoint menu functions
    public void OpenCheckpointMenu() {
        Time.timeScale = 1f;
        checkpointMenu.SetActive(true);
        menuState = MenuState.CHECKPOINT;


        if (PlayerData.currentNode == 0){
            checkpointDescription.text = "You can take this newly revealed service tunnel back to anywhere you have found an access hatch. Where would you like to go?";
        } else {
            checkpointDescription.text = "A smaller panel has been pried open here, revealing a series of service tunnels lined with large pipes and wires. You could use them to travel back and forth anywhere quickly and undetected, if you know where you’re going.";
        }
        
        // sets checkpoints active depending on if player has unlocked them
        for(int i = 0; i < CompletedNodes.checkpoints.Length; i++) {
            if(!CompletedNodes.checkpoints[i]) {
                checkpointButtons[i].SetActive(false);
            }
            else{
                checkpointButtons[i].SetActive(true);
            }
        }

        if(PlayerData.currentNode == 1) {
            checkpointButtons[0].SetActive(false);
        }
        if(PlayerData.currentNode == 6) {
            checkpointButtons[1].SetActive(false);
        }
        if(PlayerData.currentNode == 10) {
            checkpointButtons[2].SetActive(false);
        }
        if(PlayerData.currentNode == 11) {
            checkpointButtons[3].SetActive(false);

        }
    }

    // helper func for opening / closing the controls menu
    public void OpenCloseControls()
    {
        if (controlsMenu.activeInHierarchy)
        {
            controlsMenu.SetActive(false);
            menuState = MenuState.OPTIONS;
        }
        else
        {
            controlsMenu.SetActive(true);
            menuState = MenuState.CONTROLS;
        }
    }

    // helper func for closing a checkpoint menu
    public void CloseCheckpointMenu(){
        interactionScript.checkpointInteracting = false;
        Time.timeScale = 1f;
        checkpointMenu.SetActive(false);
        menuState = MenuState.UNPAUSED;
        combatController.state = PlayerController.State.IDLE;
    }


    public void TransferViaCheckpoint(int nodeDestination) {
        interactionScript.checkpointInteracting = false;
        checkpointMenu.SetActive(false);
        //trigger animations
        combatController.state = PlayerController.State.INTERACTIONANIMATION;
        combatController.animator.SetBool("isPickup", true);
        //delay transition
        StartCoroutine(FadeBlackOutSquare(nodeDestination));
    }
    // map functions
    private void OpenMap() // opens map
    {
        PlayerData.mapOpens += 1;
        menuState = MenuState.MAP_OPEN;
        Time.timeScale = 0f;
        mapUI.SetActive(true);
        completedNodes.LookAtMap();
    }

    private void CloseMap() // closes map
    {
        Time.timeScale = 1f;
        menuState = MenuState.UNPAUSED;       
        completedNodes.StopAllCoroutines();
        mapUI.SetActive(false);
    }

    public void OpenMapForNodeTransfer() // opens the map in the case of a node transfer
    {   
        menuState = MenuState.MAP_TRANSFER;
        Time.timeScale = 0f;
        mapUI.SetActive(true);
        completedNodes.StopAllCoroutines();
        completedNodes.NodeTransferMap();
    }

    public void InstantlyTransferNode()
    {
        completedNodes.LoadNode();
    }

    // allows player to pause or unpause game
    public void PauseUnpause()
    {
        // if the menus arent already active
        if(!pauseMenu.activeInHierarchy && !playtestMenu.activeInHierarchy)
        {
            // sets game to paused and in the playtest scene
            isPaused = true;
            if(amInPlaytestScene) {
                playtestMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButtonPT);
                StartCoroutine(SelectFirstChoice(resumeButtonPT));
                Time.timeScale = 0f;
            }
            // sets the game to paused
            else {
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButton);
                StartCoroutine(SelectFirstChoice(resumeButton));
                Time.timeScale = 0f;
            }
            menuState = MenuState.PAUSED;
        }
        // if the menus are already active, closes them and resumes play
        else
        {
            isPaused = false;
            playtestMenu.SetActive(false);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            menuState = MenuState.UNPAUSED;
        }
    }

    // opens and closes the options menu
    public void OpenCloseOptions()
    {
        // if options is already active
        if (optionsMenu.activeInHierarchy)
        {
            // closes options menu
            optionsMenu.SetActive(false);
            if (resumeButton.activeInHierarchy){
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButton);
            } else {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButtonPT);
            }
            menuState = MenuState.PAUSED;
        }
        // if options menu is not already active, opens it
        else
        {
            optionsMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstOption);
            menuState = MenuState.OPTIONS;
        }
    }

    // exits game to menu
    public void ExitToMenu()
    {
        Time.timeScale = 1f;

        //Save stuff for enemy manager
        GameObject transferCube = GameObject.Find("Entrance");
        if (transferCube)
        {
            SceneSwitch sceneSwitcher = transferCube.GetComponent<SceneSwitch>();
            if (sceneSwitcher)
            {
                sceneSwitcher.SetTimeAtNode();
                sceneSwitcher.SetEnemiesDefeated();
            }
        }
        DPM.SaveGame();
        SceneManager.LoadScene(sceneName:"MenuScene");
        amInPlaytestScene = false;
    }

    // returns player to node select scene
    public void ReturnToNodeSelect() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName:"MapScene");
    }

    // returns player to the cabin
    public void ReturnToCabin() {
        //Save stuff for enemy manager
        GameObject transferCube = GameObject.Find("Entrance");
        if (transferCube)
        {
            SceneSwitch sceneSwitcher = transferCube.GetComponent<SceneSwitch>();
            if (sceneSwitcher)
            {
                SpawnPoint.Mapped = true;
                sceneSwitcher.SetTimeAtNode();
                sceneSwitcher.SetEnemiesDefeated();
            }
        }
        StartCoroutine(ReturnToCabinFadeout());
    }

    // fadeout sequence that starts when player returns to the cabin
    public IEnumerator ReturnToCabinFadeout()
    {
        // closes any open menus
        if (menuState == MenuState.PAUSED)
        {
            PauseUnpause();
        }
        if (menuState == MenuState.MAP_OPEN)
        {
            CloseMap();
        }
        
        // sets up the image that will be fading to black
        Color objectColor = blackOutSquare.color;
        blackOutSquare.color = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
        float fadeAmount;
        float fadeSpeed = 1.1f;

        // slowly fades set up image to black
        while (blackOutSquare.color.a < 1)
        {
            fadeAmount = blackOutSquare.color.a + (fadeSpeed * Time.unscaledDeltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.color = objectColor;
            yield return null;
        }

        PlayerData.prevNode = PlayerData.currentNode;
        PlayerData.currentNode = 1;
        nodeSelector.OpenScene();
    }

    // opposite of the function above
    public IEnumerator FadeIn()
    {
        // sets up the image that will be fading to transparency
        Color objectColor = blackOutSquare.color;
        blackOutSquare.color = new Color(objectColor.r, objectColor.g, objectColor.b, 1);
        float fadeAmount;
        float fadeSpeed = 1f;

        // slowly fades set up image to transparent
        while (blackOutSquare.color.a > 0)
        {
            fadeAmount = blackOutSquare.color.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.color = objectColor;
            yield return null;
        }
    }


    // enters playtesting menu
    public void EnterPlaytestMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "PlaytestingScene");
        InitializePlatestVars();
    }

    // sets up the variables required to playtest efficiently (unlocks all weps)
    public void InitializePlatestVars()
    {
        amInPlaytestScene = true;

        PlayerData.hasBroom = true;
        PlayerData.hasSolarUpgrade = true;
        PlayerData.hasThrowUpgrade = true;
        PlayerData.currentlyHasBroom = true;
        PlayerData.currentlyHasSolar = true;
    }

    // -------- Playtesting menu functions --------
    public void ToggleHeavyAttack(bool toggle) {
        if (!PlayerData.hasSolarUpgrade)
        {
            combatController.TestPickedUpSolarUpgrade();
        }
        else
        {
            combatController.RemoveSolarUpgrade();
        }     
    }

    // toggles throw attack
    public void ToggleThrowAttack(bool toggle) {
        if (!PlayerData.hasThrowUpgrade)
        {
            combatController.PickedUpThrowUpgrade();
        }
        else
        {        
            combatController.RemoveThrowUpgrade();
        }
    }

    // toggles shield
    public void ToggleShield(bool toggle) {
        if (PlayerData.hasShield)
        {
            PlayerData.hasShield = false;
        }
        else
        {
            PlayerData.hasShield = true;
        }
        
    }

    // spawn in a deer
    public void SpawnDeer()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].name == "Deer")
            {
                Instantiate(enemyPrefabs[i], enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
    }

    // spawn in a bear
    public void SpawnBear()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].name == "Bear")
            {
                Instantiate(enemyPrefabs[i], enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
    }

    // spawn in a rabbit
    public void SpawnRabbit()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].name == "Rabbit")
            {
                Instantiate(enemyPrefabs[i], enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
    }

    // spawn in a spider
    public void SpawnSpider()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].name == "Spider")
            {
                Instantiate(enemyPrefabs[i], enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
    }

    // spawn in a mini spider
    public void SpawnMiniSpider()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].name == "Mini Spider")
            {
                Instantiate(enemyPrefabs[i], enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
    }

    // spawn in a CRACK DEER
    public void SpawnCrackDeer()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].name == "Crack Deer Variant")
            {
                Instantiate(enemyPrefabs[i], enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
    }

    private IEnumerator SelectFirstChoice(GameObject selectedButton)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(selectedButton);
    }

    public IEnumerator FadeBlackOutSquare(int nodeDestination) // function to slowly fade the screen to black and load map scene
    {
        Color objectColor = blackOutSquare.color;
        float fadeAmount = 0;
        int fadeSpeed = 1;
        while(blackOutSquare.color.a < 1)
        {      
            fadeAmount += fadeSpeed * Time.unscaledDeltaTime;
            blackOutSquare.color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }
        PlayerData.prevNode = PlayerData.currentNode;
        PlayerData.currentNode = nodeDestination;
        OutdoorLevelManager.isCheckpointTransition = true;
        OpenMapForNodeTransfer();     
    }
}
