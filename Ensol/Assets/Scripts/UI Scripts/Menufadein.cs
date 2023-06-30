using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menufadein : MonoBehaviour
{
    public GameObject buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttons.SetActive(false);
        StartCoroutine(TurnButtonson());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator TurnButtonson()
    {
        yield return new WaitForSeconds(0.1f);
        buttons.SetActive(true);
    }
}
