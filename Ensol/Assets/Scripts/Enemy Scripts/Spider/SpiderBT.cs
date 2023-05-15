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
                new AttackingCooldown(spiderStats.attackingCooldown, new List<string> {"tazer", "webDeploy", "webShot"}),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new FOVCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.visionRange, "tazer", spiderStats.environmentMask, 4, false),
                        new RangeCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.tazerMinRange, spiderStats.tazerMaxRange, "tazer"),
                        new CooldownCheck(spiderStats.tazerCooldown, "tazer"),
                        new SpiderTazerShot("tazer", spiderStats.tazerManager, spiderStats.playerTF, spiderStats.enemyTF, spiderStats.tazerRotation)
                    }),
                    new Sequence(new List<Node>
                    {
                        new FOVCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.visionRange, "webShot", spiderStats.environmentMask, 4, false),
                        new RangeCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.webShotMinRange, spiderStats.webShotMaxRange, "webShot"),
                        new CooldownCheck(spiderStats.webShotCooldown, "webShot"),
                        new SpiderWebShot("webShot", spiderStats.webShotManager, spiderStats.playerTF, spiderStats.enemyTF, spiderStats.webShotRotation)
                    }),
                    new Sequence(new List<Node>
                    {
                        new PlayerSeenCheck(),
                        new RangeCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.webDeployMinRange, spiderStats.webDeployMaxRange, "webDeploy"),
                        new CooldownCheck(spiderStats.webDelployCooldown, "webDeploy"),
                        new SpiderWebDeploy("webDeploy", spiderStats.webManager)
                    })
                }),          
            }),
            new Sequence(new List<Node>
            {
                new FOVCheck(spiderStats.enemyTF, spiderStats.playerTF, spiderStats.visionRange, "move", spiderStats.environmentMask, 4, true),
                new ObstacleDetector(spiderStats.obstacleDetectRadius, spiderStats.obstacleMask, spiderStats.enemyTF, spiderStats.hitbox),
                new SpiderAggroMovement(spiderStats.acceleration, spiderStats.maxSpeed, spiderStats.minSpeed, spiderStats.rapidAvoidDist, spiderStats.playerTF, spiderStats.enemyTF,
                                        spiderStats.enemyRB, spiderStats.idealDist, spiderStats.rotationSpeed, spiderStats.sidewaysMult, "movingForward")
            })           
        });

        return root;
    }
}
