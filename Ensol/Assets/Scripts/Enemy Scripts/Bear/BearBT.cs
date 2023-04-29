using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class BearBT : BT
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
                new BearAgroMovement(bearStats.acceleration, bearStats.maxSpeed, bearStats.angryAcceleration, bearStats.angryMaxSpeed, bearStats.playerTF, bearStats.enemyTF, 
                                     bearStats.enemyRB, bearStats.rotationSpeed, bearStats.environmentMask)
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
                new FOVCheck(bearStats.enemyTF, bearStats.playerTF, bearStats.visionRange, "junk", bearStats.environmentMask, 2),
                new CooldownCheck(bearStats.junkCooldown, "junk"),
                new RangeCheck(bearStats.enemyTF, bearStats.playerTF, bearStats.junkRange, "junk"),
                new BearJunkThrow()
            })
        });
        return root;
    }
}
