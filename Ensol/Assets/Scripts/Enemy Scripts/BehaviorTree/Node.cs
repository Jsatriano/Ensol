using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    //Node class used for all behaviors in behavior trees - RYAN

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //Dictionary to store waypoints/attack targets/etc in
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        //Constructors
        public Node()
        {
            parent = null;
        }
        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        //Is defined in task/check classes derived from Node
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        //Adds data to the dictionary at the root node
        public void SetData(string key, object value)
        {
            //if parent is null then it is the root
            if (parent == null)
            {
                _dataContext[key] = value;
            }
            else
            {
                parent.SetData(key, value);
            }
        }

        //returns the value associated with the provided key in the dictionary of the node or any ancestor of the node (else null)
        public object GetData(string key)
        {
            object value = null;
            //Checks if the key exists in the dictionary of the initial node
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }
            //Recursively checks the dictionary of every ancestor of the initial node
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }

        //Erases data from the dictionary (or the dictionary of any ancestor node) by checking all ancestors dictionaries
        //Returns true if key was erased or false if the key didn't exist
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            if (parent != null)
            {
                return parent.ClearData(key);
            }
            else
            {
                return false;
            }
        }
    }
}

