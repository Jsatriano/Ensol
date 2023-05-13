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
