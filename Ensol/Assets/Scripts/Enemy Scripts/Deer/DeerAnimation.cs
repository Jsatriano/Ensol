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
    [SerializeField] private Transform playerTF;
    [SerializeField] private float lookingSpeed;
    public DeerBT deerBT;

    private Vector3 movingDir;
    private Vector3 deerRight;
    State state;
    State tempState;

    void Start()
    {
        state = State.IDLE;
    }

    void FixedUpdate()
    {
        //Stops all animation once deer is dead
        if (state == State.DYING)
        {
            return;
        }
        if (!deerBT.isAlive)
        {
            state = State.DYING;
            animController.SetTrigger("dying");
            return;
        }

        if (headTF == null && deerBT.root.GetData("player") != null)
        {
            playerTF = (Transform)deerBT.root.GetData("player");
        }


        tempState = WalkingAnimDir();
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
                        print("movingForward");
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = tempState;
                        if (tempState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                            print("MovingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                            print("MovingRight");
                        }
                    }
                }
                return;

            case State.MOVING_FORWARD:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    print("Charge Windup");
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                    print("Swiping");
                }
                //Checks if the deer is now moving sideways
                else if (deerBT.root.GetData("lookingForward") == null)
                {
                    state = tempState;
                    if (state == State.MOVING_LEFT)
                    {
                        animController.SetTrigger("movingLeft");
                        print("MovingLeft");
                    }
                    else
                    {
                        animController.SetTrigger("movingRight");
                        print("MovingRight");
                    }
                }
                return;

            case State.MOVING_LEFT:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    print("Charge Windup");
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                    print("Swiping");
                }
                //Checks if the deer is no longer moving sideways
                else if (deerBT.root.GetData("lookingForward") != null)
                {
                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                    print("movingForward");
                }
                //Checks if the deer is now moving sideways in the other direction
                else if (tempState != state)
                {
                    animController.SetTrigger("movingRight");
                    state = tempState;
                    print("MovingRight");
                }
                return;

            case State.MOVING_RIGHT:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    print("ChargeWindup");
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                    print("Swipe");
                }
                //Checks if the deer is no longer moving sideways
                else if (deerBT.root.GetData("lookingForward") != null)
                {
                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                    print("movingForward");
                }
                //Checks if the deer is now moving sideways in the other direction
                else if (tempState != state)
                {
                    animController.SetTrigger("movingLeft");
                    state = tempState;
                    print("MovingLeft");
                }
                return;

            case State.CHARGE_WINDUP:
                //Checks if the charge has started
                if (deerBT.root.GetData("chargingAnim") != null)
                {
                    animController.SetTrigger("charging");
                    state = State.CHARGING;
                    print("Charging");
                }
                return;

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
                        print("movingForward");
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = tempState;
                        if (tempState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                            print("MovingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                            print("MovingRight");
                        }
                    }
                }
                return;

            case State.SWIPING:
                //Checks if the swiping attack has ended
                if (deerBT.root.GetData("swipingAnim") == null)
                {
                    //Transitions into movingForward if the deer isn't moving sideways
                    if (deerBT.root.GetData("lookingForward") != null)
                    {
                        deerBT.root.ClearData("lookingForward");
                        animController.SetTrigger("movingForward");
                        state = State.MOVING_FORWARD;
                        print("movingForward");
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = tempState;
                        if (tempState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                            print("MovingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                            print("MovingRight");
                        }
                    }
                }
                return;

            case State.DYING:
                //Don't update animations anymore when dead
                return;
        }

        if (state == State.MOVING_FORWARD || state == State.MOVING_LEFT || state ==State.MOVING_RIGHT)
        {
            //Head rotation here
        }
    } 

    //Calculates whether the deer is moving to the left or right 
    private State WalkingAnimDir()
    {
        if (deerBT.root.GetData("deerRight") != null && deerBT.root.GetData("movingDir") != null)
        {
            movingDir = (Vector3) deerBT.root.GetData("movingDir");
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
