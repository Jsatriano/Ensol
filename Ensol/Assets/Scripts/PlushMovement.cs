using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushMovement : MonoBehaviour
{

    [SerializeField] public List<Vector3> positions;
    [SerializeField] public GameObject plush;

    private int currPositionIndex = 1;
    private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plush.transform.localPosition != positions[currPositionIndex]) {
            var step =  speed * Time.deltaTime; // calculate distance to move
            plush.transform.localPosition = Vector3.MoveTowards(plush.transform.localPosition, positions[currPositionIndex], step);
        }
        else {
            currPositionIndex += 1;
            if (currPositionIndex >= positions.Count) {
                currPositionIndex = 0;
            }
        }
        
    }
}
