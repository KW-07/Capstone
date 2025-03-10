using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTParallel : BTNode
{
    public override BTNodeState Evaluate()
    {
        bool anyRunning = false;
        bool allSuccess = true;

        foreach (BTNode node in children)
        {
            BTNodeState result = node.Evaluate();
            if (result == BTNodeState.Running)
            {
                anyRunning = true;
            }
            else if (result == BTNodeState.Failure)
            {
                allSuccess = false;
            }
        }

        if (anyRunning)
            return BTNodeState.Running;
        return allSuccess ? BTNodeState.Success : BTNodeState.Failure;
    }
}
