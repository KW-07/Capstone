using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRandomSelector : BTNode
{
    private System.Random random = new System.Random();

    public override BTNodeState Evaluate()
    {
        if (children.Count == 0) return BTNodeState.Failure;

        int randomIndex = random.Next(children.Count);
        return children[randomIndex].Evaluate();
    }
}