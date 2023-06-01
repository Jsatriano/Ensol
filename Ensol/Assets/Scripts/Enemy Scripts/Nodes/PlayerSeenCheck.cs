using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class PlayerSeenCheck : Node
{
    public PlayerSeenCheck()
    {

    }

    //Checks to see if enemy has already seen the player, returning SUCCESS if yes, otherwise no
    public override NodeState Evaluate()
    {
        //Checks if enemy has already seen player
        if (GetData("player") != null)
        {       
            state = NodeState.SUCCESS;
            return state;
        }        
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
