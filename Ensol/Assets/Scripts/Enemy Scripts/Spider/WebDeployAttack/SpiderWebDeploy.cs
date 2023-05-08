using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpiderWebDeploy : Node
{
    private string _attackName;
    private SpiderWebManager _webManager;

    public SpiderWebDeploy(string attackName, SpiderWebManager webManager)
    {
        _attackName = attackName;
        _webManager = webManager;
    }

    public override NodeState Evaluate()
    {
        if ((string)GetData("animation") != _attackName)
        {
            Debug.Log("Startd WEB!");
            SetData("animation", _attackName);
            SetData("attacking", _attackName);
            _webManager.StartWebDeployAttack();
            state = NodeState.RUNNING;
            return state;
        }
        if (GetData("webEnded") != null)
        {
            ClearData("webEnded");
            ClearData("attacking");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
