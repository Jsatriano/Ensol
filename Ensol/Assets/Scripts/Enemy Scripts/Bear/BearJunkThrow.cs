using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearJunkThrow : Node
{
    private Transform _playerTF;
    private Transform _enemyTF;
    private float _rotation;
    private bool _junkThrown;

    public BearJunkThrow(Transform playerTF, Transform enemyTF, float rotation)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _rotation = rotation / 40;
        _junkThrown = false;
    }

    public override NodeState Evaluate()
    {
        //Windup for the attack
        if (GetData("throwJunk") == null) 
        {
            Vector3 toPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, _rotation);
            SetData("throwingAnim", true);
            SetData("attacking", true);
            SetData("junk", true);
            state = NodeState.RUNNING;
            return state;
        }
        //Ends the attack when triggered by animation event
        else if (GetData("endThrow") != null)
        {
            ClearData("throwingAnim");
            ClearData("attacking");
            ClearData("junk");
            ClearData("throwJunk");
            ClearData("endThrow");
            state = NodeState.SUCCESS;
            return state;
        }
        //Don't do anything doing the time after ball is thrown but before animation ends
        state = NodeState.RUNNING;
        return state;
    }
}
