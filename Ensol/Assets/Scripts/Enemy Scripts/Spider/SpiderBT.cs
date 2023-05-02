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
            
            
        });

        return root;
    }
}
