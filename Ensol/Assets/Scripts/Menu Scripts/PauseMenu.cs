using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public enum MenuState
    {
        UNPAUSED,
        PAUSED,
        OPTIONS,
        MAP_OPEN,
        MAP_TRANSFER,
        CHECKPOINT
    }

    public GameObject pauseMenu;
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

    [Header("Map")]
    [SerializeField] private GameObject mapUI;
    [SerializeField] private CompletedNodes completedNodes;

    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenu;

    [Header("Checkpoint Menu")]
    [SerializeField] private GameObject checkpointMenu;
    [SerializeField] private GameObject[] checkpointButtons;
    public TextMeshProUGUI checkpointDescription;

    private void Start()
    {
        menuState = MenuState.UNPAUSED;
        DPM = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistanceManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        isPaused = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState)
        {
            case MenuState.UNPAUSED:
                if (Input.GetButtonDown("Cancel"))
                {
                    PauseUnpause();
                }
                else if (Input.GetButtonDown("Map") && PlayerData.windowInteracted)
                {
                    OpenMap();
                }
                break;

            case MenuState.PAUSED:
                if (Input.GetButtonDown("Cancel"))
                {
                    PauseUnpause();
                }
                break;

            case MenuState.OPTIONS:
                if (Input.GetButtonDown("Cancel"))
                {
                    OpenCloseOptions();
                }
                break;

            case MenuState.MAP_OPEN:
                if ((Input.GetButtonDown("Map") || Input.GetButtonDown("Cancel")))
                {
                    CloseMap();
                }
                break;

            case MenuState.MAP_TRANSFER:
                break;

            case MenuState.CHECKPOINT:
                if((Input.GetButtonDown("Cancel"))) {
                    CloseCheckpointMenu();
                }
                break;
        }
    }

    //checkpoint menu functions

    public void OpenCheckpointMenu() {
        Time.timeScale = 1f;
        checkpointMenu.SetActive(true);
        menuState = MenuState.CHECKPOINT;

        if (PlayerData.currentNode == 0){
            checkpointDescription.text = "You can take this newly revealed service tunnel back to anywhere you have found an access hatch. Where would you like to go?";
        } else {
            checkpointDescription.text = "A smaller panel has been pried open here, revealing a series of service tunnels lined with large pipes and wires. You could use them to travel back and forth anywhere quickly and undetected, if you know where you’re going.";
        }
        
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
        if(PlayerData.currentNode == 12) {
            checkpointButtons[3].SetActive(false);

        }
    }

    public void CloseCheckpointMenu(){
        Time.timeScale = 1f;
        checkpointMenu.SetActive(false);
        menuState = MenuState.UNPAUSED;
        combatController.state = PlayerController.State.IDLE;
    }

    public void TransferViaCheckpoint(int nodeDestination) {
        checkpointMenu.SetActive(false);
        PlayerData.prevNode = PlayerData.currentNode;
        PlayerData.currentNode = nodeDestination;
        OutdoorLevelManager.isCheckpointTransition = true;
        OpenMapForNodeTransfer();
    }
    // map functions
    private void OpenMap()
    {
        PlayerData.mapOpens += 1;
        menuState = MenuState.MAP_OPEN;
        Time.timeScale = 0f;
        mapUI.SetActive(true);
        completedNodes.LookAtMap();
    }

    private void CloseMap()
    {
        Time.timeScale = 1f;
        menuState = MenuState.UNPAUSED;       
        completedNodes.StopAllCoroutines();
        mapUI.SetActive(false);
    }

    public void OpenMapForNodeTransfer()
    {   
        menuState = MenuState.MAP_TRANSFER;
        Time.timeScale = 0f;
        mapUI.SetActive(true);
        completedNodes.StopAllCoroutines();
        completedNodes.NodeTransferMap();
    }

    public void PauseUnpause()
    {
        if(!pauseMenu.activeInHierarchy && !playtestMenu.activeInHierarchy)
        {
            isPaused = true;
            if(amInPlaytestScene) {
                playtestMenu.SetActive(true);
                StartCoroutine(SelectFirstChoice(resumeButtonPT));
                Time.timeScale = 0f;
            }
            else {
                pauseMenu.SetActive(true);
                StartCoroutine(SelectFirstChoice(resumeButton));
                Time.timeScale = 0f;
            }
            menuState = MenuState.PAUSED;
        }
        else
        {
            isPaused = false;
            playtestMenu.SetActive(false);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            menuState = MenuState.UNPAUSED;
        }
    }

    public void OpenCloseOptions()
    {
        if (optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(false);
            menuState = MenuState.PAUSED;
        }
        else
        {
            optionsMenu.SetActive(true);
            menuState = MenuState.OPTIONS;
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        DPM.SaveGame();
        SceneManager.LoadScene(sceneName:"MenuScene");
        amInPlaytestScene = false;
    }

    public void ReturnToNodeSelect() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName:"MapScene");
    }

    public void ReturnToCabin() {
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
        StartCoroutine(ReturnToCabinFadeout());
    }

    public IEnumerator ReturnToCabinFadeout()
    {
        if (menuState == MenuState.PAUSED)
        {
            PauseUnpause();
        }
        if (menuState == MenuState.MAP_OPEN)
        {
            CloseMap();
        }
        GameObject blackOutSquare = GameObject.Find("Black Out Screen");
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
        float fadeSpeed = 1.1f;

        if(fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.unscaledDeltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                if(blackOutSquare.GetComponent<Image>().color.a >= 1)
                {
                    PlayerData.prevNode = PlayerData.currentNode;
                    PlayerData.currentNode = 1;
                    nodeSelector.OpenScene();
                }
                yield return null;
            }
        }
    }

    public void EnterPlaytestMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "PlaytestingScene");
        InitializePlatestVars();
    }

    public void InitializePlatestVars()
    {
        amInPlaytestScene = true;

        PlayerData.hasBroom = true;
        PlayerData.hasSolarUpgrade = true;
        PlayerData.hasThrowUpgrade = true;
        PlayerData.currentlyHasBroom = true;
        PlayerData.currentlyHasSolar = true;
    }

    //Playtesting menu functions
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
        //print("hello from line 177");
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(selectedButton);
    }
}
