using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearJunkThrow : Node
{
    private Transform _playerTF;
    private Transform _enemyTF;
    private float _rotation;
    private bool _grabbedBall;


    public BearJunkThrow(Transform playerTF, Transform enemyTF, float rotation)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _rotation = rotation;
        _grabbedBall = false;
    }

    public override NodeState Evaluate()
    {
        if (_grabbedBall && GetData("grabBall") == null) 
        {
            Vector3 toPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, _rotation);
            SetData("throwingAnim", true);
        }
        else if (!_grabbedBall && GetData("grabBall") != null)
        {
            //Spawn in ball
        }

        return base.Evaluate();
    }
}
