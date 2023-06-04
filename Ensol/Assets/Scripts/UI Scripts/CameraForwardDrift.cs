using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForwardDrift : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float maxDrift = 10f;
    private bool leftMoving = true;
    private Vector3 startPosition;
    private Vector3 curPostion;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        curPostion = transform.position;
        curPostion.z += moveSpeed;
        transform.position = curPostion;
    }
}
