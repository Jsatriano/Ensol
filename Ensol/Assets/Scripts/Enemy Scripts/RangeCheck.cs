using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RangeCheck : Node
{
    private Transform _enemyTF;  //Enemy transform
    private Transform _playerTF; //Player transform
    private float _attackRange;  //How close the player needs to be for the enemy to start the attack
    private string _attackName;  //Name of the attack. Used to check if the enemy is already attacking

    //Node for checking if the enemy is close enough to the player to do some attack - RYAN

    public RangeCheck(Transform enemyTF, Transform playerTF, float attackRange, string attackName)
    {
        _enemyTF     = enemyTF;
        _playerTF    = playerTF;
        _attackRange = attackRange;
        _attackName  = attackName;
    }

    public override NodeState Evaluate()
    {
        //Checks if enemy is already attacking and automically passes if it is the attack this node is for (to prevent interrupting it)
        string attack = (string)GetData("attacking");
        if (attack != null)
        {
            if (attack == _attackName)
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
        else
        {
            //Calculates the distance between the enemy and the player, returning success if they are within attack range otherwise failure
            //float distToPlayer = (_playerTF.position - _enemyTF.position).magnitude;
            Vector3 dirToPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z);
            float distToPlayer = dirToPlayer.magnitude;
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
