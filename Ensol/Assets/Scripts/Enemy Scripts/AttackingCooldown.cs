using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackingCooldown : Node
{
    private float _attackingCooldown;
    private float _timer;
    
    public AttackingCooldown(float attackingCooldown)
    {
        _attackingCooldown = attackingCooldown;
        _timer = _attackingCooldown;
    }

    public override NodeState Evaluate()
    {
        //Checks if enemy is already attacking and automically passes if it is the attack this node is for (to prevent interrupting it)
        //Also keeps setting the timer so that it only starts counting down once the attack ends
        string attack = (string)GetData("attacking");
        if (attack != null)
        {
            _timer = Time.time;
            state = NodeState.SUCCESS;
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
