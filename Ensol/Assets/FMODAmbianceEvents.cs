using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODAmbianceEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference zoneAmbiance { get; private set; }
    [field: SerializeField] public EventReference zoneAmbiance2 { get; private set; }
    

    public static FMODAmbianceEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }
}
