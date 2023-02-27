using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Inverter : Node
    {
        //Constructors
        public Inverter() : base() { }
        public Inverter(List<Node> children) : base(children) { }

        //Inverts and then returns the result of its children
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.FAILURE;
                        return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}
