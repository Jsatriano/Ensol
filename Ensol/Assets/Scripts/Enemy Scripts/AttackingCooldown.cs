using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackingCooldown : Node
{
    private float _attackingCooldown;
    private float _timer;
    private List<string> _attackNames;
    
    public AttackingCooldown(float attackingCooldown, List<string> attackNames)
    {
        _attackingCooldown = attackingCooldown;
        _timer = Time.time + _attackingCooldown;
        _attackNames = attackNames;
    }

    public override NodeState Evaluate()
    {
        //Checks if enemy is already attacking and automically passes if it is one of the attacks this cooldown is for, if it isn't then fail
        string attack = (string)GetData("attacking");
        if (attack != null)
        {
            for (int i = 0; i < _attackNames.Count; i++)
            {
                if (attack == _attackNames[i])
                {
                    _timer = Time.time;
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            if (Time.time - _timer >= _attackingCooldown)
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
}
