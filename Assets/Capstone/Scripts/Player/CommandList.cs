using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandList : MonoBehaviour
{
    public static CommandList instance { get; private set; }

    public Command[] commandList;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }
}