using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

//Deer Behavior Tree - RYAN

public class DeerBT : Tree
{
    public DeerStats deerStats;
    
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "charging"),
                new CooldownCheck(deerStats.cooldownLength, "charging"),
                new Charge(deerStats.chargeSpeed, deerStats.windupLength, deerStats.playerTF,
                           deerStats.enemyTF, deerStats.enemyRB, deerStats.hitZone, deerStats.chargeTurning),
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "charging"),
                new ObstacleDetector(deerStats.obstacleDetectRadius, deerStats.obstacleMask, deerStats.enemyTF),
                new TrackPlayer(deerStats.playerTF, deerStats.enemyTF, deerStats.rotationSpeed),
                new DeerAgroMovement(deerStats.speed, deerStats.playerTF, deerStats.enemyTF, deerStats.enemyRB, deerStats.cooldownLength, deerStats.distanceFromPlayer)
            })            
        });

        return root;
    }
}
