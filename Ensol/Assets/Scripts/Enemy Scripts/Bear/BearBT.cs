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
                new Inverter(new List<Node>
                {
                    new CooldownCheck(bearStats.attackingCooldown, "idle")
                }),
                new PlayerSeenCheck(),
                new ObstacleDetector(bearStats.obstacleDetectRadius, bearStats.obstacleMask, bearStats.enemyTF, bearStats.hitbox),
                new BearAgroMovement(bearStats.acceleration, bearStats.maxSpeed, bearStats.playerTF, bearStats.enemyTF, 
                                     bearStats.enemyRB, bearStats.rotationSpeed)
            }),
            new Sequence(new List<Node>
            {
                new CooldownCheck(bearStats.swipeCooldown, "swipe"),
                new RangeCheck(bearStats.enemyTF, bearStats.playerTF, bearStats.swipeRange, "swipe"),
                new BearSwipe(bearStats.swipeHitbox1, bearStats.swipeHitbox2, bearStats.playerTF, bearStats.enemyTF, 
                              bearStats.enemyRB, bearStats.swipeMovement, bearStats.swipeRotation)
            }),
            new Sequence(new List<Node>
            {
                new CooldownCheck(bearStats.junkCooldown, "junk"),
                new FOVCheck(bearStats.enemyTF, bearStats.playerTF, bearStats.visionRange, "junk", bearStats.environmentMask),
                new RangeCheck(bearStats.enemyTF, bearStats.playerTF, bearStats.junkRange, "junk"),
                new BearJunkThrow(bearStats.playerTF, bearStats.enemyTF, bearStats.junkRotation)
            })
        });
        return root;
    }
}
