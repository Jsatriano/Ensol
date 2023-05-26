using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushMovement : MonoBehaviour
{

    [SerializeField] public List<Vector3> positions;
    [SerializeField] public GameObject plush;
    [SerializeField] public float moveSpeed = 1.0f;
    [SerializeField] public float rotateSpeed = 4.0f;


    private int currPositionIndex = 0;
    private Quaternion rotation;
    private Vector3 moveDirection;
    private bool colliding = false;
    private bool talkingToPlush = false;


    // Start is called before the first frame update
    void Start()
    {
        moveDirection = (plush.transform.localPosition - positions[currPositionIndex]).normalized;
        rotation = Quaternion.LookRotation(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        // checks if player is in dialouge with plush
        if (colliding && DialogueManager.GetInstance().dialogueisPlaying) {
            talkingToPlush = true;
        }
        else {
            talkingToPlush = false;
        }

        if (plush.transform.localPosition != positions[currPositionIndex] && !talkingToPlush) {
            // rotate plush towards next position
            plush.transform.localRotation = Quaternion.Slerp(plush.transform.localRotation, rotation, rotateSpeed*Time.deltaTime);
            // move plush towards next position
            plush.transform.localPosition = Vector3.MoveTowards(plush.transform.localPosition, positions[currPositionIndex], moveSpeed*Time.deltaTime);
        }
        else {
            // update next position to move to
            currPositionIndex = Random.Range(0, positions.Count);
            // update the position to rotate towards
            moveDirection = (plush.transform.localPosition - positions[currPositionIndex]).normalized;
            rotation = Quaternion.LookRotation(moveDirection);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("COLLIDING");
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            colliding = false;
        }
    }
}
