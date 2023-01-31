using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ObstacleDetector : Node
{
    private float _detectionRadius;
    private LayerMask _obstacleMask;
    private Transform _enemyTF;
    private float _detectionRate = .1f; //Used to make it so checks for obstacles at a slower rate than update cause its unnecessary and prevents lag
    private float _detectionRateTimer;
    private float _hitBoxSize = 1f;


    public ObstacleDetector(float obstacleDetectRadius, LayerMask obstacleMask, Transform enemyTF)
    {
        _detectionRadius    = obstacleDetectRadius;
        _obstacleMask       = obstacleMask;
        _enemyTF            = enemyTF;
        _detectionRateTimer = -1;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - _detectionRateTimer > _detectionRate)
        {
            _detectionRateTimer = Time.time;
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
                Vector3 dirToObstacle = coll.ClosestPoint(_enemyTF.position) - _enemyTF.position;
                float distToObstacle = dirToObstacle.magnitude;
                //Assigns a weight for the obstacle based on how close it is
                if (distToObstacle <= _hitBoxSize)
                {
                    weight = 1;
                }
                else
                {
                    weight = (_detectionRadius - distToObstacle) / _detectionRadius;
                }
                dirToObstacle = dirToObstacle.normalized;
                //Checks how close each of the 8 directions is to the direction to the obstacle
                for (int i = 0; i < Directions.eightDirections.Count; i++)
                {
                    float dot = Vector3.Dot(dirToObstacle, Directions.eightDirections[i]);
                    float desirablity = dot * weight;
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
