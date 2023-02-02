using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Charge : Node
{
    //Charge Vars
    private float _chargeSpeed;   //How fast the charge is
    private float _windupLength;  //How long the windup for the charge is
    private float _windupTimer;   //Used internally to keep track of the windup
    private float _chargeTurning; //How well the enemy tracks the player during windup

    //Used for ending the charge
    private Vector3 _startingPosition; //Used to check if the enemy has passed the player
    private float _stuckTime;   //Used to check if the deer has been stuck on something for too long
    private float _chargeTime;  //Used to check if the deer has been charing for too long

    //Components
    private Transform _playerTF;     //Player transform
    private Transform _deerTF;       //Deer transform
    private BoxCollider _hitZone;    //Hitzone of charge attack
    private Rigidbody _deerRB;       //Deer rigidbody
    private LayerMask _obstacleMask; //Obstacle layermask for obstacle avoidance when charging

    //Temp vars
    private Material _windupMaterial;
    private Material _attackMaterial;
    private Material _deerMaterial;
    private MeshRenderer _enemyMaterial;


    public Charge(float chargeSpeed, float chargeWindupLength, Transform playerTF, 
                  Transform deerTF, Rigidbody deerRB, BoxCollider hitZone, float chargeTurning, LayerMask obstacleMask, 
                  MeshRenderer enemyMaterial, Material windupMaterial, Material attackMaterial, Material deerMaterial)
    {
        _chargeSpeed   = chargeSpeed;
        _windupLength  = chargeWindupLength;
        _windupTimer   = 0;
        _playerTF      = playerTF;
        _deerTF        = deerTF;
        _deerRB        = deerRB;
        _hitZone       = hitZone;
        _chargeTurning = chargeTurning / 10;
        _obstacleMask  = obstacleMask;
        _enemyMaterial = enemyMaterial;
        _windupMaterial = windupMaterial;
        _attackMaterial = attackMaterial;
        _deerMaterial = deerMaterial;
    }

    //Deer charge attack - RYAN
    public override NodeState Evaluate()
    {
        //Charge windup, has deer look at player and stores a target position to charge towards based on the player's current position;
        if (_windupTimer < _windupLength)
        {
            //Gradually turns deer to face player
            Vector3 toPlayer  = (_playerTF.position - _deerTF.position).normalized;
            _deerTF.forward   = Vector3.Lerp(_deerTF.forward, toPlayer, (_windupTimer / _windupLength) * 1.5f);       
            _startingPosition = _deerTF.position;

            //Setups/ticks up timers
            _stuckTime    = 0;
            _chargeTime   = 0;
            _windupTimer += Time.deltaTime;

            _enemyMaterial.material = _windupMaterial;
            SetData("charging", true);
            SetData("attacking", true);
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            _enemyMaterial.material = _attackMaterial;
            _chargeTime += Time.deltaTime;
            //Counts up stuck timer when deer is moving too slow while charging
            if (_deerRB.velocity.magnitude < 0.5f)
            {
                _stuckTime += Time.deltaTime;
            }
            //Checks if the deer is stuck on something or has been charging too long
            if (_stuckTime > 0.75f || _chargeTime > 4)
            {
                _hitZone.enabled = false;        
                _windupTimer = 0;
                _enemyMaterial.material = _deerMaterial;
                ClearData("charging");
                ClearData("attacking");
                state = NodeState.FAILURE;
                return state;
            }
            //Checks to see if deer has charged past its target position, if so then charge is over
            if (Vector3.Distance(_startingPosition, _deerTF.position) > Vector3.Distance(_startingPosition, _playerTF.position)) 
            {
                //Doesn't stop charge until deer has slowed down more
                if(_deerRB.velocity.magnitude <= 1)
                {
                    _hitZone.enabled = false; // disables enemy damage hitbox
                    _windupTimer = 0;
                    _enemyMaterial.material = _deerMaterial;
                    ClearData("charging");
                    ClearData("attacking");
                    state = NodeState.SUCCESS;
                    return state;
                }
                state = NodeState.RUNNING;
                return state;
            }
            //Steers deer towards player, keeps the hitzone enabled, and pushes deer forward
            ChangeDirection();
            _hitZone.enabled = true;
            _deerRB.AddForce(_deerTF.forward * _chargeSpeed * Time.deltaTime * 100, ForceMode.Acceleration);    
            state = NodeState.RUNNING;
            return state;
        }
    }

    //Uses context based movement to curve deer towards player or around obstacles by checking 3 forward directions and seeing which is most desirable
    private void ChangeDirection()
    {
        //Defines the directions that point slightly to the left and right of player
        Vector3 _forwardLeft  = (_deerTF.forward + (_deerTF.right * -_chargeTurning * Time.deltaTime)).normalized;
        Vector3 _forwardRight = (_deerTF.forward + (_deerTF.right * _chargeTurning * Time.deltaTime)).normalized;

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
