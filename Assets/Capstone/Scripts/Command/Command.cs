using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType
{
    Melee,
    Range,
    Dash,
    Buff
}

[CreateAssetMenu]
public class Command : ScriptableObject
{
    public string commandName;
    public CommandType commandType;
    [TextArea(3, 5)][SerializeField] private string commandEffect;
    [TextArea(3, 5)][SerializeField] private string commandDescription;

    [Header("좌하우상 순서대로 1 ~ 4")]
    [Header("A,S,D,W 순서대로 5 ~ 8")]

    public int[] command = new int[8];

    public GameObject commandObject;
}
