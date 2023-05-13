using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearJunkThrow : Node
{
    private string _attackName;
    private JunkBallManager _junkBallManager;
    private Transform _playerTF;
    private Transform _enemyTF;
    private float _rotation;

    public BearJunkThrow(string attackName, JunkBallManager junkBallManager, Transform playerTF, Transform enemyTF, float junkRotation)
    {
        _attackName = attackName;
        _junkBallManager = junkBallManager;
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _rotation = junkRotation;
    }

    //All gameplay code for this attack takes place in JunkBallManager.cs and JunkBall.cs
    //This script just handles the animations and stages of the attack

    public override NodeState Evaluate()
    {
        //Windup for the attack
        if (GetData("throwingAnim") == null ) 
        {
            SetData("throwingAnim", true);
            SetData("attacking", _attackName);
            _junkBallManager.StartJunkThrow();
            state = NodeState.RUNNING;
            return state;
        }
        //Ends the attack when triggered by animation event
        else if (GetData("endThrow") != null)
        {
            ClearData("throwingAnim");
            ClearData("attacking");
            ClearData("endThrow");
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
