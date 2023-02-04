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
                }
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    animController.SetTrigger("swipe");
                    state = State.SWIPING;
                }
                return;
            case State.CHARGE_WINDUP:
                if (deerBT.root.GetData("chargingAnim") != null)
                {
                    animController.SetTrigger("charging");
                    state = State.CHARGING;
                }
                return;
            case State.CHARGING:
                if (deerBT.root.GetData("chargingAnim") == null)
                {
                    animController.SetTrigger("idle");
                    state = State.MOVING_LEFT;
                }
                return;
            case State.SWIPING:
                if (deerBT.root.GetData("swipingAnim") == null)
                {
                    animController.SetTrigger("idle");
                    state = State.MOVING_LEFT;
                }
                return;
        }
    }
}
