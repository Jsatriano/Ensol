using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushMovement : MonoBehaviour
{

    [SerializeField] public List<Transform> positions;
    [SerializeField] public GameObject plush;
    [SerializeField] public float moveSpeed = 1.0f;
    [SerializeField] public float rotateSpeed = 4.0f;
    [SerializeField] public int maxWaitTime = 10;
    [SerializeField] public Animator plushAnims;

    [Header("Head Rotation")]
    [SerializeField] private float headRotateSpeed;
    [SerializeField] private Transform headTF;
    private Vector3 previousDirection = Vector3.forward;
    private Transform playerTF;
    private Quaternion prevRotation;

    public _00CabinNode nodeScript;


    private int currPositionIndex = 0;
    private Quaternion rotation;
    private Vector3 moveDirection;
    private float interpolator;
    private bool colliding = false;
    private bool waiting = false;
    private bool hasInteractedOnce = false;
    Vector3 toPlayer;

    // Start is called before the first frame update
    void Start()
    {
        interpolator = 0;
        moveDirection = (plush.transform.localPosition - positions[currPositionIndex].position).normalized;
        rotation = Quaternion.LookRotation(moveDirection);
        //StartCoroutine(ignorePlush());
    }

    private void LateUpdate()
    {  
        //Make cat look at player when close
        if (colliding && playerTF)
        {
            //Rotates cat's head towards player when walking around
            if (playerTF == null)
            {
                return;
            }
            Vector3 _dirToPlayer = new Vector3(playerTF.position.x - headTF.position.x, (playerTF.position.y + 1) - headTF.position.y, playerTF.position.z - headTF.position.z).normalized;
            //Rotates cats head back to normal when player isnt in front of cat, else rotates to face player
            if (Vector3.Dot(_dirToPlayer, -plush.transform.forward) < .1f)
            {
                headTF.forward = Vector3.Lerp(previousDirection, headTF.forward, headRotateSpeed * Time.deltaTime).normalized;
            }
            else
            {
                headTF.forward = Vector3.Lerp(previousDirection, _dirToPlayer, headRotateSpeed * Time.deltaTime).normalized;
            }
        }
        else
        {
            headTF.forward = Vector3.Lerp(previousDirection, headTF.forward, headRotateSpeed * Time.deltaTime).normalized;
        }
        previousDirection = headTF.forward;
    }

    // Update is called once per frame
    private void Update()
    {

        if ((colliding && DialogueManager.GetInstance().dialogueisPlaying && !hasInteractedOnce) || (Vector3.Distance(transform.position, nodeScript.players[0].transform.position) > 5.0f && !hasInteractedOnce)) {
            hasInteractedOnce = true;
        }

        if (plush.transform.position != positions[currPositionIndex].position && !(colliding && DialogueManager.GetInstance().dialogueisPlaying) && !waiting && hasInteractedOnce) {
            plushAnims.ResetTrigger("Sitting");
            plushAnims.SetTrigger("Walking");
            // rotate plush towards next position
            plush.transform.localRotation = Quaternion.Slerp(plush.transform.localRotation, rotation, rotateSpeed*Time.deltaTime);
            // move plush towards next position
            plush.transform.position = Vector3.MoveTowards(plush.transform.position, positions[currPositionIndex].position, moveSpeed*Time.deltaTime);
        }
        else {
            if (!waiting) {
                // wait
                StartCoroutine(PlushWait());
                // update next position to move to
                currPositionIndex = Random.Range(0, positions.Count);
                // update the position to rotate towards
                moveDirection = (plush.transform.position - positions[currPositionIndex].position).normalized;
                rotation = Quaternion.LookRotation(moveDirection);
            }
        }      
    }

    IEnumerator PlushWait()
    {
        waiting = true;
        plushAnims.ResetTrigger("Walking");
        plushAnims.SetTrigger("Sitting");
        var time = Random.Range(2, maxWaitTime);
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
            playerTF = other.transform;
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
