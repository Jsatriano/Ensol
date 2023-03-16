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
    private float _hitboxSize;          //Size of the enemies hitbox, would be nice to automate this value in the future
    private float[] avoidanceWeights  = new float[8]; //Context map for the obstacles
    private float[] prevWeights       = new float[8];


    public ObstacleDetector(float obstacleDetectRadius, LayerMask obstacleMask, Transform enemyTF, BoxCollider hitbox)
    {
        _detectionRadius    = obstacleDetectRadius;
        _obstacleMask       = obstacleMask;
        _enemyTF            = enemyTF;
        _hitboxSize         = hitbox.size.z;
        _detectionRateTimer = -1;
        for (int i = 0; i < 8; i++)
        {
            prevWeights[i] = 0;
        }
        //SetData("obstacles", avoidanceWeights);
    }

    //Script for the obstacle avoidance part of context steering. Detects all the obstacles around a player and assigns
    //a weight to all 8 directions based on how close an obstacle is to each direction - RYAN

    public override NodeState Evaluate()
    {   
        //Only runs at the rate given by detection rate (running every update would be unecessary and intensive)
        if (Time.time - _detectionRateTimer > _detectionRate)
        {
            _detectionRateTimer = Time.time;                    
            for (int i = 0; i < 8; i++)
            {
                avoidanceWeights[i] = 0;
            }

            float weight;
            //Checks for every obstacle within a certain radius of the enemy
            Collider[] obstacles = Physics.OverlapSphere(_enemyTF.position, _detectionRadius, _obstacleMask);
            foreach (Collider coll in obstacles)
            {
                //Checks if the collider is the enemy this script is atached to
                if (coll.transform == _enemyTF.transform || coll.tag == "Sound")
                {
                    continue;
                }
                
                //Assigns a weight for the obstacle based on how close it is
                Vector3 dirToObstacle = (coll.ClosestPoint(_enemyTF.position) - _enemyTF.position).normalized;
                float distToObstacle  = Vector3.Distance(_enemyTF.position, coll.ClosestPoint(_enemyTF.position));            
                if (distToObstacle <= _hitboxSize)
                {
                    weight = 1;
                }
                else
                {
                    weight = (_detectionRadius - distToObstacle) / _detectionRadius;
                }
                weight = Mathf.Clamp(weight, 0, 1);

                //Checks how close each of the 8 directions is to the direction to the obstacle           
                for (int i = 0; i < Directions.eightDirections.Count; i++)
                {
                    float dot = Vector3.Dot(dirToObstacle, Directions.eightDirections[i]);
                    dot = Mathf.Clamp(dot, 0, 1);
                    float desirablity = Mathf.Clamp01(dot * weight);
                    //desirablity = (desirablity + prevWeights[i]) / 2;
                    //Assigns the danger of that direction to the result array (if its higher than the current danger for that direction)
                    if (desirablity > avoidanceWeights[i])
                    {
                        avoidanceWeights[i] = desirablity;
                    }
                }
            }

            for (int i = 0; i < avoidanceWeights.Length; i++)
            {
                avoidanceWeights[i] = (avoidanceWeights[i] + (prevWeights[i])) / 2;
            }
            avoidanceWeights.CopyTo(prevWeights, 0);
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
