using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public GameObject playtestMenu;
    public bool amInPlaytestScene = false;
    public GameObject[] EnemyPrefabs;



    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if(!pauseMenu.activeInHierarchy && !playtestMenu.activeInHierarchy)
        {
            if(amInPlaytestScene) {
                playtestMenu.SetActive(true);
                Time.timeScale = 0f;
                print("enabled playtest scene");
            }
            else {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else
        {
            playtestMenu.SetActive(false);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            print("it should be unpaused now");
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

    public void EnterPlaytestMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "PlaytestingScene");
        amInPlaytestScene = true;

        PlayerData.hasBroom = true;
        PlayerData.hasHeavyAttack = true;
        PlayerData.hasSolarUpgrade = true;
        PlayerData.diedToCrackDeer = true;
        PlayerData.hasHeavyAttack = true;
    }

    //Playtesting menu functions
    public void ToggleHeavyAttack(bool toggle) {
        PlayerData.hasHeavyAttack = toggle;
    }

    public void ToggleThrowAttack(bool toggle) {
        PlayerData.hasThrowUpgrade = toggle;
    }

    public void ToggleShield(bool toggle) {
        PlayerData.hasShield = toggle;
    }

}
