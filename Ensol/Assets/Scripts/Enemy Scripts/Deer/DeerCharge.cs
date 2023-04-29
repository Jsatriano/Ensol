using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerCharge : Node
{
    //Charge Vars
    private float _chargeMaxSpeed; //How fast the charge is
    private float _chargeAccel;    //How fast the charge gets to max speed
    private float _chargeTurning;  //How well the enemy tracks the player during windup
    private float _rotation;       //How much the deer rotates to face the player during windup
    private Vector3 target;
    private Vector3 toPlayer;

    //Used for ending the charge
    private Vector3 _startingPosition; //Used to check if the enemy has passed the player
    private float _stuckTime;   //Used to check if the deer has been stuck on something for too long
    private float _chargeTime;  //Used to check if the deer has been charing for too long
    private bool _chargeEnding;

    //Components
    private Transform _playerTF;     //Player transform
    private Transform _deerTF;       //Deer transform
    private BoxCollider _hitZone;    //Hitzone of charge attack
    private Rigidbody _deerRB;       //Deer rigidbody
    private LayerMask _obstacleMask; //Obstacle layermask for obstacle avoidance when charging

    public DeerCharge(float chargeMaxSpeed, float chargeAccel, Transform playerTF, Transform deerTF, Rigidbody deerRB, BoxCollider hitZone, 
                      float chargeTurning, float rotation, LayerMask obstacleMask)
    {
        _chargeMaxSpeed = chargeMaxSpeed;
        _chargeAccel    = chargeAccel;
        _playerTF       = playerTF;
        _deerTF         = deerTF;
        _deerRB         = deerRB;
        _hitZone        = hitZone;
        _chargeTurning  = chargeTurning / 1000;
        _rotation       = rotation / 10;
        _obstacleMask   = obstacleMask;
        _chargeEnding   = false;
    }

    //Deer charge attack - RYAN
    public override NodeState Evaluate()
    {
        //Picking the deer's target (either the player or one of their breadcrumbs)
        target = _playerTF.position;
        List<Vector3> breadcrumbs = (List<Vector3>)GetData("breadcrumbs");
        //Uses player's current position as the target if they are currently in FOV
        if (breadcrumbs == null || !Physics.Linecast(_deerTF.position, _playerTF.position, _obstacleMask))
        {
            target = _playerTF.position;
        }
        //Else use the breadcrumbs
        else
        {
            bool foundTarget = false;
            //Check if any of the breadcrumbs are in FOV, starting at the most recent one
            for (int i = breadcrumbs.Count - 1; i >= 0; i--)
            {
                if (!Physics.Linecast(_deerTF.position, breadcrumbs[i], _obstacleMask))
                {
                    target = breadcrumbs[i];
                    foundTarget = true;
                    break;
                }
            }
            //Use oldest breadcrumb as target if none are within FOV
            if (!foundTarget)
            {
                if (breadcrumbs.Count <= 0)
                {
                    target = _playerTF.position;
                }
                else
                {
                    target = breadcrumbs[0];
                }
            }
        }
        toPlayer = new Vector3(target.x - _deerTF.position.x, 0, target.z - _deerTF.position.z).normalized;

        //Charge windup, has deer look at player and stores a target position to charge towards based on the player's current position
        if (GetData("chargingAnim") == null && !_chargeEnding)
        {
            //Gradually turns deer to face player
            float rotationMultiplier = Mathf.Clamp(Vector3.Dot(_deerTF.forward, toPlayer), 0.5f, 1);
            _deerTF.forward = Vector3.Lerp(_deerTF.forward, toPlayer, _rotation * rotationMultiplier);
            _startingPosition = _deerTF.position;

            //Setups/ticks up timers
            _stuckTime = 0;
            _chargeTime = 0;
            _chargeEnding = false;

            SetData("charging", true);
            SetData("attacking", true);
            SetData("chargeWindupAnim", true);
            ClearData("movingForward");
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            _chargeTime += Time.deltaTime;
            //Counts up stuck timer when deer is moving too slow while charging
            if (_deerRB.velocity.magnitude < 0.5f)
            {
                _stuckTime += Time.deltaTime;
            }
            //Checks if the deer is stuck on something or has been charging too long
            if (_stuckTime > 0.75f || _chargeTime > 4)
            {
                _chargeEnding = true;
            }
            //Checks to see if deer has charged past its target position, if so then charge is over
            if (Vector3.Distance(_startingPosition, _deerTF.position) > Vector3.Distance(_startingPosition, _playerTF.position) || GetData("attackHit") != null || _chargeEnding) 
            {
                //Stops the charging anim once the deer slows down enough (so they play the anim when barely moving)
                if (_deerRB.velocity.magnitude <= 6f)
                {
                    ClearData("chargingAnim");
                }
                
                _deerRB.drag  = 1f;
                _chargeEnding = true;
                //Doesn't stop charge until deer has slowed down more
                if (_deerRB.velocity.magnitude <= 0.25f)
                {
                    _hitZone.enabled = false; // disables enemy damage hitbox
                    _chargeEnding = false;
                    ClearData("charging");
                    ClearData("attacking");
                    ClearData("attackHit");
                    state = NodeState.SUCCESS;
                    return state;
                }
                state = NodeState.RUNNING;
                return state;
            }
            //Steers deer towards player, keeps the hitzone enabled, and pushes deer forward
            ChangeDirection();
            _hitZone.enabled = true;
            _deerRB.drag = 1;
            _deerRB.AddForce(_deerTF.forward * _chargeAccel, ForceMode.Acceleration);
            if (_deerRB.velocity.magnitude > _chargeMaxSpeed)
            {
                _deerRB.velocity = Vector3.ClampMagnitude(_deerRB.velocity, _chargeMaxSpeed);
            }
            state = NodeState.RUNNING;
            return state;
        }
    }

    //Uses context based movement to curve deer towards player or around obstacles by checking 3 forward directions and seeing which is most desirable
    private void ChangeDirection()
    {
        //Defines the directions that point slightly to the left and right of player
        Vector3 _forwardLeft  = (_deerTF.forward + (_deerTF.right * -_chargeTurning)).normalized;
        Vector3 _forwardRight = (_deerTF.forward + (_deerTF.right * _chargeTurning)).normalized;

        //Defines the direction from the deer to the player
        bool obstacleLeft, obstacleForward, obstacleRight;
        float leftWeight, forwardWeight, rightWeight;
        float dotLeft, dotForward, dotRight;

        //Checks if there are obstacles in the way of the three forward directions
        obstacleLeft    = Physics.Raycast(_deerTF.position, _forwardLeft, 100f, _obstacleMask);
        obstacleForward = Physics.Raycast(_deerTF.position, _deerTF.forward, 100f, _obstacleMask);
        obstacleRight   = Physics.Raycast(_deerTF.position, _forwardRight, 100f, _obstacleMask);

        //Gets the dot product between the three forward directions and the direction to the player
        dotLeft    = Vector3.Dot(_forwardLeft, toPlayer);
        dotForward = Vector3.Dot(_deerTF.forward, toPlayer);
        dotRight   = Vector3.Dot(_forwardRight, toPlayer);

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
