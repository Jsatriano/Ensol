using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RecloneScreen : MonoBehaviour
{
    public GameObject recloneButton;
    public NodeSelector nodeSelector;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(recloneButton);
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        // Respawns at cabin
        nodeSelector.node = 1;
        nodeSelector.OpenScene();
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName:"MenuScene");
    }
}
