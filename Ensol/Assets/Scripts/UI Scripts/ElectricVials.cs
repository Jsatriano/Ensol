using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricVials : MonoBehaviour // justin
{

    public GameObject[] vials;

    public int currVial;
    public GameObject toggler;
    public RectTransform rectTransform;
    public RectTransform vial1;
    public RectTransform vial2;
    public RectTransform vial3;
    public RectTransform center;
    private float interpolator;
    public Vector3 startingPos;
    private Vector3 endingPos1, endingPos2, endingPos3;
    private float transitionTimer;
    public float transitionTime;
    public HealthBar healthUI;

    // Start is called before the first frame update
    void Start()
    {
        currVial = 2; // full vials
        for (int i = 0; i < vials.Length; i += 1)
        {
            vials[i].SetActive(true);
        }
        transitionTimer = 0;
        
        if (PlayerData.hasSolarUpgrade && healthUI.finishedTransition)
        {
            transitionTimer = transitionTime;
            toggler.SetActive(true);
        }
        else
        {
            transitionTimer = 0;
            toggler.SetActive(false);
        }

        endingPos1 = vial1.localPosition;
        endingPos2 = vial2.localPosition;
        endingPos3 = vial3.localPosition;

    }

    private void Update()
    {
        //Moves the energyUI onto the screen if the player has the solar upgrade at the start of every scene and the health has finished transitioning onto screen
        if (PlayerData.hasSolarUpgrade && healthUI.finishedTransition && transitionTimer < transitionTime)
        {
            slideIn(startingPos, endingPos1, center.localPosition, vial1);
            slideIn(startingPos, endingPos2, center.localPosition, vial2);
            slideIn(startingPos, endingPos3, center.localPosition, vial3);
        }
    }

    public void AddVial()
    {
        // if vials arent full yet
        if (currVial < 2)
        {
            currVial += 1;
            vials[currVial].SetActive(true);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.hudBatteryCharge, this.transform.position);
        }
    }

    public void RemoveVials(int numVials)
    {
        // if vials arent empty yet
        if (currVial > -1)
        {
            // remove amount of vials (can vary per attack)
            for (int i = 0; i < numVials; i += 1)
            {
                vials[currVial].SetActive(false);
                currVial -= 1;
            }
        }
    }

    private void slideIn(Vector3 startPos, Vector3 endPos, Vector3 centerPos, RectTransform vial)
    {
        toggler.SetActive(true);
        Vector3 relativeStart = startPos - centerPos;
        Vector3 relativeEnd = endPos - centerPos;

        //Moves the energy bar in a circle around the healthUI until it reaches its position
        interpolator = transitionTimer / transitionTime;
        interpolator = Mathf.Sin(interpolator * Mathf.PI * 0.5f);
        vial.localPosition = Vector3.Slerp(relativeStart, relativeEnd, interpolator) + centerPos;

        //Updates rotation of energy bar
        Vector3 dirToCenter = (centerPos - vial.localPosition).normalized;
        vial.up = dirToCenter;

        transitionTimer += Time.deltaTime;
    }
}