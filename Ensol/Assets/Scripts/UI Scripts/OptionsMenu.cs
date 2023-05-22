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

    //Unity Events
    [HideInInspector] public UnityEvent<float> OnScreenShakeChange;
    [HideInInspector] public UnityEvent<float> OnMusicVolumeChange;
    [HideInInspector] public UnityEvent<float> OnSFXVolumeChange;
    [HideInInspector] public UnityEvent<bool> OnCatModeChange;

    private void Start()
    {
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
        screenShakeSlider.value = screenShakeValue;
        catModeToggle.isOn = catModeActivated;
    }

    public void UpdateScreenShake()
    {
        screenShakeValue = screenShakeSlider.value;
        OnScreenShakeChange.Invoke(screenShakeValue);
    }

    public void UpdateMusicVolume()
    {
        musicValue = musicSlider.value;
        OnMusicVolumeChange.Invoke(musicValue);
    }

    public void UpdateSFXVolume()
    {
        sfxValue = sfxSlider.value;
        OnSFXVolumeChange.Invoke(sfxValue);
    }

    public void UpdateCatMode()
    {
        Debug.Log("CATMODeeee");
        catModeActivated = catModeToggle.isOn;
        OnCatModeChange.Invoke(catModeActivated);
    }
}
