using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "CommandSO/Command")]
public class CommandSO : ScriptableObject
{
    public string commandName;

    [Header("좌하우상 순서대로 1 ~ 4")]
    [Header("A,S,D,W 순서대로 5 ~ 8")]

    public int[] command = new int[8];

    public GameObject commandObject;
}
