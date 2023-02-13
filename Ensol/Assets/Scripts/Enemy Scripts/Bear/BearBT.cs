using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class BearBT : Tree
{
    public BearStats bearStats;
    protected override Node SetupTree()
    {
        if (bearStats.playerTF == null || bearStats.player == null)
        {
            bearStats.SearchForPlayer();
        }
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new PlayerSeenCheck(),
                new ObstacleDetector(bearStats.obstacleDetectRadius, bearStats.obstacleMask, bearStats.enemyTF, bearStats.hitbox),
                new BearAgroMovement(bearStats.acceleration, bearStats.maxSpeed, bearStats.playerTF, bearStats.enemyTF, 
                                     bearStats.enemyRB, bearStats.rotationSpeed)
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(bearStats.enemyTF, bearStats.playerTF, bearStats.visionRange, "Claw", bearStats.environmentMask)
            })
        });
        return root;
    }
}
