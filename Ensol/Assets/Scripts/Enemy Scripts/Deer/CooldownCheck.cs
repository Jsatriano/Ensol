using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CooldownCheck : Node
{
    private float _cooldownLength;
    private float _cooldownTimer;
    public CooldownCheck(float cooldownLength)
    {
        _cooldownLength = cooldownLength;
        _cooldownTimer = Time.time;
    }

    //Checks to see if ability is off cooldown, if so resets cooldown and returns SUCCESS, otherwise failure
    public override NodeState Evaluate()
    {
        if (Time.time - _cooldownTimer >= _cooldownLength)
        {
            _cooldownTimer = Time.time;
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
