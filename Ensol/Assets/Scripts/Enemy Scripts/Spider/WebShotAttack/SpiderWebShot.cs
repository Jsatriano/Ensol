using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpiderWebShot : Node
{
    private string _attackName;
    private WebShotManager _webShotManager;
    private Transform _playerTF;
    private Transform _enemyTF;
    private float _rotation;

    public SpiderWebShot(string attackName, WebShotManager webShotManger, Transform playerTF, Transform enemyTF, float webRotation)
    {
        _attackName = attackName;
        _webShotManager = webShotManger;
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _rotation = webRotation;
    }

    public override NodeState Evaluate()
    {
        if ((string)GetData("animation") != _attackName)
        {
            SetData("animation", _attackName);
            SetData("attacking", _attackName);
            _webShotManager.StartWebShotAttack();
            state = NodeState.RUNNING;
            return state;
        }
        if (GetData("webShotEnded") != null)
        {
            ClearData("webShotEnded");
            ClearData("attacking");
            ClearData("animation");
            state = NodeState.SUCCESS;
            return state;
        }
        RotateTowardsPlayer();
        state = NodeState.RUNNING;
        return state;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 toPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z).normalized;
        float angle = Vector3.Angle(_enemyTF.forward, toPlayer);
        if (angle < _rotation)
        {
            _enemyTF.forward = toPlayer;
        }
        else
        {
            if (Vector3.Dot(_enemyTF.right, toPlayer) > Vector3.Dot(-_enemyTF.right, toPlayer))
            {
                _enemyTF.rotation = Quaternion.AngleAxis(_rotation, _enemyTF.up) * _enemyTF.rotation;
            }
            else
            {
                _enemyTF.rotation = Quaternion.AngleAxis(_rotation, -_enemyTF.up) * _enemyTF.rotation;
            }
        }
    }
}
