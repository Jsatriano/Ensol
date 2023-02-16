using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearAgroMovement : Node
{
    private Transform _playerTF;  //Player transform
    private Transform _enemyTF;   //Enemy transform
    private Rigidbody _enemyRB;   //Enemy rigidbody
    private float _acceleration;  //How fast the bear gets to max speed
    private float _maxSpeed;      //Speed of the enemy
    private Vector3 _dirToPlayer; //The direction from the enemy to the player
    private Vector3 movingDir;   //The bears direction of movement
    private float _rotationSpeed; //How quickly the enemy turns (how well they can track the player)

    public BearAgroMovement(float acceleration, float maxSpeed, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float rotationSpeed)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _enemyRB = enemyRB;
        _acceleration = acceleration;
        _maxSpeed = maxSpeed / 10;
        _rotationSpeed = rotationSpeed;
        movingDir = Vector3.zero;
    }

    public override NodeState Evaluate()
    {
        ChooseDirection();

        //Finds how closely the bear's transform.forward is to the direction it wants to move and then limits that to a number between 0 and 1
        float speedDot = Vector3.Dot(_enemyTF.forward, movingDir);
        speedDot = (speedDot / 2) + 0.5f;
        speedDot = Mathf.Clamp(speedDot, 0.3f, 1);

        _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, movingDir, _rotationSpeed / 100);

        //Moves bear in the desired direction (with a speed cap)
        _enemyRB.AddForce(_enemyTF.forward * _acceleration * speedDot, ForceMode.Acceleration);
        if (_enemyRB.velocity.magnitude > _maxSpeed)
        {
            _enemyRB.velocity = Vector3.ClampMagnitude(_enemyRB.velocity, _maxSpeed);
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
        //Sets up array and calcuates the distance/direction to the player
        float[] playerWeights = new float[8];
        _dirToPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z);
        float distanceToPlayer = _dirToPlayer.magnitude;
        _dirToPlayer = _dirToPlayer.normalized;

        //Calculates a weight for all 8 directions based on how close it is to the direction the the player
        for (int i = 0; i < playerWeights.Length; i++)
        {
            float dot = Vector3.Dot(_dirToPlayer.normalized, Directions.eightDirections[i]);
            dot += 0.4f;
            float weightToAdd = Mathf.Clamp01(dot);
            if (weightToAdd > playerWeights[i])
            {
                playerWeights[i] = weightToAdd;
            }
        }
        SetData("playerWeights", playerWeights);
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
}
