using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyClicker : MonoBehaviour, ISelectHandler
{
    public KeyCode _key;

    private Button _button;

    public static GameObject highlightedChoice;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        highlightedChoice = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(highlightedChoice == this.gameObject && Input.GetKeyDown(_key)){
            //print("button pressed by E");
            _button.onClick.Invoke();
        }
    }
}
