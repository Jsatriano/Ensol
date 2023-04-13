using System.Collections;
using System.Collections.Generic;
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
            /*
            new Sequence(new List<Node>
            {
                new PlayerSeenCheck(),
                new Inverter(new List<Node>
                {
                    new CooldownCheck(rabbitStats.attackingCooldown, "idle")
                }),       
                new ObstacleDetector(rabbitStats.obstacleDetectRadius, rabbitStats.obstacleMask, rabbitStats.enemyTF, rabbitStats.hitbox),
                new RabbitEvadeMode(rabbitStats.maxSpeed)
            }),
            */
            new Sequence(new List<Node>
            {
                new CooldownCheck(rabbitStats.evadeDuration, "agro"),
                new FOVCheck(rabbitStats.enemyTF, rabbitStats.playerTF, rabbitStats.visionRange, "agro", rabbitStats.environmentMask, 3),
                new ObstacleDetector(rabbitStats.obstacleDetectRadius, rabbitStats.obstacleMask, rabbitStats.enemyTF, rabbitStats.hitbox),
                new RabbitAgroMode(rabbitStats.attackHitbox, rabbitStats.acceleration, rabbitStats.maxSpeed, rabbitStats.playerTF, 
                                   rabbitStats.enemyTF, rabbitStats.enemyRB, rabbitStats.rotationSpeed, rabbitStats.environmentMask, rabbitStats.agroDuration)
            })
        });
        return root;
    }
}
