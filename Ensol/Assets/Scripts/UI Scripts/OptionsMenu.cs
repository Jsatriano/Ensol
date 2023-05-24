using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Slider musicSlider;
    public static float musicValue = 1;
    [SerializeField] private Slider sfxSlider;
    public static float sfxValue = 1;
    [SerializeField] private Slider screenShakeSlider;
    public static float screenShakeValue = 1;
    [SerializeField] private Toggle catModeToggle;
    public static bool catModeActivated = false;

    [Header("VCAs")]
    private FMOD.Studio.VCA musicVCA;
    private FMOD.Studio.VCA sfxVCA;



    //Unity Events
    [HideInInspector] public UnityEvent<float> OnScreenShakeChange;
    [HideInInspector] public UnityEvent<bool> OnCatModeChange;

    private void Start()
    {
        //Sets current values
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
        screenShakeSlider.value = screenShakeValue;
        catModeToggle.isOn = catModeActivated;

        musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        sfxVCA   = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");

    }

    public void UpdateScreenShake()
    {
        screenShakeValue = screenShakeSlider.value;
        OnScreenShakeChange.Invoke(screenShakeValue);
    }

    public void UpdateMusicVolume()
    {
        musicValue = musicSlider.value;
        musicVCA.setVolume(musicValue);
    }

    public void UpdateSFXVolume()
    {
        sfxValue = sfxSlider.value;
        sfxVCA.setVolume(sfxValue);
    }

    public void UpdateCatMode()
    {
        Debug.Log("CATMODeeee");
        catModeActivated = catModeToggle.isOn;
        OnCatModeChange.Invoke(catModeActivated);
    }
}
