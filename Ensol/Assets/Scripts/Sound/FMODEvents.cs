using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerWeaponLightStab { get; private set; }
    [field: SerializeField] public EventReference playerWeaponSpecial { get; private set; }
    [field: SerializeField] public EventReference playerWeaponSpecialPrep { get; private set; }
    [field: SerializeField] public EventReference playerWeaponSpecialReturn { get; private set; }
    [field: SerializeField] public EventReference playerWeaponSpecialThunk { get; private set; }
    [field: SerializeField] public EventReference playerWeaponLight { get; private set; }
    [field: SerializeField] public EventReference playerWeaponHeavy { get; private set; }
    [field: SerializeField] public EventReference playerWeaponHeavyPrep { get; private set; }
    [field: SerializeField] public EventReference playerDodge { get; private set; }
    [field: SerializeField] public EventReference playerDeath { get; private set; }
    [field: SerializeField] public EventReference playerWalk { get; private set; }
    [field: SerializeField] public EventReference playerSpin { get; private set; }

    [field: Header("Deer")]
    [field: SerializeField] public EventReference deerDeath { get; private set; }
    [field: SerializeField] public EventReference deerAttack { get; private set; }
    [field: SerializeField] public EventReference deerMove { get; private set; }
    [field: SerializeField] public EventReference deerAlerted { get; private set; }
    [field: SerializeField] public EventReference deerGutted { get; private set; }

    [field: Header("Bear")]
    [field: SerializeField] public EventReference BearDeath { get; private set; }
    [field: SerializeField] public EventReference BearSwipe { get; private set; }
    [field: SerializeField] public EventReference BearThrow { get; private set; }
    [field: SerializeField] public EventReference BearMove { get; private set; }
    [field: SerializeField] public EventReference BearAlerted { get; private set; }
    [field: SerializeField] public EventReference BearScrapExplosion { get; private set; }

    [field: Header("Neutral")]
    [field: SerializeField] public EventReference minorCut { get; private set; }
    [field: SerializeField] public EventReference deathCut { get; private set; }
    [field: SerializeField] public EventReference envGateOpen { get; private set; }
    [field: SerializeField] public EventReference envBroomBreak { get; private set; }
    [field: SerializeField] public EventReference hudMapOpen { get; private set; }
    [field: SerializeField] public EventReference hudBatteryCharge { get; private set; }
    [field: SerializeField] public EventReference catMeow { get; private set; }

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
