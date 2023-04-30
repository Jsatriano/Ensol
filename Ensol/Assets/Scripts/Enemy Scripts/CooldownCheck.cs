using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CooldownCheck : Node
{
    private float _cooldownLength; //How long the cooldown is
    private float _cooldownTimer;  //Used internally to time the cooldown
    private float _cooldownLengthRandom;
    private string _attackName;    //Name of the attack the cooldown is for

    public CooldownCheck(float cooldownLength, string attackName)
    {
        _cooldownLength = cooldownLength;
        _cooldownTimer  = -1;
        _attackName     = attackName;
    }

    //Checks to see if ability is off cooldown, if so resets cooldown and returns SUCCESS, otherwise failure - RYAN
    public override NodeState Evaluate()
    {
        //Initially stores or retrieves the cooldown length from the dashboard, used to allow the cooldown to be updated during runtime
        if (GetData(_attackName + "Cooldown") == null)
        {
            SetData(_attackName + "Cooldown", _cooldownLength);
        }
        else
        {
            _cooldownLength = (float)GetData(_attackName + "Cooldown");
        }

        //Checks if enemy is already attacking and automically passes if it is the attack this node is for (to prevent interrupting it)
        //Also keeps setting the timer so that it only starts counting down once the attack ends
        string attack = (string)GetData("attacking");
        if (attack != null)
        {
            if (attack == _attackName)
            {
                _cooldownTimer = Time.time;
                state = NodeState.SUCCESS;
                return NodeState.SUCCESS;
            }
            else
            {
                //Checks if this is a cooldown for the idle state, which will have an Inverter on it, which means this needs to be inverted
                if (_attackName == "idle")
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
        else
        {;
            //Makes a random delay before the first time an enemy attacks that way groups of enemies will have offset attacks
            if (_cooldownTimer == -1)
            {
                if (_attackName == "idle")
                {
                    _cooldownTimer = 0;
                    _cooldownLengthRandom = 0;
                }
                else
                {
                    _cooldownTimer = Time.time - Random.Range(_cooldownLength / 4, _cooldownLength / 1.5f);
                    _cooldownLengthRandom = Random.Range(_cooldownLength * 0.85f, _cooldownLength * 1.15f);
                }
            }
            //Checks if enough time has passed since the last time the attack has been used
            //Also sets the cooldown length to a random number close to the num set in the inspector to make attacks feel less perfectly rythmic 
            if (Time.time - _cooldownTimer >= _cooldownLengthRandom)
            {
                _cooldownLengthRandom = Random.Range(_cooldownLength * 0.85f, _cooldownLength * 1.15f);
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
}
