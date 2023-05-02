using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class SpiderBT : BT
{
    public SpiderStats spiderStats;

    protected override Node SetupTree()
    {
        if (spiderStats.playerTF == null || spiderStats.player == null)
        {
            spiderStats.SearchForPlayer();
        }
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new AttackingCooldown(spiderStats.attackingCooldown, new List<string> {"electric", "webDeploy"}),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new FOVCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.visionRange, "electric", spiderStats.environmentMask, 4, false),
                        new RangeCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.electricMinRange, spiderStats.electricMaxRange, "electric"),
                        new CooldownCheck(spiderStats.electricCooldown, "electric"),
                        new SpiderElectricBolts()
                    }),
                    new Sequence(new List<Node>
                    {
                        new PlayerSeenCheck(),
                        new RangeCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.webDeployMinRange, spiderStats.webDeployMaxRange, "webDeploy"),
                        new CooldownCheck(spiderStats.webDelployCooldown, "webDeploy"),
                        new SpiderWebDeploy()
                    })
                }),
                
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.visionRange, "move", spiderStats.environmentMask, 4, true),
                new ObstacleDetector(spiderStats.obstacleDetectRadius, spiderStats.obstacleMask, spiderStats.enemyTF, spiderStats.hitbox),
                new SpiderAggroMovement()
            })           
        });

        return root;
    }
}
