using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("playerWeaponLightStab")]
    [field: SerializeField] public EventReference playerWeaponLightStabEvent { get; private set; }
    [field: Header("playerWeaponSpecial")]
    [field: SerializeField] public EventReference playerWeaponSpecialEvent { get; private set; }
    [field: Header("playerWeaponLight")]
    [field: SerializeField] public EventReference playerWeaponLightEvent { get; private set; }
    [field: Header("playerWeaponHeavy")]
    [field: SerializeField] public EventReference playerWeaponHeavyEvent { get; private set; }
    [field: Header("playerDodge")]
    [field: SerializeField] public EventReference playerDodgeEvent { get; private set; }
    [field: Header("playerDeath")]
    [field: SerializeField] public EventReference playerDeathEvent { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }
}
