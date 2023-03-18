using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TutorialText : MonoBehaviour
{
    private enum State
    {
        NONE,
        WALK,
        BROOM,
        SOLAR,
        THROW
    }

    public TextMeshProUGUI textMesh;
    [SerializeField] private Camera cam;
    private State state;

    [Header("Tutorial Texts")]
    [TextArea] public string walkText;
    [TextArea] public string broomText;
    [TextArea] public string solarText;
    [TextArea] public string throwText;
    private string noText = "";

    [Header("Requirements")] //The things the player needs to do to make the text go away
    //Walking Requirements
    [SerializeField] private float distMoved;
    //BroomText Requirements
    [SerializeField] private float lights;
    [SerializeField] private float dashes;
    //SolarText Requirements (temporarily includes throw attacks)
    [SerializeField] private float heavies;
    [SerializeField] private float throws;

    [Header("Text Fade")]
    [SerializeField] private float fadeLength;
    private float fadeTimer;
    private Color originalColor;

    private void Start()
    {
        textMesh.text = noText;
        state = State.NONE;
        originalColor = textMesh.color;
    }

    private void LateUpdate()
    {
        //Makes the text face the camera
        transform.LookAt(cam.transform.position);

        //Shows walking controls first time in cabin node
        if (!PlayerData.shownWalkText)
        {
            state = State.WALK;
            PlayerData.shownWalkText = true;
            textMesh.text = walkText;
            textMesh.color = originalColor;
        }
        //Displays the correct text over the player's head when they get a weapon upgrade for the first time
        else if (PlayerData.hasBroom && !PlayerData.shownBroomText)
        {
            state = State.BROOM;
            PlayerData.shownBroomText = true;
            textMesh.text = broomText;
            textMesh.color = originalColor;
        }
        else if (PlayerData.hasSolarUpgrade && !PlayerData.shownSolarText)
        {
            state = State.SOLAR;
            PlayerData.shownSolarText = true;
            textMesh.text = solarText;
            textMesh.color = originalColor;
        }
        else if (PlayerData.hasThrowUpgrade && PlayerData.shownThrowText)
        {
            state = State.THROW;
            textMesh.color = originalColor;
        }

        //Checks if the player has met the requirements to get rid of the current text
        switch (state)
        {
            case State.WALK:
                if (PlayerData.distanceMoved >= distMoved)
                {
                    StartCoroutine(FadeText());
                }
                break;

            case State.BROOM:
                if (PlayerData.lightAttacks >= lights && PlayerData.dashes >= dashes)
                {
                    StartCoroutine(FadeText());           
                }
                break;

            case State.SOLAR:
                if (PlayerData.heavyAttacks >= heavies && PlayerData.throwAttacks >= throws)
                {
                    StartCoroutine(FadeText());
                }
                break;

            case State.THROW:
                break;

            case State.NONE:
                break;
        }
    }

    IEnumerator FadeText()
    {
        fadeTimer = fadeLength;
        Color col = textMesh.color;
        float originalAlpha = col.a;
        while (fadeTimer > 0)
        {
            col.a = (fadeTimer / fadeLength) * originalAlpha;
            textMesh.color = col;
            fadeTimer -= Time.deltaTime;
            yield return null;
        }
        textMesh.text = noText;
        state = State.NONE;
    }
}
