using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JUSTIN
public class GeneratorWallToggle : MonoBehaviour
{
    public MeshRenderer[] generatorMeshes;

    private bool inZone = false;
    private float currAlpha = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //print(currAlpha);
        // if player walks in generator zone
        if(inZone && currAlpha > 0.0f)
        {
            currAlpha = Mathf.MoveTowards(currAlpha, currAlpha -= 0.01f, 1f);
            setAlpha(currAlpha);
            if(currAlpha <= 0.0f)
            {
                // turn off walls
                foreach (MeshRenderer mesh in generatorMeshes)
                {
                    mesh.enabled = false;
                }
            }
        }

        // if player walks out of generator zone
        if(!inZone && currAlpha <= 1.0f)
        {
            currAlpha = Mathf.MoveTowards(currAlpha, currAlpha += 0.01f, 1f);
            setAlpha(currAlpha);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        // turn walls off
        if(col.tag == "Player")
        {
            inZone = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            // turn back on walls
            foreach (MeshRenderer mesh in generatorMeshes)
            {
                mesh.enabled = true;
            }
            inZone = false;
        }
    }

    void setAlpha(float alpha)
    {
        Color tempColor;
        for(int i = 0; i < (generatorMeshes.Length - 1); i += 1)
        {
            // if there is more than 1 material attached to a mesh
            if(generatorMeshes[i].gameObject.tag == "MultipleMaterials")
            {
                foreach (Material mat in generatorMeshes[i].materials)
                {
                    tempColor = mat.color;
                    tempColor.a = alpha;
                    mat.color = tempColor;
                }
            }
            // if there is only 1 material attached to a mesh
            else
            {
                tempColor = generatorMeshes[i].materials[0].color;
                tempColor.a = alpha;
                generatorMeshes[i].materials[0].color = tempColor;
            }
                
        }
    }

}
