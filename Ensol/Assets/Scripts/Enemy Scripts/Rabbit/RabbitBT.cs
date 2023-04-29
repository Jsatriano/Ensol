using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RabbitBT : BT
{
    public RabbitStats rabbitStats;

    protected override Node SetupTree()
    {
        if (rabbitStats.playerTF == null || rabbitStats.player == null)
        {
            rabbitStats.SearchForPlayer();
        }
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new FOVCheck(rabbitStats.enemyTF, rabbitStats.playerTF, rabbitStats.visionRange, "aggro", rabbitStats.environmentMask, 3),
                new CooldownCheck(rabbitStats.attackingCooldown, "aggro"),
                new ObstacleDetector(rabbitStats.obstacleDetectRadius, rabbitStats.obstacleMask, rabbitStats.enemyTF, rabbitStats.hitbox),
                new RabbitAggroMode(rabbitStats.acceleration, rabbitStats.aggroSpeed, rabbitStats.playerTF,
                                   rabbitStats.enemyTF, rabbitStats.enemyRB, rabbitStats.rotationSpeed, 
                                   rabbitStats.environmentMask, rabbitStats.landingDrag, rabbitStats.normalDrag, rabbitStats.aggroLeaps, rabbitStats.grinderVFX)
            }),
            new Sequence(new List<Node>
            {    
                new ObstacleDetector(rabbitStats.obstacleDetectRadius, rabbitStats.obstacleMask, rabbitStats.enemyTF, rabbitStats.hitbox),
                new RabbitEvadeMode(rabbitStats.acceleration, rabbitStats.maxSpeed, rabbitStats.minSpeed, rabbitStats.rapidAvoidDist, rabbitStats.playerTF, 
                                    rabbitStats.enemyTF, rabbitStats.enemyRB, rabbitStats.evadeDistance, rabbitStats.rotationSpeed, rabbitStats.landingDrag, rabbitStats.normalDrag)
            }),          
            
        });
        return root;
    }

    /*
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && player != null && root.GetData("prevWeights") != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, rabbitStats.evadeDistance);
            float[] pweights = (float[])root.GetData("prevWeights");
            Gizmos.color = Color.red;
            for (int i = 0; i < pweights.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * pweights[i] * 3);
            }
        }
    }
    */
}
