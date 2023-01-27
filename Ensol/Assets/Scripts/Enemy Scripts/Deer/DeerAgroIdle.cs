using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerAgroIdle : Node
{
    private float _speed;
    private Transform _playerTF;
    private Transform _deerTF;
    private float _rotationSpeed;

    public DeerAgroIdle(float speed, Transform playerTF, Transform deerTF, float rotationSpeed)
    {
        _speed = speed;
        _playerTF = playerTF;
        _deerTF = deerTF;
        _rotationSpeed = rotationSpeed;
    }

    public override NodeState Evaluate()
    {
        if (GetData("player") != null)
        {
            Vector3 toPlayer = (_playerTF.position - _deerTF.position).normalized;
            float dot = Vector3.Dot(toPlayer, _deerTF.forward);
            if (dot < 1)
            {
                if (dot < 0)
                {
                    dot = 0;
                }
                _deerTF.forward = Vector3.Lerp(_deerTF.forward, toPlayer, _rotationSpeed * Time.deltaTime);
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
