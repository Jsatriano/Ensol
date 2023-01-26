using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class FOVCheck : Node
{
    private static int _envLayerMask = 1 << 0;
    private Transform _enemyTF;
    private Transform _playerTF;
    private float _visionRange;
    private RaycastHit _hit;
    private string _attackName;

    public FOVCheck(Transform enemyTF, Transform playerTF, float visionRange, string attackName)
    {
        _enemyTF = enemyTF;
        _playerTF = playerTF;
        _visionRange = visionRange;
        _attackName = attackName;
    }

    //Checks to see if enemy can see the player or if they have already seen the player - RYAN
    public override NodeState Evaluate()
    {
        //Automatically returns success if the attack is already running to prevent prematurely terminating attacks
        if (GetData(_attackName) != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        //Checks if enemy has already seen player
        object player = GetData("player");
        if (player == null)
        {
            //Checks if player is within range of the enemy
            if (Vector3.Distance(_enemyTF.position, _playerTF.position) <= _visionRange)
            {
                //checks if enemy has LOS of player, if so returns success
                _hit = new RaycastHit();
                if (!Physics.Linecast(_enemyTF.position, _playerTF.position, out _hit, _envLayerMask))
                {
                    SetData("player", _playerTF);
                    state = NodeState.SUCCESS;
                    return state;
                }  
            }
            state = NodeState.FAILURE;
            return state;
        }
        //Doesn't start attacking unless player is in LOS
        if (!Physics.Linecast(_enemyTF.position, _playerTF.position, out _hit, _envLayerMask))
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
