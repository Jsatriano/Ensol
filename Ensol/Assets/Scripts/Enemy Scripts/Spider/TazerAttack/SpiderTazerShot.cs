using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpiderTazerShot : Node
{
    private string _attackName;
    private SpiderTazerManager _tazerManager;

    public SpiderTazerShot(string attackName, SpiderTazerManager tazerManager)
    {
        _attackName = attackName;
        _tazerManager = tazerManager;
    }

    public override NodeState Evaluate()
    {
        if ((string)GetData("animation") != _attackName)
        {
            SetData("animation", _attackName);
            SetData("attacking", _attackName);
            _tazerManager.StartTazerAttack();
            state = NodeState.RUNNING;
            return state;
        }
        if (GetData("tazerEnded") != null)
        {
            ClearData("tazerEnded");
            ClearData("attacking");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
