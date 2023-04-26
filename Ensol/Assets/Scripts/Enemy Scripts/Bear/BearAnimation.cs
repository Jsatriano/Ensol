using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnimation : MonoBehaviour
{
    public enum State
    {
        IDLE,
        WALKING,
        SWIPING,
        THROWING,
        DYING
    }

    [SerializeField] private Animator animController;
    [SerializeField] private Transform headTF;
    [SerializeField] private float lookingSpeed;
    private Transform playerTF;
    public BearBT bearBT;
    [SerializeField] private Rigidbody bearRB;
    private float maxSpeed;

    private Vector3 previousDirection;
    private Vector3 _dirToPlayer;
    private State state;

    void Start()
    {
        state = State.IDLE;
    }

    private void FixedUpdate()
    {
        //Stops all animation once bear is dead
        if (state == State.DYING)
        {
            animController.SetTrigger("Dying");
            return;
        }
        if (!bearBT.isAlive)
        {
            state = State.DYING;
            animController.SetTrigger("Dying");
            return;
        }

        //Used to check when the bear enters aggro
        if (playerTF == null && bearBT.root.GetData("player") != null)
        {
            playerTF = (Transform)bearBT.root.GetData("player");
            maxSpeed = bearBT.bearStats.maxSpeed;

        }

        switch(state)
        {
            case State.IDLE:
                //Exits idle once bear has seen player
                if (bearBT.root.GetData("player") != null)
                {
                    animController.SetTrigger("Walking");
                    state = State.WALKING;
                }
                return;

            case State.WALKING:
                //Sets the speed of the walking animation based on current velocity
                animController.SetFloat("AnimSpeed", Mathf.Clamp((bearRB.velocity.magnitude * 10) / maxSpeed, 0.2f, 1.5f));        

                //Start swiping anim
                if (bearBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("Swiping");
                    state = State.SWIPING;
                }
                //Start throwing anim
                else if (bearBT.root.GetData("throwingAnim") != null)
                {
                    animController.SetTrigger("Throwing");
                    state = State.THROWING;
                }
                return;

            case State.SWIPING:
                //End swiping anim (go back to walking)
                if (bearBT.root.GetData("swipingAnim") == null)
                {
                    animController.SetTrigger("Walking");
                    state = State.WALKING;
                }
                return;

            case State.THROWING:
                //End throwing anim (go back to walking)
                if (bearBT.root.GetData("throwingAnim") == null)
                {
                    animController.SetTrigger("Walking");
                    state = State.WALKING;
                }
                return;

            case State.DYING:
                //Don't do anything when dead
                return;
        }
    }

    /*
    private void LateUpdate()
    {
        
        //Rotates bear's head towards player when walking around
        if (state == State.WALKING)
        {
            //Finds the direction to the player and then checks if that is greater than 90 degrees from the current head position
            _dirToPlayer = new Vector3(playerTF.position.x - headTF.position.x, 0, playerTF.position.z - headTF.position.z).normalized;
            if (Vector3.Dot(_dirToPlayer, transform.forward) < 0)
            {
                //If greater than 90 degrees, check if the dir to player is closer to the right or left of the deer (that will be the new rotate goal)
                if (Vector3.Dot(transform.right, _dirToPlayer) > Vector3.Dot(-transform.right, _dirToPlayer))
                {
                    _dirToPlayer = transform.right;
                }
                else
                {
                    _dirToPlayer = -transform.right;
                }
            }
            //Rotate bear's head towards the player
            //headTF.forward = Vector3.Lerp(previousDirection, _dirToPlayer, lookingSpeed * Time.deltaTime).normalized;
        }
        previousDirection = headTF.forward;
        
    }

    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && bearBT.root.GetData("player") != null)
        {
            Gizmos.color = Color.red;
            _dirToPlayer = new Vector3(playerTF.position.x - headTF.position.x, 0, playerTF.position.z - headTF.position.z).normalized;
            Gizmos.DrawRay(transform.position, _dirToPlayer * 2);
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, headTF.forward * 2);
        }
    }
    */

    //Animation events
    private void EndSwipeWindup()
    {
        bearBT.root.SetData("endWindup", true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bearSwipe, this.transform.position);
    }

    private void EndSwipe()
    {
        bearBT.root.SetData("endSwipe", true);
    }

    private void ThrowBall()
    {
        bearBT.root.SetData("throwJunk", true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bearThrow, this.transform.position);
    }

    private void EndThrow()
    {
        bearBT.root.SetData("endThrow", true);
    }
}
