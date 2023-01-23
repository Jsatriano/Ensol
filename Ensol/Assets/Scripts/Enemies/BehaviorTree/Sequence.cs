using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        //Constructors
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        //Evaluates the status of all child nodes returning FAILURE if any of them failed, RUNNING if any of them 
        //are still running (and none failed), or SUCCESS if all children succeeded
        public override NodeState Evaluate()
        {
            bool AnyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        AnyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = AnyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }


    }
}
