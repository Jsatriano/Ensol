using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private float duration;
    [SerializeField] private GameObject[] makeFlash;
    

    
    public IEnumerator FlashRoutine()
    {
        // swap to the flash material (start flash)
        for(int i = 0; i < makeFlash.Length; i += 1)
        {
            if(makeFlash[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                makeFlash[i].GetComponent<SkinnedMeshRenderer>().material = flashMaterial;
            }
            else
            {
                makeFlash[i].GetComponent<MeshRenderer>().material = flashMaterial;
            }
        }

        yield return new WaitForSeconds(duration);

        // swap back to original material (end flash)
        for(int i = 0; i < makeFlash.Length; i += 1)
        {
            if(makeFlash[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                makeFlash[i].GetComponent<SkinnedMeshRenderer>().material = originalMaterial;
            }
            else
            {
                makeFlash[i].GetComponent<MeshRenderer>().material = originalMaterial;
            }
        }

    }
}
