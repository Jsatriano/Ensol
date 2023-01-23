using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class FOVCheck : Node
{
    private static int _playerLayerMask = 1 << 3;
    private Transform _transform;
    private float _visionRange;

    public FOVCheck(Transform transform, float visionRange)
    {
        _transform = transform;
        _visionRange = visionRange;
    }

    //Checks to see if enemy can see the player or if they have already seen the player
    public override NodeState Evaluate()
    {
        //Checks if enemy already has data on the player, returns success if yes
        object player = GetData("player");
        if (player == null)
        {
            //Checks if player is within range of the enemy, if so stores the players transform and returns success, otherwise failure
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _visionRange, _playerLayerMask);
            if (colliders.Length > 0)
            {
                SetData("player", colliders[0].transform);
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
        state = NodeState.SUCCESS;
        return state;   
    }
}
