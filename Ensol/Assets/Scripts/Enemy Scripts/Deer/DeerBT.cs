using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

//Deer Behavior Tree - RYAN

public class DeerBT : Tree
{
    public DeerStats deerStats;
    
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new FOVCheck(deerStats.deerTF, deerStats.playerTF, deerStats.visionRange, "charging"),
                new CooldownCheck(deerStats.cooldownLength, "charging"),
                new Charge(deerStats.chargeSpeed, deerStats.windupLength, deerStats.playerTF,
                           deerStats.deerTF, deerStats.deerRB, deerStats.hitZone, deerStats.chargeTurning),
            }),
            new TrackPlayer(deerStats.playerTF, deerStats.deerTF, deerStats.rotationSpeed)         
        });

        return root;
    }
}
