using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public int availablePoints = 10;

    public List<SkillNode> allNodes;
    private HashSet<Skill> unlockedSkills = new HashSet<Skill>();

    public List<SkillConnection> allConnections;

    void Start()
    {
        foreach (var node in allNodes)
        {
            node.Initialize(this);
        }
    }

    public bool CanUnlock(Skill skill)
    {
        if (skill.currentPoints >= skill.maxPoints)
        {
            Debug.Log($"{skill.skillName}�� �̹� �ִ�ġ��");
            return false;
        }


        if (skill.prerequisites == null || skill.prerequisites.Count == 0)
        {
            Debug.Log($"{skill.skillName}�� ���� ������ ���� �� ���� �� ����");
            return true;
        }

        foreach (var pre in skill.prerequisites)
        {
            if (!pre.IsMaxed)
            {
                Debug.Log($"{skill.skillName}�� {pre.skillName}�� max ���°� �ƴ϶� �� ����");
                return false;
            }
        }

        Debug.Log($"{skill.skillName}�� ���� ���� ���� �� ���� �� ����");
        return true;
    }

    public bool UnlockSkill(Skill skill)
    {
        if (!CanUnlock(skill)) return false;
        if (availablePoints <= 0) return false;

        skill.currentPoints++;
        availablePoints--;
        return true;
    }

    public bool IsUnlocked(Skill skill)
    {
        return skill.IsMaxed;
    }

    public void RefreshAllNodes()
    {
        foreach (var node in allNodes)
            node.Refresh();

        foreach (var conn in allConnections)
            conn.UpdateLine();
    }
}
