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
    private Vector3 startingPos;
    private Vector3 endingPos;
    private float transitionTimer;
    public float transitionTime;
    [HideInInspector] public bool finishedTransition;

    private void Start()
    {
        toggler.SetActive(PlayerData.currentlyHasBroom);
        if (PlayerData.currentlyHasBroom)
        {
            finishedTransition = true;
            transitionTimer = transitionTime;
        }
        else
        {
            finishedTransition = false;
            transitionTimer = 0;
        }
        endingPos = rectTransform.localPosition;
        startingPos = new Vector3(endingPos.x - 200, endingPos.y + 200, endingPos.z);
    }

    private void Update()
    {
        //Moves the health bar onto the screen if the player has the broom at the start of every scene
        if (PlayerData.currentlyHasBroom && transitionTimer < transitionTime)
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