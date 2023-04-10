using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RabbitAgroMode : Node
{
    private SphereCollider _hitbox;
    private float _moveSpeed;

    public RabbitAgroMode(SphereCollider hitbox, float moveSpeed)
    {
        _hitbox = hitbox;
        _moveSpeed = moveSpeed;
    }

    public override NodeState Evaluate()
    {
        state = NodeState.SUCCESS;
        return state;
    }
}
