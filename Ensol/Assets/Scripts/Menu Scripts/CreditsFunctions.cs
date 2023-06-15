using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CreditsFunctions : MonoBehaviour
{

    public GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenu()
    {
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        SceneManager.LoadScene(sceneName:"MenuScene");
    }
}
