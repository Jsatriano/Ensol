using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODMusicEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference zoneMusic { get; private set; }
    

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
