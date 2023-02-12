using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ObstacleDetector : Node
{
    private LayerMask _obstacleMask;    //Layer(s) the obstacles to detect are on (usually the environment and other enemies)
    private Transform _enemyTF;         //Enemy transform
    private float _detectionRadius;     //Distance the enemy detects obstacles from
    private float _detectionRate = .1f; //Used to make it so checks for obstacles at a slower rate than update cause its unnecessary and prevents lag
    private float _detectionRateTimer;  //Used internally to keep track of the above
    private float _hitBoxSize = 1f;     //Size of the enemies hitbox, would be nice to automate this value in the future


    public ObstacleDetector(float obstacleDetectRadius, LayerMask obstacleMask, Transform enemyTF)
    {
        _detectionRadius    = obstacleDetectRadius;
        _obstacleMask       = obstacleMask;
        _enemyTF            = enemyTF;
        _detectionRateTimer = -1;
    }

    //Script for the obstacle avoidance part of context steering. Detects all the obstacles around a player and assigns
    //a weight to all 8 directions based on how close an obstacle is to each direction - RYAN

    public override NodeState Evaluate()
    {   
        //Only runs at the rate given by detection rate (running every update would be unecessary and intensive)
        if (Time.time - _detectionRateTimer > _detectionRate)
        {
            _detectionRateTimer = Time.time;
            //Sets up array and fills it with zeroes (zero part might be unnecessary)
            float[] avoidanceWeights = new float[8];
            for (int i = 0; i < avoidanceWeights.Length; i++)
            {
                avoidanceWeights[i] = 0;
            }
            float weight;
            //Checks for every obstacle within a certain radius of the enemy
            Collider[] obstacles = Physics.OverlapSphere(_enemyTF.position, _detectionRadius, _obstacleMask);
            foreach (Collider coll in obstacles)
            {
                //Assigns a weight for the obstacle based on how close it is
                Vector3 dirToObstacle = coll.ClosestPoint(_enemyTF.position) - _enemyTF.position;
                float distToObstacle = Vector3.Distance(_enemyTF.position, coll.ClosestPoint(_enemyTF.position));            
                if (distToObstacle <= _hitBoxSize)
                {
                    weight = 1;
                }
                else
                {
                    weight = (_detectionRadius - distToObstacle) / _detectionRadius;
                }

                //Checks how close each of the 8 directions is to the direction to the obstacle
                dirToObstacle = dirToObstacle.normalized;             
                for (int i = 0; i < Directions.eightDirections.Count; i++)
                {
                    float dot = Vector3.Dot(dirToObstacle, Directions.eightDirections[i]);
                    float desirablity = Mathf.Clamp01((dot * 1.25f) * weight);
                    //Assigns the danger of that direction to the result array (if its higher than the current danger for that direction)
                    if (desirablity > avoidanceWeights[i])
                    {
                        avoidanceWeights[i] = desirablity;
                    }
                }

            }
            SetData("obstacles", avoidanceWeights);
        }
        state = NodeState.SUCCESS;
        return state;
    }
}

//The 8 directions used for context based steering
public static class Directions
{
    public static List<Vector3> eightDirections = new List<Vector3> {
        new Vector3(0, 0, 1).normalized,
        new Vector3(1, 0, 1).normalized,
        new Vector3(1, 0, 0).normalized,
        new Vector3(1, 0, -1).normalized,
        new Vector3(0, 0, -1).normalized,
        new Vector3(-1, 0, -1).normalized,
        new Vector3(-1, 0, 0).normalized,
        new Vector3(-1, 0, 1).normalized
    };
}
