using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerAgroMovement : Node
{
    private Transform _playerTF;  //Player transform
    private Transform _enemyTF;   //Enemy transform
    private Rigidbody _enemyRB;   //Enemy rigidbody
    private float _acceleration;  //How fast the deer gets to max speed
    private float _maxSpeed;      //Speed of the enemy
    private float _idealDistance; //The ideal distance the deer tries to stay from the player
    private Vector3 _dirToPlayer; //The direction from the enemy to the player
    private Vector3 movingDir;    //The deers direction of movement
    private float _rotationSpeed; //How quickly the enemy turns (how well they can track the player)
    private float[] weights   = new float[8];    //The context map for the deer
    private float[] zeroArray = new float[8];    //Used for resetting arrays to all zeroes

    public DeerAgroMovement(float acceleration, float maxSpeed, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float idealDistance, float rotationSpeed)
    {
        _playerTF  = playerTF;
        _enemyTF   = enemyTF;
        _enemyRB   = enemyRB;
        _maxSpeed  = maxSpeed / 10;
        _acceleration  = acceleration;    
        _idealDistance = idealDistance;
        _rotationSpeed = rotationSpeed;
        movingDir   = Vector3.zero;

        for (int i = 0; i < 8; i++)
        {
            zeroArray[i] = 0;
        }
        weights = zeroArray;
}

    //Idle movement inbetween attacks when deer is in combat,using context steering to avoid obstacles and other enemies - RYAN
    public override NodeState Evaluate()
    {
        ChooseDirection();

        //Makes the deer try to face diagonally between the dir it is moving and the dir to the player
        //Plays the normal walking anim if they are walking towards/away from the player as opposed to perpendicular
        Vector3 lookingDirection = (_dirToPlayer.normalized + movingDir.normalized).normalized;
        //Multiplier makes it so that the rotation starts slow and then speeds up
        float rotationMultiplier = Mathf.Clamp(Vector3.Dot(_enemyTF.forward, lookingDirection), 0.5f, 1);
        _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, lookingDirection, (_rotationSpeed * rotationMultiplier) / 100);
        float lookingDot = Vector3.Dot(_dirToPlayer.normalized, lookingDirection);
        if (lookingDot > .8f || lookingDot < -.8f)
        {
            SetData("lookingForward", true);
        }
        //Moves deer in the desired direction (with a speed cap)
        _enemyRB.AddForce(movingDir.normalized * _acceleration, ForceMode.Acceleration);
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
            movingDir += Directions.eightDirections[i]  * weights[i];
        }
        movingDir = movingDir.normalized;
        SetData("prevWeights", weights);

        //Caculates the perpendicular direction to the direction to the player for sideways movement
        Vector3 movingCross = Vector3.Cross(_dirToPlayer.normalized, Vector3.up).normalized;
        SetData("deerRight", movingCross);

        //If there are no obstacles nearby, deer picks between moving left/right based on which direction is
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
        float[] playerWeights     = new float[8];
        _dirToPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z);
        float distanceToPlayer    = _dirToPlayer.magnitude;
        _dirToPlayer = _dirToPlayer.normalized;

        //Calculates a weight for all 8 directions based on how close it is to being perpindicular to player.
        //Results in the enemy circling the player instead of chasing them
        for (int i = 0; i < playerWeights.Length; i++)
        {
            float dot = Vector3.Dot(_dirToPlayer.normalized, Directions.eightDirections[i]);
            float weightToAdd = 1 - (Mathf.Abs(dot) * 0.9f);
            weightToAdd = Mathf.Clamp(weightToAdd, 0.3f, 1);
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
        float[] finalWeights    = new float[8];
        //Substracts the interest weights by the danger weights to get the final weights
        for (int i = 0; i < playerWeights.Length; i++)
        {
            finalWeights[i] = Mathf.Clamp01(playerWeights[i] - obstacleWeights[i]);
        }
        return finalWeights;
    }
}
