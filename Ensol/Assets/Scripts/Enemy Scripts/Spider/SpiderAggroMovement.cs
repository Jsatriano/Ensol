using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpiderAggroMovement : Node
{
    private Transform _playerTF;  //Player transform
    private Transform _enemyTF;   //Enemy transform
    private Rigidbody _enemyRB;   //Enemy rigidbody
    private float _acceleration;  //How fast the enemy gets to max speed
    private float _maxSpeed;      //Max speed of the enemy
    private float _minSpeed;      //Min speed the enemy will go, the enemy will fluctuate between this min and max speed based on how close the player is
    private float _rapidAvoidDist;
    private float _idealDistance; //The ideal distance the enemy tries to stay from the player
    private Vector3 _dirToPlayer; //The direction from the enemy to the player
    private Vector3 movingDir;    //The enemy's direction of movement
    private float _rotationSpeed; //How quickly the enemy turns (how well they can track the player)
    private float[] weights = new float[8];    //The context map for the deer
    private float[] zeroArray = new float[8];  //Used for resetting arrays to all zeroes
    private float _distanceToPlayer;

    public SpiderAggroMovement(float acceleration, float maxSpeed, float minSpeed, float rapidAvoidDist, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float idealDistance,
                               float rotationSpeed)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _enemyRB = enemyRB;
        _maxSpeed = maxSpeed / 10;
        _acceleration = acceleration;
        _idealDistance = idealDistance;
        _rotationSpeed = rotationSpeed;
        movingDir = Vector3.zero;
        _minSpeed = minSpeed / 10;
        _rapidAvoidDist = rapidAvoidDist;
    }

    public override NodeState Evaluate()
    {
        return base.Evaluate();
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

        //If there are no obstacles nearby, spider picks between moving left/right based on which direction is
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
        //If player hasn't been seen yet, then the spider instead just leaps around its original position

        _dirToPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z);

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
}
