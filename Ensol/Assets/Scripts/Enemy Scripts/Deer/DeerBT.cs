using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class DeerBT : Tree
{
    public DeerStats deerStats;
    //This is where the deer enemy behavior tree will be set up
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new FOVCheck(deerStats.deerTF, deerStats.visionRange),
            new Charge(deerStats.chargeSpeed, deerStats.chargeHitbox, deerStats.windupLength, deerStats.cooldownLength, deerStats.playerTF, deerStats.deerTF, deerStats.deerRB),
        });

        return root;
    }
}
