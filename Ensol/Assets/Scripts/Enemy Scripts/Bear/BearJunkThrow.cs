using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BearJunkThrow : Node
{
    public BearJunkThrow()
    {

    }

    //All gameplay code for this attack takes place in JunkBallManager.cs and JunkBall.cs
    //This script just handles the animations and stages of the attack

    public override NodeState Evaluate()
    {
        //Windup for the attack
        if (GetData("throwJunk") == null) 
        {
            SetData("throwingAnim", true);
            SetData("attacking", true);
            SetData("junk", true);
            state = NodeState.RUNNING;
            return state;
        }
        //Ends the attack when triggered by animation event
        else if (GetData("endThrow") != null)
        {
            ClearData("throwingAnim");
            ClearData("attacking");
            ClearData("junk");
            ClearData("throwJunk");
            ClearData("endThrow");
            state = NodeState.SUCCESS;
            return state;
        }
        //Don't do anything doing the time after ball is thrown but before animation ends
        state = NodeState.RUNNING;
        return state;
    }
}
