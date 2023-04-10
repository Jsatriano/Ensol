using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RabbitEvadeMode : Node
{
    private float _moveSpeed;
    public RabbitEvadeMode(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public override NodeState Evaluate()
    {
        state = NodeState.SUCCESS;
        return state;
    }
}
