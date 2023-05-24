using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public enum MenuState
    {
        UNPAUSED,
        PAUSED,
        OPTIONS
    }

    public GameObject pauseMenu;
    public DataPersistanceManager dataManager;
    public GameObject resumeButton;
    public GameObject playtestMenu;
    public bool amInPlaytestScene = false;
    public GameObject[] enemyPrefabs;
    public PlayerController combatController;
    public static bool isPaused;
    public Transform enemySpawnPoint;
    public NodeSelector nodeSelector;
    [HideInInspector] public MenuState menuState;

    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        menuState = MenuState.UNPAUSED;
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
        }
    }

    public void PauseUnpause()
    {
        if(!pauseMenu.activeInHierarchy && !playtestMenu.activeInHierarchy)
        {
            isPaused = true;
            if(amInPlaytestScene) {
                playtestMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            else {
                pauseMenu.SetActive(true);
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
        SceneManager.LoadScene(sceneName:"MenuScene");
        amInPlaytestScene = false;
    }

    public void ReturnToNodeSelect() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName:"MapScene");
    }

    public void ReturnToCabin() {
        StartCoroutine(ReturnToCabinFadeout());
    }

    public IEnumerator ReturnToCabinFadeout()
    {
        PauseUnpause();
        GameObject blackOutSquare = GameObject.Find("Black Out Screen");
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        bool fadeToBlack = true;
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
}
