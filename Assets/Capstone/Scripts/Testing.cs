using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private SkillTree skillTree;

    private void Start()
    {
        skillTree.SetPlayerSkills(playerMove.GetPlayerSkills());
    }
}
