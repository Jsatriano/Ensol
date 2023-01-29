using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Charge : Node
{
    private float _chargeSpeed;
    private BoxCollider _hitZone;
    private float _windupLength;
    private float _windupTimer;
    private Transform _playerTF;
    private Transform _deerTF;
    private Vector3 _startingPosition;
    private Rigidbody _deerRB;
    private float _stuckTime;
    private float _chargeTime;
    private Vector3 _forwardRight;
    private Vector3 _forwardLeft;
    private float _chargeTurning;
    private LayerMask _obstacleMask = 1 << 7;


    public Charge(float chargeSpeed, float chargeWindupLength, Transform playerTF, 
                  Transform deerTF, Rigidbody deerRB, BoxCollider hitZone, float chargeTurning)
    {
        _chargeSpeed   = chargeSpeed;
        _windupLength  = chargeWindupLength;
        _windupTimer   = Time.time;
        _playerTF      = playerTF;
        _deerTF        = deerTF;
        _deerRB        = deerRB;
        _hitZone       = hitZone;
        _chargeTurning = chargeTurning / 10;
    }

    //Deer charge attack - RYAN
    public override NodeState Evaluate()
    {
        //Charge windup, has deer look at player and stores a target position to charge towards based on the player's current position;
        if (_windupTimer < _windupLength)
        {
            //Gradually turns deer to face player
            Vector3 toPlayer = (_playerTF.position - _deerTF.position).normalized;
            _deerTF.forward = Vector3.Lerp(_deerTF.forward, toPlayer, _windupTimer);       
            _startingPosition = _deerTF.position;

            _stuckTime = 0;
            _chargeTime = 0;
            _windupTimer += Time.deltaTime;

            SetData("charging", true);
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

            if (_deerRB.velocity.magnitude < 0.5f)
            {
                _stuckTime += Time.deltaTime;
            }

            //Checks if the deer is stuck on something or has been charging too long
            if (_stuckTime > 0.75f || _chargeTime > 4)
            {
                _hitZone.enabled = false;
                ClearData("charging");
                _windupTimer = 0;
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.RUNNING;
            return state;
        }
    }

    //Uses context based movement to curve deer towards player or around obstacles by checking 3 forward directions and seeing which is most desirable
    private void ChangeDirection()
    {
        //Defines the directions that point slightly to the left and right of player
        _forwardLeft  = (_deerTF.forward + (_deerTF.right * -_chargeTurning * Time.deltaTime)).normalized;
        _forwardRight = (_deerTF.forward + (_deerTF.right * _chargeTurning * Time.deltaTime)).normalized;

        //Defines the direction from the deer to the player
        Vector3 toPlayer = (_playerTF.position - _deerTF.position).normalized;
        bool obstacleLeft, obstacleForward, obstacleRight;
        float leftWeight, forwardWeight, rightWeight;
        float dotLeft, dotForward, dotRight;

        //Checks if there are obstacles in the way of the three forward directions
        obstacleLeft    = Physics.Raycast(_deerTF.position, _forwardLeft, 100f, _obstacleMask);
        obstacleForward = Physics.Raycast(_deerTF.position, _deerTF.forward, 100f, _obstacleMask);
        obstacleRight   = Physics.Raycast(_deerTF.position, _forwardRight, 100f, _obstacleMask);

        //Gets the dot product between the three forward directions and the direction to the player
        dotLeft = Vector3.Dot(_forwardLeft, toPlayer);
        dotForward = Vector3.Dot(_deerTF.forward, toPlayer);
        dotRight = Vector3.Dot(_forwardRight, toPlayer);

        //Gets how desirable each direction is based on if there are obstacles in the way and
        //how close each direction is to the direction to player
        leftWeight    = CalculateWeight(obstacleLeft, dotLeft);
        forwardWeight = CalculateWeight(obstacleForward, dotForward);
        rightWeight   = CalculateWeight(obstacleRight, dotRight);

        //Checks if left or right are more desirable and if so sets the deers transform.forward to that direction
        if (leftWeight > forwardWeight && leftWeight >= rightWeight)
        {
            _deerTF.forward = _forwardLeft;
        }
        else if (rightWeight > forwardWeight && rightWeight > leftWeight)
        {
            _deerTF.forward = _forwardRight;
        }
        else
        {
            //If forward is deemed most desirable, but all directions are blocked then pick left or right depending on
            //which has the best dot product result
            if (obstacleLeft && obstacleForward && obstacleRight)
            {
                if (dotLeft >= dotRight)
                {
                    _deerTF.forward = _forwardLeft;
                }
                else
                {
                    _deerTF.forward = _forwardRight;
                }
            }
        }
    }

    //Calculates the desirablity of a given direction based on if there is an obstacle in the
    //way and its dot product with the direction to the player
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
