using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance;

    [Header("Command List")]
    public CommandData[] commandList;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
}
