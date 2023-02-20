using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearSwipe : Node
{
    private Transform _playerTF; //Player transform
    private Transform _enemyTF;  //Enemy transform
    private Rigidbody _enemyRB;  //Rigidbody of the enemy
    private SphereCollider _hitBox1;  //Hitbox of attack  
    private SphereCollider _hitBox2;
    private float _originalDrag; //Used to remember what the enemies drag was originally
    private float _movement;
    private float _rotation;

    //The bear's basic attack, has a short windup and then sticks out a hitbox for a provided length - RYAN
    public BearSwipe(SphereCollider hitBox1, SphereCollider hitBox2, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float movement, float rotation)
    {
        _hitBox1      = hitBox1;
        _hitBox2      = hitBox2;
        _playerTF     = playerTF;
        _enemyTF      = enemyTF;
        _enemyRB      = enemyRB;
        _originalDrag = _enemyRB.drag;
        _movement     = movement;
        _rotation     = rotation / 40;
    }

    public override NodeState Evaluate()
    {
        //Windup of the attack, turns bear to look at player
        if (GetData("endWindup") == null)
        {
            Vector3 toPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, _rotation);
            SetData("swipe", true);
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
                _hitBox1.enabled = false;
                _hitBox2.enabled = false;
                _enemyRB.drag = _originalDrag;
                ClearData("swipe");
                ClearData("attacking");
                ClearData("endSwipe");
                ClearData("endWindup");
                ClearData("swipingAnim");
                state = NodeState.SUCCESS;
                return state;
            }
            //Moves the bear forward when attacking
            _enemyRB.drag = 1f;
            _enemyRB.AddForce(_enemyTF.forward * _movement);

            //Keeps all the hitboxes on and increments timer while attacking
            _hitBox1.enabled = true;
            _hitBox2.enabled = true;
            state = NodeState.RUNNING;
            return NodeState.RUNNING;
        }
    }
}
