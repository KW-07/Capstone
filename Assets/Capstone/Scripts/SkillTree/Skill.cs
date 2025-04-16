using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillTree/Skill")]

public class Skill : ScriptableObject
{
    public string skillName;
    public string description;

    public int maxPoints = 4;
    public int currentPoints = 0;

    public List<Skill> prerequisites;

    [Tooltip("포인트별 효과 설명 (예: 공격력 +10, +20...)")]
    public List<string> effectDescriptions;
    public List<int> powerByLevel;

    public bool IsMaxed => currentPoints >= maxPoints;

    public string GetCurrentEffect()
    {
        if (effectDescriptions == null || effectDescriptions.Count == 0)
            return "";

        int index = Mathf.Clamp(currentPoints - 1, 0, effectDescriptions.Count - 1);
        return currentPoints > 0 ? effectDescriptions[index] : "아직 효과 없음";
    }

    public int GetPower()
    {
        int index = Mathf.Clamp(currentPoints - 1, 0, powerByLevel.Count - 1);
        return currentPoints > 0 ? powerByLevel[index] : 0;
    }
}
