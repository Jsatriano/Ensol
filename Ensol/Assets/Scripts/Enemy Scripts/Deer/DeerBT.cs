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
                new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "charging", deerStats.environmentMask),
                new CooldownCheck(deerStats.chargeCooldown, "charging"),
                new Charge(deerStats.chargeSpeed, deerStats.chargeWindupLength, deerStats.playerTF,
                           deerStats.enemyTF, deerStats.enemyRB, deerStats.chargeHitZone, deerStats.chargeTurning, deerStats.obstacleMask,
                           deerStats.enemyMaterial, deerStats.windupMaterial, deerStats.attackMaterial, deerStats.defaultMaterial)
            }),
            new Sequence(new List<Node>
            {
                new RangeCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.attackRange, "basic"),
                new CooldownCheck(deerStats.attackCooldown, "basic"),
                new DeerBasicAttack(deerStats.hitZone, deerStats.basicAttackDuration, deerStats.basicAttackWindup, 
                                    deerStats.playerTF, deerStats.enemyTF, deerStats.tempAttackIndicator,
                                    deerStats.enemyMaterial, deerStats.windupMaterial, deerStats.attackMaterial, deerStats.defaultMaterial)
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(deerStats.enemyTF, deerStats.playerTF, deerStats.visionRange, "charging", deerStats.environmentMask),
                new ObstacleDetector(deerStats.obstacleDetectRadius, deerStats.obstacleMask, deerStats.enemyTF),
                new DeerAgroMovement(deerStats.speed, deerStats.playerTF, deerStats.enemyTF, deerStats.enemyRB, deerStats.chargeCooldown, 
                                     deerStats.distanceFromPlayer, deerStats.rotationSpeed)
            })            
        });

        return root;
    }
}
