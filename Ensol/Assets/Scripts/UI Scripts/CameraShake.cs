using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCam;
    private Coroutine shakeRoutine = null;
    [SerializeField] private float defaultShakeIntensity;
    private float shakeIntensity;
    [SerializeField] private float shakeTime;
    [SerializeField] private OptionsMenu optionsMenu;

    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        shakeIntensity = defaultShakeIntensity;
    }

    private void Start()
    {
        optionsMenu.OnScreenShakeChange.AddListener(UpdateCameraShake);
        UpdateCameraShake(defaultShakeIntensity * OptionsMenu.screenShakeValue);
    }

    private void UpdateCameraShake(float value)
    {
        shakeIntensity = defaultShakeIntensity * value;
    }


    public void ShakeCamera()
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }
        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = shakeIntensity;

        float shakeTimer = shakeTime;
        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            yield return null;
        }
        perlin.m_AmplitudeGain = 0f;       
    }
}
