using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: Header("playerWeaponLightStab")]
    [field: SerializeField] public EventReference playerWeaponLightStab { get; private set; }
    [field: Header("playerWeaponSpecial")]
    [field: SerializeField] public EventReference playerWeaponSpecial { get; private set; }
    [field: Header("playerWeaponLight")]
    [field: SerializeField] public EventReference playerWeaponLight { get; private set; }
    [field: Header("playerWeaponHeavy")]
    [field: SerializeField] public EventReference playerWeaponHeavy { get; private set; }
    [field: Header("playerDodge")]
    [field: SerializeField] public EventReference playerDodge { get; private set; }
    [field: Header("playerDeath")]
    [field: SerializeField] public EventReference playerDeath { get; private set; }
    [field: Header("playerWalk")]
    [field: SerializeField] public EventReference playerWalk { get; private set; }

    [field: Header("Deer")]
    [field: Header("deerDeath")]
    [field: SerializeField] public EventReference deerDeath { get; private set; }
    [field: Header("deerAttack")]
    [field: SerializeField] public EventReference deerAttack { get; private set; }
    [field: Header("deerMove")]
    [field: SerializeField] public EventReference deerMove { get; private set; }

    [field: Header("Neutral")]
    [field: Header("minorCut")]
    [field: SerializeField] public EventReference minorCut { get; private set; }
    [field: Header("deathCut")]
    [field: SerializeField] public EventReference deathCut { get; private set; }

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
