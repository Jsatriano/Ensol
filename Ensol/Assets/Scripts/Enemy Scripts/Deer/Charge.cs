using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Charge : Node
{
    private float _chargeSpeed;
    private BoxCollider _chargeHitbox;
    private BoxCollider _hitZone;
    private float _windupLength;
    private float _windupTimer;
    private Transform _playerTF;
    private Transform _deerTF;
    private Vector3 _targetPosition;
    private Vector3 _startingPosition;
    private Rigidbody _deerRB;
    private float _chargeTime;


    public Charge(float chargeSpeed, BoxCollider chargeHitbox, float chargeWindupLength, Transform playerTF, 
                  Transform deerTF, Rigidbody deerRB, BoxCollider hitZone)
    {
        _chargeSpeed = chargeSpeed;
        _chargeHitbox = chargeHitbox;
        _windupLength = chargeWindupLength;
        _windupTimer = Time.time;
        _playerTF = playerTF;
        _deerTF = deerTF;
        _deerRB = deerRB;
        _hitZone = hitZone;
    }

    //Deer charge attack - RYAN
    public override NodeState Evaluate()
    {
        //Charge windup, has deer look at player and stores a target position to charge towards based on the player's current position;
        if (_windupTimer < _windupLength)
        {
            _windupTimer += Time.deltaTime;
            SetData("charging", true);
            _deerTF.LookAt(_playerTF);
            _targetPosition = _playerTF.position;
            _startingPosition = _deerTF.position;
            _chargeTime = 0;
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            //Checks to see if deer has charged past its target position, if so then charge is over
            if (Vector3.Distance(_startingPosition, _deerTF.position) > Vector3.Distance(_startingPosition, _targetPosition)) 
            {
                if(_deerRB.velocity.magnitude <= 1)
                {
                    _hitZone.enabled = false; // disables enemy damage hitbox
                    ClearData("charging");
                    _windupTimer = 0;
                    state = NodeState.SUCCESS;
                    return state;
                }
                state = NodeState.RUNNING;
                return state;
            }
            //Makes deer charge forwards
            _deerRB.AddForce(_deerTF.forward * _chargeSpeed * Time.deltaTime * 100, ForceMode.Acceleration);
            _chargeTime += Time.deltaTime;

            // enables damage hitbox
            _hitZone.enabled = true;

            //Checks if the deer has been charging for too long (bandaid fix for getting stuck on walls)
            if (_chargeTime > 2)
            {
                ClearData("charging");
                _windupTimer = 0;
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.RUNNING;
            return state;
        }
    }
}
