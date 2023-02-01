using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerBasicAttack : Node
{
    private BoxCollider _hitBox;
    private float _attackLength;
    private float _attackTimer;
    private float _windupLength;
    private float _windupTimer;
    private Transform _playerTF;
    private Transform _enemyTF;

    public DeerBasicAttack(BoxCollider hitBox, float attackLength, float basicWindup, Transform playerTF, Transform enemyTF)
    {
        _hitBox = hitBox;
        _attackLength = attackLength;
        _windupTimer = 0;
        _playerTF = playerTF;
        _enemyTF = enemyTF;
    }

    public override NodeState Evaluate()
    {
        if (_windupTimer < _windupLength)
        {
            SetData("Attacking", true);
            Vector3 toPlayer = (_playerTF.position - _enemyTF.position).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, (_windupTimer / _windupLength) * 0.9f);
            _windupTimer += Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        } 
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
