using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Color outlineColor;
    [SerializeField] private float outlineScaleFactor;
    private Renderer outlineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        outlineRenderer.enabled = true;
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        outlineObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Renderer rend = outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        outlineObject.GetComponent<OutlineScript>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }

    public void outlineSwitch()
    {
        if(outlineRenderer.enabled == true)
        {
            outlineRenderer.enabled = false;
        }
        else
        {
            outlineRenderer.enabled = true;
        }
    }
}
