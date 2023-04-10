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
            new Sequence(new List<Node>
            {
                new Inverter(new List<Node>
                {
                    new CooldownCheck(rabbitStats.evadeDuration, "idle")
                }),
                new PlayerSeenCheck(),
                new ObstacleDetector(rabbitStats.obstacleDetectRadius, rabbitStats.obstacleMask, rabbitStats.enemyTF, rabbitStats.hitbox),
                new RabbitEvadeMode(rabbitStats.maxSpeed)
            }),
            new Sequence(new List<Node>
            {
                new RangeCheck(rabbitStats.enemyTF, rabbitStats.playerTF, rabbitStats.agroRange, "agro"),
                new RabbitAgroMode(rabbitStats.attackHitbox, rabbitStats.maxSpeed)
            })
        });
        return root;
    }
}
