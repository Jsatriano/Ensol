using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpiderWebShot : Node
{
    private string _attackName;
    private WebShotManager _webShotManager;

    public SpiderWebShot(string attackName, WebShotManager webShotManger)
    {
        _attackName = attackName;
        _webShotManager = webShotManger;
    }

    public override NodeState Evaluate()
    {
        if ((string)GetData("animation") != _attackName)
        {
            SetData("animation", _attackName);
            SetData("attacking", _attackName);
            _webShotManager.StartWebShotAttack();
            state = NodeState.RUNNING;
            return state;
        }
        if (GetData("webShotEnded") != null)
        {
            ClearData("webShotEnded");
            ClearData("attacking");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
