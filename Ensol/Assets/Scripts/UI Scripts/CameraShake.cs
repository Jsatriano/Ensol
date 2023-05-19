using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCam;
    private Coroutine shakeRoutine = null;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;

    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
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
        Debug.Log("HEYY");
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
