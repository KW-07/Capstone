using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance { get; private set; }

    public int availablePoints = 10;

    public List<SkillNode> allNodes;
    public List<SkillConnection> allConnections;

    public List<Skill> AllSkills => allNodes.Select(n => n.skill).ToList();

    private PlayerStats playerStats;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        foreach (var node in allNodes)
        {
            Debug.Log($"Node: {node.name}, Skill: {node.skill?.skillName}");
            node.Initialize(this);
        }
    }

    public bool CanUnlock(Skill skill)
    {
        if (skill.currentPoints >= skill.maxPoints)
        {
            return false;
        }


        if (skill.prerequisites == null || skill.prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var pre in skill.prerequisites)
        {
            if (!pre.IsMaxed)
            {
                return false;
            }
        }

        return true;
    }

    public bool UnlockSkill(Skill skill)
    {
        if (!CanUnlock(skill)) return false;
        if (availablePoints <= 0) return false;

        skill.currentPoints++;
        availablePoints--;

        playerStats.RecalculateStats();
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
    }

    public List<Skill> GetUnlockedSkills()
    {
        if (allNodes == null || allNodes.Count == 0)
        {
            Debug.LogWarning("[SkillTreeManager] allNodes 비어있음");
            return new List<Skill>();
        }

        return allNodes
            .Where(n => n.skill != null && n.skill.currentPoints > 0)
            .Select(n => n.skill)
            .ToList();
    }
}
