using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

//Deer Behavior Tree - RYAN

public class DeerBT : Tree
{
    public DeerStats deerStats;
    
    protected override Node SetupTree()
    {
        if(deerStats.playerTF == null || deerStats.player == null) {
            deerStats.SearchForPlayer();
        }
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Inverter(new List<Node>
                {
                    new CooldownCheck(deerStats.attackingCooldown, "idle")
                }),
                new PlayerSeenCheck(),
                new ObstacleDetector(deerStats.obstacleDetectRadius, deerStats.obstacleMask, deerStats.enemyTF),
                new DeerAgroMovement(deerStats.acceleration, deerStats.maxSpeed, deerStats.playerTF, deerStats.enemyTF, deerStats.enemyRB, deerStats.chargeCooldown,
                                     deerStats.distanceFromPlayer, deerStats.rotationSpeed)
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "charging", deerStats.environmentMask),
                new CooldownCheck(deerStats.chargeCooldown, "charging"),
                new Charge(deerStats.chargeMaxSpeed, deerStats.chargeAccel, deerStats.chargeWindupLength, deerStats.playerTF,
                           deerStats.enemyTF, deerStats.enemyRB, deerStats.chargeHitbox, deerStats.chargeTurning, deerStats.obstacleMask)
            }),
            new Sequence(new List<Node>
            {
                new RangeCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.attackRange, "basic"),
                new CooldownCheck(deerStats.attackCooldown, "basic"),
                new DeerBasicAttack(deerStats.hitZone, deerStats.basicAttackDuration, deerStats.basicAttackWindup, 
                                    deerStats.playerTF, deerStats.enemyTF)
            }),           
        });

        return root;
    }
}
