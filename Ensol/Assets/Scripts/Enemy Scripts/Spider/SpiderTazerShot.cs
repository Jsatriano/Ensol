using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpiderTazerShot : Node
{
    private string _attackName;

    public SpiderTazerShot(string attackName)
    {
        _attackName = attackName;
    }

    public override NodeState Evaluate()
    {
       if ((string)GetData("animation") != _attackName)
        {
            SetData("animation", _attackName);
            SetData("attacking", _attackName);
            state = NodeState.RUNNING;
            return state;
        }
       if (GetData("tazerEnded") != null)
        {
            ClearData("tazerEnded");
            ClearData("attacking");
            state = NodeState.FAILURE;
            return NodeState.FAILURE;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
