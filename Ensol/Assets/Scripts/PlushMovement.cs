using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushMovement : MonoBehaviour
{

    [SerializeField] public List<Vector3> positions;
    [SerializeField] public GameObject plush;
    [SerializeField] public float moveSpeed = 1.0f;
    [SerializeField] public float rotateSpeed = 4.0f;
    [SerializeField] public int maxWaitTime = 10;
    [SerializeField] public Animator plushAnims;

    public _00CabinNode nodeScript;


    private int currPositionIndex = 0;
    private Quaternion rotation;
    private Vector3 moveDirection;
    private bool colliding = false;
    private bool waiting = false;
    private bool hasInteractedOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = (plush.transform.localPosition - positions[currPositionIndex]).normalized;
        rotation = Quaternion.LookRotation(moveDirection);
        //StartCoroutine(ignorePlush());
    }

    // Update is called once per frame
    void Update()
    {
        if ((colliding && DialogueManager.GetInstance().dialogueisPlaying && !hasInteractedOnce) || (Vector3.Distance(transform.position, nodeScript.players[0].transform.position) > 5.0f && !hasInteractedOnce)) {
            hasInteractedOnce = true;
        }

        if (plush.transform.localPosition != positions[currPositionIndex] && !(colliding && DialogueManager.GetInstance().dialogueisPlaying) && !waiting && hasInteractedOnce) {
            plushAnims.ResetTrigger("Sitting");
            plushAnims.SetTrigger("Walking");
            // rotate plush towards next position
            plush.transform.localRotation = Quaternion.Slerp(plush.transform.localRotation, rotation, rotateSpeed*Time.deltaTime);
            // move plush towards next position
            plush.transform.localPosition = Vector3.MoveTowards(plush.transform.localPosition, positions[currPositionIndex], moveSpeed*Time.deltaTime);
        }
        else {
            if (!waiting) {
                // wait
                StartCoroutine(PlushWait());
                // update next position to move to
                currPositionIndex = Random.Range(0, positions.Count);
                // update the position to rotate towards
                moveDirection = (plush.transform.localPosition - positions[currPositionIndex]).normalized;
                rotation = Quaternion.LookRotation(moveDirection);
            }
        }
        
    }

    IEnumerator PlushWait()
    {
        waiting = true;
        plushAnims.ResetTrigger("Walking");
        plushAnims.SetTrigger("Sitting");
        var time = Random.Range(0, maxWaitTime);
        yield return new WaitForSeconds(time);
        waiting = false;
    }

    IEnumerator ignorePlush() {
        yield return new WaitForSeconds(30);
        hasInteractedOnce = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
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
