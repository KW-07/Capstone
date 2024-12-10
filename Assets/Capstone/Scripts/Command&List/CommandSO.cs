using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "CommandSO/Command")]
public class CommandSO : ScriptableObject
{
    [SerializeField] private string commandName;

    [Header("���Ͽ�� ������� 1 ~ 4")]
    [Header("A,S,D,W ������� 5 ~ 8")]

    [SerializeField] private int[] command = new int[8];
}
