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
    [SerializeField] private Toggle grassToggle;
    public static bool grassActivated = true;

    [Header("VCAs")]
    private FMOD.Studio.VCA musicVCA;
    private FMOD.Studio.VCA sfxVCA;



    //Unity Events
    [HideInInspector] public UnityEvent<float> OnScreenShakeChange;
    [HideInInspector] public UnityEvent<bool> OnCatModeChange;
    [HideInInspector] public UnityEvent<bool> OnGrassActivatedChange;

    //Called by PauseMenu script on start to make sure settings stay the same between scene transitions
    public void Start()
    {
        //Sets current values
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
        screenShakeSlider.value = screenShakeValue;
        catModeToggle.isOn = catModeActivated;
        grassToggle.isOn = grassActivated;
        UpdateAllSettings();

        musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        sfxVCA   = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
    }

    private void UpdateAllSettings()
    {
        UpdateScreenShake();
        UpdateMusicVolume();
        UpdateSFXVolume();
        UpdateCatMode();
        UpdateGrass();
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
        catModeActivated = catModeToggle.isOn;
        OnCatModeChange.Invoke(catModeActivated);
    }

    public void UpdateGrass()
    {
        grassActivated = grassToggle.isOn;
        OnGrassActivatedChange.Invoke(grassActivated);
    }
}
