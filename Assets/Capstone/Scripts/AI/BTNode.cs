using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTNodeState
{
    Success,
    Failure,
    Running
}
public abstract class BTNode
{
    protected List<BTNode> children = new List<BTNode>();
    public void AddChild(BTNode node)
    {
        children.Add(node);
    }
    public virtual BTNodeState Evaluate()
    {
        return BTNodeState.Failure;
    }
}
