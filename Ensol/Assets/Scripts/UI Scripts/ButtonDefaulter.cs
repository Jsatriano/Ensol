using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDefaulter : MonoBehaviour
{

    //private bool buttonSet = false;
    public GameObject buttonActiveCheck;
    public GameObject buttonTwoActiveCheck;

    // Start is called before the first frame update
    void Update()
    {
        print(EventSystem.current.currentSelectedGameObject);
    }

    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
    //check if the first button option is available, and set it or the next button active
        yield return new WaitForEndOfFrame();
        if (buttonActiveCheck.activeInHierarchy){
            print("button1");
            StartCoroutine(SelectFirstChoice(buttonActiveCheck));
        } else if (buttonTwoActiveCheck.activeInHierarchy){
            print("button2");
            StartCoroutine(SelectFirstChoice(buttonTwoActiveCheck));
        }
    }

    private IEnumerator SelectFirstChoice(GameObject theButton)
    {
        print("hello from line 177");
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(theButton.gameObject);
        print(EventSystem.current.currentSelectedGameObject);
    }
}
