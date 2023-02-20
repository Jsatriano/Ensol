using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerBasicAttack : Node
{
    private Transform _playerTF; //Player transform
    private Transform _enemyTF;  //Enemy transform
    private BoxCollider _hitBox; //Hitbox of attack
    private float _rotation;

    //The deer's basic attack, has a short windup and then sticks out a hitbox for a provided length - RYAN
    public DeerBasicAttack(BoxCollider hitBox, Transform playerTF, Transform enemyTF, float rotation)
    {
        _hitBox       = hitBox;
        _playerTF     = playerTF;
        _enemyTF      = enemyTF;
        _rotation     = rotation / 10;
    }

    public override NodeState Evaluate()
    {
        //Windup of the attack, turns deer to look at player
        if (GetData("endSwipeWindup") == null)
        { 
            Vector3 toPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, _rotation);
            SetData("basic", true);
            SetData("attacking", true);
            SetData("swipingAnim", true);
            state = NodeState.RUNNING;
            return state;
        } 
        else
        {
            //Checks if attack is over, resets all the vars
            if (GetData("endSwipe") != null)
            {
                _hitBox.enabled = false;
                ClearData("basic");
                ClearData("attacking");
                ClearData("swipingAnim");
                ClearData("endSwipeWindup");
                ClearData("endSwipe");
                state = NodeState.SUCCESS;
                return state;
            }
            //Keeps all the hitboxes on and increments timer while attacking
            _hitBox.enabled = true;
            state = NodeState.RUNNING;
            return NodeState.RUNNING;
        }
    }
}
