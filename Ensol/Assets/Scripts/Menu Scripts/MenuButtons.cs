using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    public GameObject playButton, controlsButton, exitButton;
    public NodeSelector nodeSelector;
    public DataPersistanceManager GameManager;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
        Cursor.visible = true;
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
