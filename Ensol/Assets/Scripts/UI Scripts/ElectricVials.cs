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
    public RectTransform vial1, vial2, vial3, center;
    public Slider[] sliders;
    private float interpolator;
    public Vector3 startingPos;
    private Vector3 endingPos1, endingPos2, endingPos3;
    private float transitionTimer;
    public float transitionTime;
    public HealthBar healthUI;

    [Header("Flicker Variables")]
    [SerializeField] private Image[] borders;
    [SerializeField] private float flickerLength;
    private float flickerLengthTimer;
    [SerializeField] private float flickerTime;
    private float flickerTimer;
    [SerializeField] private Color flickerColor;
    private Color originalColor;
    private bool flickerState;
    private bool isFlickering;

    // Start is called before the first frame update
    void Start()
    {
        isFlickering = false;
        originalColor = borders[0].color;

        currVial = 2; // full vials
        for (int i = 0; i < vials.Length; i += 1)
        {
            vials[i].SetActive(true);
            UpdateVial(1, i);
            
        }
        transitionTimer = 0;
        
        if (PlayerData.currentlyHasSolar && healthUI.finishedTransition)
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
        if (PlayerData.currentlyHasSolar && healthUI.finishedTransition && transitionTimer < transitionTime)
        {
            slideIn(startingPos, endingPos1, center.localPosition, vial1);
            slideIn(startingPos, endingPos2, center.localPosition, vial2);
            slideIn(startingPos, endingPos3, center.localPosition, vial3);
        }

        //Resets all the colors once flickering is over
        if (flickerLengthTimer >= flickerLength && borders[0].color == originalColor)
        {
            isFlickering = false;
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = originalColor;
            }
        }

        //UI Flickering
        if (isFlickering)
        {
            print("hewwo");
            //Checks if it needs to start flickering in the other direction
            if (flickerTimer >= flickerTime)
            {

                flickerState = !flickerState;
                flickerTimer = 0;
            }
            flickerLengthTimer += Time.deltaTime;
            flickerTimer += Time.deltaTime;
            //Transitions towards the flicker color
            if (flickerState)
            {
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i].color = Color.Lerp(originalColor, flickerColor, flickerTimer / flickerTime);
                }
            }
            //Transitions back to original color;
            else
            {
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i].color = Color.Lerp(flickerColor, originalColor, flickerTimer / flickerTime);
                }
            }
        }

    }

    public void AddVial(float percent)
    {
        // if vials arent full yet
        if (currVial < 2)
        {
            currVial += 1;
            //vials[currVial].SetActive(true);
            sliders[currVial].value = 1;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.hudBatteryCharge, this.transform.position);
        }
    }

    public void UpdateVial(float percent, int vial)
    {
        //Updates the fill percentage of the currently refilling energy vial
        if (currVial < 3)
        {
            sliders[vial].maxValue = 1;
            sliders[vial].value = percent;
            //Makes sure all of the vials above this vial are empty. (For when the player uses vials halfway through a vial refilling)
            for (int i = vial + 1; i < sliders.Length; i++)
            {
                sliders[i].value = 0;
            }
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
                //vials[currVial].SetActive(false);
                sliders[currVial].value = 0;
                currVial -= 1;
            }
        }
    }

    //Takes in number of vials needed to use an ability, returns if the player currently has enough vials
    public bool enoughVials(int neededVials)
    {
        //Activates the UI flashing effect if player doesn't have enough vials
        if (currVial + 1 < neededVials)
        {
            if (isFlickering)
            {
                flickerLengthTimer = 0;
            }
            else
            {
                flickerLengthTimer = 0;
                flickerTimer = 0;
                isFlickering = true;
                flickerState = true;
            }
            return false;
        }
        return true;
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