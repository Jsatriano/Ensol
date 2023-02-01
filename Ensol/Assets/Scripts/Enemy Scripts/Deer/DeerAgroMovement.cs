using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerAgroMovement : Node
{
    private float _moveSpeed;
    private Transform _playerTF;
    private Transform _enemyTF;
    private Rigidbody _enemyRB;
    private bool movingCauseZero;
    private int randomDir;
    private float _randomMovementTimer;
    private float _randomMovementLength;
    float _idealDistance;

    public DeerAgroMovement(float moveSpeed, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float cooldown, float idealDistance)
    {
        _playerTF  = playerTF;
        _enemyTF   = enemyTF;
        _enemyRB   = enemyRB;
        _moveSpeed = moveSpeed;
        randomDir  = 0;
        _randomMovementTimer  = 0;
        _randomMovementLength = cooldown;
        _idealDistance   = idealDistance;
}

    public override NodeState Evaluate()
    {
        float[] weights = CalculateWeights();
        Vector3 direction = Vector3.zero;
        for (int i = 0; i < weights.Length; i++)
        {
            direction += Directions.eightDirections[i] * weights[i];
        }
        direction = direction.normalized;
        //Fixes cases where there are no obstacles nearby by random picking whether to move left or right
        if (direction == Vector3.zero)
        {
            if (!movingCauseZero || Time.time - _randomMovementTimer > _randomMovementLength)
            {
                movingCauseZero = true;
                randomDir = Random.Range(0, 2);
                _randomMovementTimer = Time.time;
            }
            else
            {
                if (randomDir == 0)
                {
                    direction = _enemyTF.right;
                }
                else
                {
                    direction = _enemyTF.right * -1;
                }
            }
        } 
        else
        {
            movingCauseZero = false;
        }
        _enemyRB.AddForce(direction * _moveSpeed * Time.deltaTime * 100, ForceMode.Acceleration);
        SetData("dir", direction);

        state = NodeState.SUCCESS;
        return state;

    }

    private float[] CalculateWeights()
    {
        float[] playerWeights = new float[8];
        Vector3 directionToPlayer = (_playerTF.position - _enemyTF.position);
        float distanceToPlayer = directionToPlayer.magnitude;
        //Calculates a weight for all 8 directions based on how close it is to being perpindicular to player.
        //Results in the enemy circling the player instead of chasing them
        for (int i = 0; i < playerWeights.Length; i++)
        {
            float dot = Vector3.Dot(directionToPlayer.normalized, Directions.eightDirections[i]);
            float weightToAdd = 1 - Mathf.Abs(dot);
            //This blocked out code is an attempt at making the deer stay a certain distance from the player, giving up for now
            /*
            Vector3 newPotentialPos = Directions.eightDirections[i] + _enemyTF.position;
            float distFromIdealDist = _idealDistance - (newPotentialPos - _playerTF.position).magnitude;
            float distModifier = 1;
            if (distFromIdealDist > 0)
            {
                distModifier = (_idealDistance - distFromIdealDist ) / _idealDistance;
                distModifier = Mathf.Clamp(distFromIdealDist, 0.4f, 0.6f);
            } else if (distFromIdealDist < 0)
            {

            }
            */
            if (weightToAdd > playerWeights[i])
            {
                playerWeights[i] = weightToAdd;
            }
        }
        SetData("playerWeights", playerWeights);
        float[] obstacleWeights = (float[])GetData("obstacles");
        float[] finalWeights = new float[8];
        //Substracts the interest weights by the danger weights to get the final weights
        for (int i = 0; i < playerWeights.Length; i++)
        {
            finalWeights[i] = Mathf.Clamp01(playerWeights[i] - obstacleWeights[i]);
        }
        SetData("final", finalWeights);
        return finalWeights;
    }
}
