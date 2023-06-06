using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLighting : MonoBehaviour
{
    [Header("Lighting Change")]
    [SerializeField] private Color startingColor;
    [SerializeField] private Color endingColor;
    [SerializeField] private Light sunlight;

    private void Awake()
    {
        int completed = 0;
        foreach (bool state in CompletedNodes.completedNodes)
        {
            if (state)
            {
                completed++;
            }
        }
        //Does minus 2 to exclude the cabin/computer node which are always completed
        sunlight.color = Color.Lerp(startingColor, endingColor, (float)(completed-2) / (float)(CompletedNodes.completedNodes.Length - 2));

        //StartCoroutine(LightChange());
    }

    //Was used for showing the transition from start to finish
    private IEnumerator LightChange()
    {
        float interpolator = 0;
        while (sunlight.color != endingColor)
        {
            sunlight.color = Color.Lerp(startingColor, endingColor, interpolator);
            interpolator += Time.deltaTime * 0.05f;
            yield return null;
        }
    }
}
