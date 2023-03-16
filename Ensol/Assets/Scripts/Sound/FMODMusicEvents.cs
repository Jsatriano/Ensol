using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODMusicEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: Header("zone1")]
    [field: SerializeField] public EventReference zone1 { get; private set; }
    [field: Header("zone2")]
    [field: SerializeField] public EventReference zone2 { get; private set; }
    [field: Header("zone3")]
    [field: SerializeField] public EventReference zone3 { get; private set; }
    [field: Header("Menu")]
    [field: SerializeField] public EventReference mainMenu { get; private set; }
    [field: Header("Cabin")]
    [field: SerializeField] public EventReference cabin { get; private set; }
    

    public static FMODMusicEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }
}
