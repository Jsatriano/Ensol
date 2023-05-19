using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractText : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private Transform canvasTF;
    private Camera cam;
    [HideInInspector] public bool canBeInteracted;
    public bool interacted;
    [SerializeField] private Collider coll;
    private float originalScale;
    [SerializeField] private float transitionRate = 6;
    private Coroutine fadeRoutine;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private bool isDialogueText;

    private void Start()
    {
        canvas.SetActive(false);
        canvasTF = canvas.transform;
        originalScale = canvasTF.localScale.x;
        interacted = false;
        canBeInteracted = true;
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (isDialogueText)
        {
            if (dialogueTrigger.interacted)
            {
                interacted = true;
            }
        }
        //Turns off text once interacted with
        if (!coll.enabled)
        {
            interacted = true;
            canvas.SetActive(false);
        }
        else if (coll.enabled && gameObject.tag != "InteractableOnce")
        {
            interacted = false;
        }

        if (interacted)
        {
            canvas.SetActive(false);
        }

        canvasTF.LookAt(cam.transform.position);

    }

    private IEnumerator TextFadeIn()
    {
        canvas.SetActive(true);
        canvasTF.localScale = new Vector3(0, 0, 0);
        float interpolator = 0;
        while (canvasTF.localScale.x < originalScale)
        {
            interpolator += transitionRate * Time.deltaTime;
            float newScale = Mathf.Lerp(0, originalScale, interpolator);
            canvasTF.localScale = Vector3.one * newScale;
            yield return null;
        }
        fadeRoutine = null;
    }

    private IEnumerator TextFadeOut()
    {
        float interpolator = 0;
        while (canvasTF.localScale.x > 0)
        {
            interpolator += transitionRate * Time.deltaTime;
            float newScale = Mathf.Lerp(originalScale, 0, interpolator);
            canvasTF.localScale = Vector3.one * newScale;
            yield return null;
        }
        canvas.SetActive(false);
        fadeRoutine = null;
    }


    //Turn on text when player is in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canBeInteracted && !interacted)
        {
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
            }
            fadeRoutine = StartCoroutine(TextFadeIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
            }
            fadeRoutine = StartCoroutine(TextFadeOut());
        }
    }
}
