using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearAggroMovement : Node
{
    private Transform _playerTF;  //Player transform
    private Transform _enemyTF;   //Enemy transform
    private Rigidbody _enemyRB;   //Enemy rigidbody
    private float _acceleration;  //How fast the bear gets to max speed
    private float _maxSpeed;      //Speed of the enemy
    private float _angryAcceleration; //2nd phase acceleration
    private float _angryMaxSpeed;     //2nd phase max speed
    private Vector3 _dirToPlayer; //The direction from the enemy to the player
    private Vector3 movingDir;    //The bears direction of movement
    private float _rotationSpeed; //How quickly the enemy turns (how well they can track the player)
    private LayerMask _envLayerMask; //Used for linecasting to player breadcrumbs

    public BearAggroMovement(float acceleration, float maxSpeed, float angryAcceleration, float angryMaxSpeed, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float rotationSpeed, LayerMask envLayerMask)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _enemyRB = enemyRB;
        _acceleration = acceleration;
        _maxSpeed = maxSpeed / 10;
        _rotationSpeed = rotationSpeed;
        movingDir = Vector3.zero;
        _envLayerMask = envLayerMask;
        _angryMaxSpeed = angryMaxSpeed / 10;
        _angryAcceleration = angryAcceleration;
    }

    public override NodeState Evaluate()
    {
        ChooseDirection();

        //Initiates 2nd phase speed
        if (_maxSpeed != _angryMaxSpeed && GetData("belowHalf") != null)
        {
            _maxSpeed = _angryMaxSpeed;
            _acceleration = _angryAcceleration;
        }

        //Finds how closely the bear's transform.forward is to the direction it wants to move and then limits that to a number between 0 and 1
        float speedDot = Vector3.Dot(_enemyTF.forward, movingDir);
        speedDot = (speedDot / 2) + 0.5f;
        speedDot = Mathf.Clamp(speedDot, 0.3f, 1);

        _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, movingDir, _rotationSpeed / 100);

        //Moves bear in the desired direction (with a speed cap)
        if (_enemyRB.velocity.magnitude > _maxSpeed)
        {
            //Keep moving bear at max speed
            _enemyRB.velocity = Vector3.ClampMagnitude(_enemyRB.velocity, _maxSpeed);
        }
        else
        {
            //Accelerate bear when not at max speed
            _enemyRB.AddForce(_enemyTF.forward * _acceleration * speedDot, ForceMode.Acceleration);
        }
        state = NodeState.SUCCESS;
        return state;
    }

    private void ChooseDirection()
    {
        //Gets the weights for all 8 directions and adds them together to get the most optimal direction
        float[] weights = CalculateWeights();
        movingDir = Vector3.zero;
        for (int i = 0; i < weights.Length; i++)
        {
            movingDir += Directions.eightDirections[i] * weights[i];
        }
        movingDir = movingDir.normalized;
        SetData("moveDir", movingDir);
    }

    private float[] CalculateWeights()
    {
        float[] playerWeights = CalculatePlayerWeights();
        float[] obstacleWeights = (float[])GetData("obstacles");
        float[] finalWeights    = new float[8];
        //Substracts the interest weights by the danger weights to get the final weights
        for (int i = 0; i < playerWeights.Length; i++)
        {
            finalWeights[i] = Mathf.Clamp01(playerWeights[i] - obstacleWeights[i]);
        }
        SetData("final", finalWeights);
        SetData("playerWeights", playerWeights);
        return finalWeights;
    }

    private float[] CalculatePlayerWeights()
    {
        //Sets up array and calcuates the distance and direction to the player
        float[] playerWeights = new float[8];
        Vector3 target = _playerTF.position;

        List<Vector3> breadcrumbs = (List<Vector3>)GetData("breadcrumbs");
        //Uses player's current position as the target if they are currently in FOV
        if (breadcrumbs == null || !Physics.Linecast(_enemyTF.position, _playerTF.position, _envLayerMask))
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
                if (!Physics.Linecast(_enemyTF.position, breadcrumbs[i], _envLayerMask))
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

        _dirToPlayer = new Vector3(target.x - _enemyTF.position.x, 0, target.z - _enemyTF.position.z);

        float distanceToPlayer = _dirToPlayer.magnitude;
        _dirToPlayer = _dirToPlayer.normalized;

        //Calculates a weight for all 8 directions based on how close it is to the direction the the player
        for (int i = 0; i < playerWeights.Length; i++)
        {
            float dot = Vector3.Dot(_dirToPlayer.normalized, Directions.eightDirections[i]);
            //Favors directions the bear is already facing
            float dot2 = Vector3.Dot(_enemyTF.forward, Directions.eightDirections[i]);
            dot += Mathf.Clamp(dot2, 0, 0.2f);
            float weightToAdd = Mathf.Clamp01(dot);
            if (weightToAdd > playerWeights[i])
            {
                playerWeights[i] = weightToAdd;
            }
        }
        SetData("playerWeights", playerWeights);

        return playerWeights;
    }
}