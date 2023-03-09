using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    
    private void Start()
    {
        PlayerData.hasBroom = false;
        PlayerData.currentNode = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
