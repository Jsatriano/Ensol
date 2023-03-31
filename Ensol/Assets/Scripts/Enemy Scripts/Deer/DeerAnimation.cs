using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimation : MonoBehaviour
{

    public enum State
    {
        IDLE,
        MOVING_FORWARD,
        MOVING_LEFT,
        MOVING_RIGHT,
        CHARGE_WINDUP,
        CHARGING,
        SWIPING,
        DYING
    }


    [SerializeField] private Animator animController;
    [SerializeField] private Transform headTF;   
    [SerializeField] private float lookingSpeed;
    private Transform playerTF;
    public DeerBT deerBT;

    private Vector3 previousDirection;
    private Vector3 _dirToPlayer;
    private Vector3 movingDir;
    private Vector3 deerRight;
    private State state;          //Current animation the deer is playing
    private State walkingState;   //Used to check which walking anim should be playing
    private State debugState;
    public bool finishedDeathAnim;

    void Start()
    {
        state = State.IDLE;
        finishedDeathAnim = false;
        debugState = state;
    }

    void FixedUpdate()
    {
        /*
        if (debugState != state)
        {
            debugState = state;
            print(debugState);
        }
        */

        //Stops all animation once deer is dead
        if (state == State.DYING)
        {
            animController.SetBool("dying", true);
            return;
        }
        if (!deerBT.isAlive)
        {
            state = State.DYING;
            animController.SetBool("dying", true);
            return;
        }

        if (playerTF == null && deerBT != null && deerBT.root.GetData("player") != null)
        {
            playerTF = (Transform)deerBT.root.GetData("player");
        }

        walkingState = WalkingAnimDir();
        switch (state)
        {
            case State.IDLE:
                //Deer is only in idle before it notices the player, so it transitions from Idle once
                //the behavior tree has seen the player
                if (deerBT.root.GetData("player") != null)
                {
                    //Transitions into movingForward if the deer isn't moving sideways
                    if (deerBT.root.GetData("lookingForward") != null)
                    {
                        deerBT.root.ClearData("lookingForward");                      
                        animController.SetTrigger("movingForward");
                        state = State.MOVING_FORWARD;
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = walkingState;
                        if (walkingState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                        }
                    }
                }
                break;

            case State.MOVING_FORWARD:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                }
                //Checks if the deer is now moving sideways
                else if (deerBT.root.GetData("lookingForward") == null)
                {
                    state = walkingState;
                    if (state == State.MOVING_LEFT)
                    {
                        animController.SetTrigger("movingLeft");
                    }
                    else
                    {
                        animController.SetTrigger("movingRight");
                    }
                }
                break;

            case State.MOVING_LEFT:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                }
                //Checks if the deer is no longer moving sideways
                else if (deerBT.root.GetData("lookingForward") != null)
                {
                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                //Checks if the deer is now moving sideways in the other direction
                else if (walkingState != state)
                {
                    animController.SetTrigger("movingRight");
                    state = State.MOVING_RIGHT;
                }
                break;

            case State.MOVING_RIGHT:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                }
                //Checks if the deer is no longer moving sideways
                else if (deerBT.root.GetData("lookingForward") != null)
                {
                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                //Checks if the deer is now moving sideways in the other direction
                else if (walkingState != state)
                {
                    animController.SetTrigger("movingLeft");
                    state = State.MOVING_LEFT;
                }
                break;

            case State.CHARGE_WINDUP:
                //Transitions to charging anim at the end of the charge windup anim from animation event
                if (deerBT.root.GetData("chargingAnim") != null)
                {
                    //Ensures that no walking triggers are currently active
                    animController.ResetTrigger("movingForward");
                    animController.ResetTrigger("movingLeft");
                    animController.ResetTrigger("movingRight");

                    animController.SetTrigger("charging");
                    state = State.CHARGING;
                }
                break;

            case State.CHARGING:
                //Checks if the charge has ended
                if (deerBT.root.GetData("chargingAnim") == null)
                {
                    //Transitions into movingForward if the deer isn't moving sideways
                    if (deerBT.root.GetData("lookingForward") != null)
                    {
                        deerBT.root.ClearData("lookingForward");
                        animController.SetTrigger("movingForward");
                        state = State.MOVING_FORWARD;
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = walkingState;
                        if (walkingState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                        }
                    }
                }
                break;

            case State.SWIPING:
                //Checks if the swiping attack has ended
                if (deerBT.root.GetData("swipingAnim") == null)
                {
                    //Ensures that no walking triggers are currently active
                    animController.ResetTrigger("movingForward");
                    animController.ResetTrigger("movingLeft");
                    animController.ResetTrigger("movingRight");

                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                break;

            case State.DYING:
                //Don't update animations anymore when dead
                
                break;
        }
    }

    private void LateUpdate()
    {
        //Rotates deers head towards player when walking around
        if (state == State.MOVING_FORWARD || state == State.MOVING_LEFT || state == State.MOVING_RIGHT)
        {
            if (playerTF == null)
            {
                return;
            }
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
            //Rotate deers head towards the player
            headTF.forward = Vector3.Lerp(previousDirection, _dirToPlayer, lookingSpeed * Time.deltaTime).normalized;        
        }
        previousDirection = headTF.forward;
    }

    private void StartCharge()
    {
        deerBT.root.SetData("chargingAnim", true);
        deerBT.root.ClearData("chargeWindupAnim");
    }

    private void EndSwipeWindup()
    {
        deerBT.root.SetData("endSwipeWindup", true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deerAttack, this.transform.position);
    }

    private void EndSwipe()
    {
        deerBT.root.SetData("endSwipe", true);
    }

    private void DeathFinished()
    {
        finishedDeathAnim = true;
    }


    //Calculates whether the deer is moving to the left or right 
    private State WalkingAnimDir()
    {
        if (deerBT.root.GetData("deerRight") != null && deerBT.root.GetData("movingDir") != null)
        {
            movingDir = (Vector3)deerBT.root.GetData("movingDir");
            deerRight = (Vector3)deerBT.root.GetData("deerRight");
            if (Vector3.Dot(movingDir, deerRight) > 0)
            {
                return State.MOVING_LEFT;
            }
            else
            {
                return State.MOVING_RIGHT;
            }
        }
        else
        {
            return State.MOVING_LEFT;
        }
    }
}
