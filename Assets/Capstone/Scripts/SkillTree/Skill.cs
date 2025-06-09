using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName = "SkillTree/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;

    public Sprite skillImage;

    public int maxPoints = 4;
    public int currentPoints = 0;

    public List<Skill> prerequisites;

    public List<StatModifier> statModifiers;

    public bool IsMaxed => currentPoints >= maxPoints;

    public float GetStatValue(StatType statType)
    {
        foreach (var mod in statModifiers)
        {
            if (mod.statType == statType)
            {
                int index = Mathf.Clamp(currentPoints - 1, 0, mod.valuesPerLevel.Count - 1);
                return currentPoints > 0 ? mod.valuesPerLevel[index] : 0f;
            }
        }
        return 0f;
    }
}

