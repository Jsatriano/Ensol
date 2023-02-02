using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RangeCheck : Node
{
    private Transform _enemyTF;
    private Transform _playerTF;
    private float _attackRange;
    private string _attackName;

    //Node for checking if the enemy is close enough to the player to do some attack - RYAN

    public RangeCheck(Transform enemyTF, Transform playerTF, float attackRange, string attackName)
    {
        _enemyTF = enemyTF;
        _playerTF = playerTF;
        _attackRange = attackRange;
        _attackName = attackName;
    }

    public override NodeState Evaluate()
    {
        //Checks if enemy is already doing the attack (automatically passes if so)
        if (GetData(_attackName) != null)
        {
            state = NodeState.SUCCESS;
            return state;
        } 
        //Checks if enemy is currently doing a different attack (automatically fails if so)
        else if (GetData("attacking") != null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            //Calculates the distance between the enemy and the player, returning success if they are within attack range otherwise failure
            float distToPlayer = (_playerTF.position - _enemyTF.position).magnitude;
            if (distToPlayer <= _attackRange)
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
