using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    public GameObject playButton, controlsButton, exitButton, firstOptions, newGameQuery, loadButton;
    public NodeSelector nodeSelector;
    public DataPersistanceManager GameManager;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
        Cursor.visible = true;
        print (PlayerData.startedGame);
        if (PlayerData.startedGame == false){
            Color fadedColor = loadButton.GetComponent<Image>().color;
            fadedColor = new Color(fadedColor.r, fadedColor.g, fadedColor.b, 0.3f);
            loadButton.GetComponent<Image>().color = fadedColor;
            loadButton.GetComponent<Button>().interactable = false;
        }
    }

    public void update()
    {

    }

    public void LoadGame()
    {
        // will need to change this to the cabin once its created
        //SceneManager.LoadScene(sceneName:"MapScene");
        SceneSwitch.exitFrom = true;
        nodeSelector.OpenScene();
    }

    public void NGame()
    {
        // will need to change this to the cabin once its created
        //SceneManager.LoadScene(sceneName:"MapScene");
        GameManager.ClearGame();
        SceneSwitch.exitFrom = true;
        nodeSelector.OpenScene();
    }

    public void NGQuery()
    {
        //check if the player is sure
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        firstOptions.SetActive(false);
        newGameQuery.SetActive(true);
    }

    public void NGChangeMind()
    {
        //go back if player is unsure
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        firstOptions.SetActive(true);
        newGameQuery.SetActive(false);
    }

    public void Controls()
    {
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        SceneManager.LoadScene(sceneName:"ControlScene");
    }

    public void ExitGame()
    {
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        Application.Quit();
    }
}
