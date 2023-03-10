using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class FOVCheck : Node
{
    private LayerMask _envLayerMask; //Layer(s) the environment is on
    private Transform _enemyTF;      //Enemy transform
    private Transform _playerTF;     //Player transform
    private float _visionRange;      //Detection range of the enemy
    private string _attackName;      //Name of the attack this check is for
    private int _enemyType;          //The enemy type

    public FOVCheck(Transform enemyTF, Transform playerTF, float visionRange, string attackName, LayerMask envLayerMask, int enemyType)
    {
        _enemyTF      = enemyTF;
        _playerTF     = playerTF;
        _visionRange  = visionRange;
        _attackName   = attackName;
        _envLayerMask = envLayerMask;
        _enemyType    = enemyType;
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
        //Checks if enemy is currently doing a different attack (automatically fails if so)
        else if (GetData("attacking") != null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            
            //Checks if enemy has already seen player
            if (GetData("player") == null)
            {
                //Checks if player is within range of the enemy
                if (Vector3.Distance(_enemyTF.position, _playerTF.position) <= _visionRange)
                {                   
                    //checks if enemy has LOS of player, if so returns success
                    if (!Physics.Linecast(_enemyTF.position, _playerTF.position, _envLayerMask))
                    {
                        SetData("player", _playerTF);
                        state = NodeState.SUCCESS;
                        if (_enemyType == 1){
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.deerAlerted, _enemyTF.position);
                        }
                        return state;
                    }  
                }
                state = NodeState.FAILURE;
                return state;
            }
            //Doesn't start attacking unless player is in LOS
            if (!Physics.Linecast(_enemyTF.position, _playerTF.position, _envLayerMask))
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
