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
    private Vector3 _startingPosition;
    private Rigidbody _deerRB;
    private float _chargeTime;
    private Vector3 _forwardRight;
    private Vector3 _forwardLeft;
    private float _chargeTurning;
    private LayerMask _obstacleMask = 1 << 7;


    public Charge(float chargeSpeed, BoxCollider chargeHitbox, float chargeWindupLength, Transform playerTF, 
                  Transform deerTF, Rigidbody deerRB, BoxCollider hitZone, float chargeTurning)
    {
        _chargeSpeed   = chargeSpeed;
        _chargeHitbox  = chargeHitbox;
        _windupLength  = chargeWindupLength;
        _windupTimer   = Time.time;
        _playerTF      = playerTF;
        _deerTF        = deerTF;
        _deerRB        = deerRB;
        _hitZone       = hitZone;
        _chargeTurning = chargeTurning / 10000;
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
            _startingPosition = _deerTF.position;
            _chargeTime = 0;
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            //Checks to see if deer has charged past its target position, if so then charge is over
            if (Vector3.Distance(_startingPosition, _deerTF.position) > Vector3.Distance(_startingPosition, _playerTF.position)) 
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

            ChangeDirection();

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

    private void ChangeDirection()
    {
        _forwardLeft  = (_deerTF.forward + (_deerTF.right * -_chargeTurning)).normalized;
        _forwardRight = (_deerTF.forward + (_deerTF.right * _chargeTurning)).normalized;

        Vector3 toPlayer = (_playerTF.position - _deerTF.position).normalized;

        float leftWeight, forwardWeight, rightWeight;
        leftWeight    = CalculateWeight(Physics.Raycast(_deerTF.position, _forwardLeft, 100f, _obstacleMask), Vector3.Dot(_forwardLeft, toPlayer));
        forwardWeight = CalculateWeight(Physics.Raycast(_deerTF.position, _deerTF.forward, 100f, _obstacleMask), Vector3.Dot(_deerTF.forward, toPlayer));
        rightWeight   = CalculateWeight(Physics.Raycast(_deerTF.position, _forwardRight, 100f, _obstacleMask), Vector3.Dot(_forwardRight, toPlayer));

        if (leftWeight > forwardWeight && leftWeight >= rightWeight)
        {
            _deerTF.forward = _forwardLeft;
        }
        else if (rightWeight > forwardWeight && rightWeight > leftWeight)
        {
            _deerTF.forward = _forwardRight;
        }
    }

    private float CalculateWeight(bool hitObstacle, float dotProduct)
    {
        if (hitObstacle)
        {
            return (0.25f * dotProduct);
        }
        else
        {
            return (dotProduct);
        }
    }
}
