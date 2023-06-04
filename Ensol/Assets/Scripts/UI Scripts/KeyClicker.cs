using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyClicker : MonoBehaviour
{
    public KeyCode _key;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject && Input.GetKeyDown(_key)){
            //print("button pressed by E");
            _button.onClick.Invoke();
        }
    }
}

// public class KeyClicker : MonoBehaviour, ISelectHandler
// {
//     [Header("KeyCodes")]
//     public KeyCode _key1;
//     public KeyCode _key2;

//     private Button _button;

//     public static GameObject highlightedChoice;

//     void Awake()
//     {
//         _button = GetComponent<Button>();
//     }

//     public void OnSelect(BaseEventData eventData)
//     {
//         highlightedChoice = this.gameObject;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(highlightedChoice == this.gameObject && (Input.GetKeyDown(_key1) || Input.GetKeyDown(_key2))){
//             //print("button pressed by E");
//             _button.onClick.Invoke();
//         }
//     }
// }