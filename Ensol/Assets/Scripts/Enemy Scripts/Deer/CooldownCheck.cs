using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CooldownCheck : Node
{
    private float _cooldownLength;
    private float _cooldownTimer;
    private string _attackName;

    public CooldownCheck(float cooldownLength, string attackName)
    {
        _cooldownLength = cooldownLength;
        _cooldownTimer = -1;
        _attackName = attackName;
    }

    //Checks to see if ability is off cooldown, if so resets cooldown and returns SUCCESS, otherwise failure - RYAN
    public override NodeState Evaluate()
    {
        //Automatically returns success if the attack is already running to prevent prematurely terminating attacks
        //Also keeps setting the timer so that it only starts counting down once the attack ends
        if (GetData(_attackName) != null)
        {
            _cooldownTimer = Time.time;
            state = NodeState.SUCCESS;
            return NodeState.SUCCESS;          
        }
        //Makes a random delay before the first time an enemy attacks that way groups of enemies will have offset attacks
        if (_cooldownTimer == -1)
        {
            _cooldownTimer = Time.time - Random.Range(_cooldownLength / 4, _cooldownLength / 1.5f);
        }
        //Checks if enough time has passed since the last time the attack has been used
        if (Time.time - _cooldownTimer >= _cooldownLength)
        {        
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
