using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

//Deer Behavior Tree - RYAN

public class DeerBT : BT
{
    public DeerStats deerStats;
    
    protected override Node SetupTree()
    {
        if(deerStats.playerTF == null || deerStats.player == null) 
        {
            deerStats.SearchForPlayer();
        }
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new AttackingCooldown(deerStats.attackingCooldown, new List<string> {"charging", "basic"}),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new RangeCheck(deerStats.enemyTF, deerStats.playerTF, 0, deerStats.chargeRange, "charging"),
                        new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "charging", deerStats.environmentMask, 1, false),
                        new CooldownCheck(deerStats.chargeCooldown, "charging"),
                        new DeerCharge(deerStats.chargeMaxSpeed, deerStats.chargeAccel, deerStats.playerTF,
                                       deerStats.enemyTF, deerStats.enemyRB, deerStats.chargeHitbox, deerStats.chargeTurning, deerStats.windupRotation, deerStats.obstacleMask, "charging")
                    }),
                    new Sequence(new List<Node>
                    {
                        new RangeCheck(deerStats.enemyTF, deerStats.playerTF, 0, deerStats.attackRange, "basic"),
                        new CooldownCheck(deerStats.attackCooldown, "basic"),
                        new DeerBasicAttack(deerStats.basicAttackHitbox, deerStats.playerTF, deerStats.enemyTF, deerStats.windupTurning, "basic")
                    }),
                })
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "move", deerStats.environmentMask, 1, true),
                new ObstacleDetector(deerStats.obstacleDetectRadius, deerStats.obstacleMask, deerStats.enemyTF, deerStats.hitbox),
                new DeerAggroMovement(deerStats.acceleration, deerStats.maxSpeed, deerStats.playerTF, deerStats.enemyTF, deerStats.enemyRB,
                                      deerStats.rotationSpeed)
            })
        });

        return root;
    }
}
