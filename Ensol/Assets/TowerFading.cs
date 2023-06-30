using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFading : MonoBehaviour
{
    public GameObject powerTower;
    public float speed;
    private float currAlpha = 1f;
    private bool fadeOut = false;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeOut == true && currAlpha > 0.0f)
        {
            currAlpha = Mathf.MoveTowards(currAlpha, currAlpha -= 0.01f, 1f);
            setAlpha(currAlpha);
        }
        
        if(fadeOut == false && currAlpha <= 1.0f)
        {
            currAlpha = Mathf.MoveTowards(currAlpha, currAlpha += 0.01f, 1f);
            setAlpha(currAlpha);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            fadeOut = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            fadeOut = false;
        }
    }

    void setAlpha(float alpha)
    {
        Color tempColor;
        tempColor = powerTower.GetComponent<Renderer>().material.GetColor("_AlbedoColor");
        tempColor.a = alpha;
        powerTower.GetComponent<Renderer>().material.SetColor("_AlbedoColor", tempColor);
    }
}
