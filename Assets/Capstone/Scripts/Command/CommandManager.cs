using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance;

    [Header("Command List")]
    public CommandData[] commandList;

    public CommandData[] fireCommandList;
    public CommandData[] waterCommandList;
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void initCommandList()
    {
        commandList = new CommandData[0];
    }
    private void Start()
    {
    }

    private void Update()
    {
        
    }
}