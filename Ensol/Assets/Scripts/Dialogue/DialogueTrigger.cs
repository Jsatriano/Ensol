using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    public Collider c;

    private void Update()
    {
        if(c.enabled == false) 
        {
            Debug.Log(inkJSON.text);
        }
    }
}
