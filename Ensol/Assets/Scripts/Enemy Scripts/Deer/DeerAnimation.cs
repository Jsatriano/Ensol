using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimation : MonoBehaviour
{

    public enum State
    {
        MOVING_LEFT,
        MOVING_RIGHT,
        CHARGE_WINDUP,
        CHARGING,
        SWIPING,
    }


    [SerializeField] private Animator animController;
    public DeerBT deerBT;

    private Vector3 cross;
    private Vector3 dirToPlayer;
    State state;

    void Start()
    {
        state = State.MOVING_LEFT;
    }

    void Update()
    {
        switch (state)
        {
            case State.MOVING_LEFT: 
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    print("Charge Windup");
                }
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                    print("Swiping");
                }
                else if (IdleAnimDir() != state)
                {
                    animController.SetTrigger("movingRight");
                    state = IdleAnimDir();
                    print("MovingRight");
                }
                return;

            case State.MOVING_RIGHT:
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    print("ChargeWindup");
                }
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                    print("Swipe");
                }
                else if (IdleAnimDir() != state)
                {
                    animController.SetTrigger("movingLeft");
                    state = IdleAnimDir();
                    print("MovingLeft");
                }
                return;

            case State.CHARGE_WINDUP:
                if (deerBT.root.GetData("chargingAnim") != null)
                {
                    animController.SetTrigger("charging");
                    state = State.CHARGING;
                    print("Charging");
                }
                return;

            case State.CHARGING:
                if (deerBT.root.GetData("chargingAnim") == null)
                {
                    state = IdleAnimDir();
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

            case State.SWIPING:
                if (deerBT.root.GetData("swipingAnim") == null)
                {
                    state = IdleAnimDir();
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
        }
    } 

    private State IdleAnimDir()
    {
        if (deerBT.root.GetData("movingCross") != null && deerBT.root.GetData("dirToPlayer") != null)
        {
            cross = (Vector3) deerBT.root.GetData("movingCross");
            dirToPlayer = (Vector3)deerBT.root.GetData("dirToPlayer");
            if (Vector3.Dot(cross, dirToPlayer) > 0)
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
