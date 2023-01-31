using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TrackPlayer : Node
{
    private float _speed;
    private Transform _playerTF;
    private Transform _enemyTF;
    private float _rotationSpeed;

    public TrackPlayer(Transform playerTF, Transform enemyTF, float rotationSpeed)
    {
        _playerTF      = playerTF;
        _enemyTF       = enemyTF;
        _rotationSpeed = rotationSpeed;
        
    }

    //Has the enemy try to look at the player. How well they track the player set by rotationSpeed - RYAN
    public override NodeState Evaluate()
    {
        if (GetData("player") != null)
        {
            Vector3 toPlayer = (_playerTF.position - _enemyTF.position).normalized;
            float dot = Vector3.Dot(toPlayer, _enemyTF.forward);
            //Lerps the enemies forward vector towards the direction of the player when they aren't already looking at them
            if (dot < 1)
            {
                _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, _rotationSpeed * Time.deltaTime);
            }
            state = NodeState.SUCCESS;
            return NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
