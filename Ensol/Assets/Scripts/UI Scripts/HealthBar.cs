using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour // justin
{

    public Slider slider;
    public GameObject toggler;
    public RectTransform rectTransform;
    private float interpolator;
    public Vector3 startingPos, endingPos;
    private float transitionTimer;
    public float transitionTime;
    public bool finishedTransition;

    private void Start()
    {
        transitionTimer = 0;
        toggler.SetActive(false);
        finishedTransition = false;
    }

    private void Update()
    {
        //Moves the health bar onto the screen if the player has the broom at the start of every scene
        if (PlayerData.hasBroom && transitionTimer < transitionTime)
        {
            slideIn();
        }
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    private void slideIn()
    {
        toggler.SetActive(true);
        //Transitions the health bar onto screen 
        interpolator = transitionTimer / transitionTime;
        interpolator = Mathf.Sin(interpolator * Mathf.PI * 0.5f);
        rectTransform.localPosition = Vector3.Lerp(startingPos, endingPos, interpolator);
        transitionTimer += Time.deltaTime;
        if (transitionTimer >= transitionTime)
        {
            finishedTransition = true;
        }
        
    }
}
