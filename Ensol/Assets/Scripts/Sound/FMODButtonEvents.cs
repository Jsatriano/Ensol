using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODButtonEvents : MonoBehaviour
{
    [field: Header("Universal Buttons")]
    [field: SerializeField] public EventReference envbeepboop { get; private set; }
    

    public static FMODButtonEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }
}
