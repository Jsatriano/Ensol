using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerCharge : Node
{
    //Charge Vars
    private float _chargeMaxSpeed; //How fast the charge is
    private float _chargeAccel;    //How fast the charge gets to max speed
    private float _windupRotation;       //How much the deer rotates to face the player during windup
    private float _chargeRotation;
    private Vector3 target;
    private Vector3 toPlayer;
    private string _attackname;

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
                      float chargeRotation, float windupRotation, LayerMask obstacleMask, string attackName)
    {
        _chargeMaxSpeed = chargeMaxSpeed;
        _chargeAccel    = chargeAccel;
        _playerTF       = playerTF;
        _deerTF         = deerTF;
        _deerRB         = deerRB;
        _hitZone        = hitZone;
        _chargeRotation = chargeRotation;
        _windupRotation = windupRotation;
        _obstacleMask   = obstacleMask;
        _chargeEnding   = false;
        _attackname = attackName;
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
            ChangeDirection(_windupRotation);
            _startingPosition = _deerTF.position;

            //Setups/ticks up timers
            _stuckTime = 0;
            _chargeTime = 0;
            _chargeEnding = false;

            SetData("attacking", _attackname);
            SetData("chargeWindupAnim", true);
            ClearData("movingForward");
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            _chargeTime += Time.deltaTime;
            ChangeDirection(_chargeRotation);
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
            if (_chargeEnding || Vector3.Distance(_startingPosition, _deerTF.position) > Vector3.Distance(_startingPosition, _playerTF.position) || GetData("attackHit") != null) 
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
                    ResetVars();
                    state = NodeState.SUCCESS;
                    return state;
                }
                state = NodeState.RUNNING;
                return state;
            }
            //Steers deer towards player, keeps the hitzone enabled, and pushes deer forward
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

    private void ChangeDirection(float maxAngle)
    {
        float angle = Vector3.Angle(_deerTF.forward, toPlayer);
        if (angle < maxAngle)
        {
            _deerTF.forward = toPlayer;
        }
        else
        {
            if (Vector3.Dot(_deerTF.right, toPlayer) > Vector3.Dot(-_deerTF.right, toPlayer))
            {
                _deerTF.rotation = Quaternion.AngleAxis(maxAngle, _deerTF.up) * _deerTF.rotation;
            }
            else
            {
                _deerTF.rotation = Quaternion.AngleAxis(maxAngle, -_deerTF.up) * _deerTF.rotation;
            }
        }
    }

    private void ResetVars()
    {
        _hitZone.enabled = false; // disables enemy damage hitbox
        _chargeEnding = false;
        ClearData("attacking");
        ClearData("attackHit");
    }
}
