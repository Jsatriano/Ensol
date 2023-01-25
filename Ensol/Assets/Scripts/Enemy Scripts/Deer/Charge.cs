using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Charge : Node
{
    private float _chargeSpeed;
    private BoxCollider _chargeHitbox;
    private float _windupLength;
    private float _windupTimer;
    private float _cooldownLength;
    private float _cooldownTimer;
    private bool _chargeStarted;
    private Transform _playerTF;
    private Transform _deerTF;
    private Vector3 _targetPosition;
    private Vector3 _startingPosition;
    private Rigidbody _deerRB;

    public Charge(float chargeSpeed, BoxCollider chargeHitbox, float chargeWindupLength, float cooldownLength, Transform playerTF, Transform deerTF, Rigidbody deerRB)
    {
        _chargeSpeed = chargeSpeed;
        _chargeHitbox = chargeHitbox;
        _windupLength = chargeWindupLength;
        _windupTimer = Time.time;
        _cooldownLength = cooldownLength;
        _cooldownTimer = Time.time;
        _chargeStarted = false;
        _playerTF = playerTF;
        _deerTF = deerTF;
        _deerRB = deerRB;
    }

    public override NodeState Evaluate()
    {
        //Check to see if charge is off cooldown, returns false if no
        if (Time.time - _cooldownTimer < _cooldownLength)
        {
            _chargeStarted = false;
            _windupTimer = Time.time;
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            //If the charge is still in winduup, have the deer stand still and look at player
            if (Time.time - _windupTimer < _windupLength)
            {
                _deerTF.LookAt(_playerTF);
                _targetPosition = _playerTF.position;
                _startingPosition = _deerTF.position;
                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                //Checks to see if deer has charged past its target position, if so then charge is over
                if (Vector3.Distance(_startingPosition, _deerTF.position) > Vector3.Distance(_startingPosition,_targetPosition)) 
                {
                    _cooldownTimer = Time.time;
                    state = NodeState.SUCCESS;
                    return state;
                }
                //Makes deer charge forwards
                _deerRB.AddForce(_deerTF.forward * _chargeSpeed * Time.deltaTime * 100, ForceMode.Acceleration);
                state = NodeState.RUNNING;
                return state;
            }
        }
    }
}
