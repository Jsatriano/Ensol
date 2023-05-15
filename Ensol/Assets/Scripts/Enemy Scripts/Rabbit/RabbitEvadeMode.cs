using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RabbitEvadeMode : Node
{
    private Transform _playerTF;  //Player transform
    private Transform _enemyTF;   //Enemy transform
    private Rigidbody _enemyRB;   //Enemy rigidbody
    private float _acceleration;  //How fast the rabbit gets to max speed
    private float _maxSpeed;      //Max speed of the rabbit
    private float _minSpeed;      //Min speed the rabbit will go, the rabbit will fluctuate between this min and max speed based on how close the player is
    private float _rapidAvoidDist;
    private float _landingDrag;
    private float _normalDrag;
    private float _idealDistance; //The ideal distance the rabbit tries to stay from the player
    private Vector3 _dirToPlayer; //The direction from the enemy to the player
    private Vector3 movingDir;    //The rabbit's direction of movement
    private float _rotation; //How quickly the enemy turns (how well they can track the player)
    private float[] weights = new float[8];    //The context map for the deer
    private float[] zeroArray = new float[8];  //Used for resetting arrays to all zeroes
    private Vector3 _originalPos;
    private float _distanceToPlayer;

    public RabbitEvadeMode(float acceleration, float maxSpeed, float minSpeed, float rapidAvoidDist, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float idealDistance, 
                           float rotationSpeed, float landingDrag, float normalDrag)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _enemyRB = enemyRB;
        _maxSpeed = maxSpeed / 10;
        _acceleration = acceleration;
        _idealDistance = idealDistance;
        _rotation = rotationSpeed;
        movingDir = Vector3.zero;
        _landingDrag = landingDrag;
        _normalDrag = normalDrag;
        _originalPos = enemyTF.position;
        _minSpeed = minSpeed / 10;
        _rapidAvoidDist = rapidAvoidDist;
    }

    public override NodeState Evaluate()
    {
        ChooseDirection();

        if (GetData("applyLandingDrag") != null)
        {
            _enemyRB.drag = _landingDrag;
            RotateTowardsPlayer();
        }

        //Evade currently doesn't count leaps, so this is just making sure that aggro doesn't instantly count a leap when transferring to it
        if (GetData("incrementLeaps") != null)
        {
            ClearData("incrementLeaps");
        }

        //Makes sure the var for feet being on ground is set
        if (GetData("feetOnGround") == null)
        {
            SetData("feetOnGround", false);
        }

        //Doesn't accelerate or rotate rabbit while it is in the air
        if (!(bool)GetData("feetOnGround"))
        {
            state = NodeState.SUCCESS;
            return state;
        }

        _enemyRB.drag = _normalDrag;
        //Makes the rabbit move slower the less it is facing the direction it wants to move
        float speedDot = Vector3.Dot(_enemyTF.forward, movingDir);
        speedDot = (speedDot / 2) + 0.5f;
        speedDot = Mathf.Clamp(speedDot, 0.3f, 1);

        RotateTowardsPlayer();

        float evadeSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, 1 - Mathf.Clamp01(_distanceToPlayer / _rapidAvoidDist));

        //Moves rabbit in the desired direction (with a speed cap)
        if (_enemyRB.velocity.magnitude > evadeSpeed)
        {
            //Keep moving rabbit at max speed
            //_enemyRB.velocity = Vector3.ClampMagnitude(_enemyRB.velocity, evadeSpeed);
        }
        else
        {
            //Accelerate rabbit when not at max speed
            _enemyRB.AddForce(_enemyTF.forward * _acceleration * speedDot, ForceMode.Acceleration);
        }
        state = NodeState.SUCCESS;
        return state;
    }

    private void ChooseDirection()
    {
        //Gets the weights for all 8 directions and adds them together to get the most optimal direction
        weights = CalculateWeights();
        movingDir = Vector3.zero;
        float[] prevWeights = (float[])GetData("prevWeights");
        if (prevWeights == null)
        {
            prevWeights = zeroArray;
        }

        //Combines all the weights and moveDirections to find the optimal move direction
        for (int i = 0; i < weights.Length; i++)
        {
            movingDir += Directions.eightDirections[i] * weights[i];
        }
        movingDir = movingDir.normalized;
        SetData("prevWeights", weights);

        //Caculates the perpendicular direction to the direction to the player for sideways movement
        Vector3 movingCross = Vector3.Cross(_dirToPlayer.normalized, Vector3.up).normalized;

        //If there are no obstacles nearby, rabbit picks between moving left/right based on which direction is
        //closest to the direction it is already facing
        if (movingDir == Vector3.zero)
        {
            if (Vector3.Dot(_enemyTF.forward, movingCross) > Vector3.Dot(_enemyTF.forward, -movingCross))
            {
                movingDir = movingCross;
            }
            else
            {
                movingDir = -movingCross;
            }
        }
        SetData("movingDir", movingDir);
    }

    private float[] CalculateWeights()
    {
        //Sets up array and calcuates the distance/direction to the player
        float[] playerWeights = new float[8];
        //If player hasn't been seen yet, then the rabbit instead just leaps around its original position
        if (GetData("player") != null)
        {
            _dirToPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z);
        }
        else
        {
            _dirToPlayer = new Vector3(_originalPos.x - _enemyTF.position.x, 0, _originalPos.z - _enemyTF.position.z);
        }
        
        _distanceToPlayer = _dirToPlayer.magnitude;
        float distanceOffset = _idealDistance - _distanceToPlayer;
        _dirToPlayer = _dirToPlayer.normalized;

        //Calculates a weight for all 8 directions based on how close it is to being perpindicular to player.
        //Results in the enemy circling the player instead of chasing them
        for (int i = 0; i < playerWeights.Length; i++)
        {
            //Modifying weight based on how perpindicular it is
            float dot = Vector3.Dot(_dirToPlayer.normalized, Directions.eightDirections[i]);
            float weightToAdd = 1 - Mathf.Clamp01((Mathf.Abs(dot) + Mathf.Clamp01(Mathf.Abs(distanceOffset * 0.25f))) * 0.9f);
            weightToAdd = Mathf.Clamp(weightToAdd, 0.3f, 1);

            if (Mathf.Abs(distanceOffset) >= 2)
            {
                if (distanceOffset >= 0)
                {
                    weightToAdd += Mathf.Clamp01(dot * -1 * (distanceOffset * 0.25f)) * 0.9f;
                }
                else
                {
                    weightToAdd += Mathf.Clamp01(dot * Mathf.Abs(distanceOffset * 0.25f)) * 0.9f;
                }
            }
            //Modifying weight based on distance           
            weightToAdd = Mathf.Clamp01(weightToAdd);
            if (weightToAdd > playerWeights[i])
            {
                playerWeights[i] = weightToAdd;
            }          
        }
        //SetData("playerWeights", playerWeights);
        float[] obstacleWeights = (float[])GetData("obstacles");
        if (obstacleWeights == null)
        {
            obstacleWeights = zeroArray;
        }
        float[] finalWeights = new float[8];
        //Substracts the interest weights by the danger weights to get the final weights
        for (int i = 0; i < playerWeights.Length; i++)
        {
            finalWeights[i] = Mathf.Clamp01(playerWeights[i] - obstacleWeights[i]);
        }
        return finalWeights;
    }

    private void RotateTowardsPlayer()
    {
        float angle = Vector3.Angle(_enemyTF.forward, movingDir);
        if (angle < _rotation)
        {
            _enemyTF.forward = movingDir;
        }
        else
        {
            if (Vector3.Dot(_enemyTF.right, movingDir) > Vector3.Dot(-_enemyTF.right, movingDir))
            {
                _enemyTF.rotation = Quaternion.AngleAxis(_rotation, _enemyTF.up) * _enemyTF.rotation;
            }
            else
            {
                _enemyTF.rotation = Quaternion.AngleAxis(_rotation, -_enemyTF.up) * _enemyTF.rotation;
            }
        }
    }
}
