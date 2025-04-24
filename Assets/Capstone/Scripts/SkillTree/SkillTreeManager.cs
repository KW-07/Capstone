using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance { get; private set; }

    public int availablePoints = 10;

    public List<SkillNode> allNodes;

    public List<SkillNode> teNodes;
    public List<SkillNode> tpNodes;
    public List<SkillNode> taNodes;
    public List<SkillNode> tbNodes;
    public List<SkillNode> tdNodes;

    public List<SkillConnection> allConnections;


    public List<SkillNode> currentNodeList = new List<SkillNode>();
    public SkillNode currentNode;
    [SerializeField] private float directionThreshold = 0.7f;

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
        allNodes.AddRange(teNodes);
        allNodes.AddRange(tpNodes);
        allNodes.AddRange(taNodes);
        allNodes.AddRange(tbNodes);
        allNodes.AddRange(tdNodes);

        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        InitCurrentPoints();

        foreach (var node in allNodes)
        {
            Debug.Log($"Node: {node.name}, Skill: {node.skill?.skillName}");
            node.Initialize(this);
        }

        if (currentNode == null && teNodes.Count > 0)
        {
            currentNode = teNodes[0];
            Highlight(currentNode);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveToClosest(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveToClosest(Vector2.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveToClosest(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveToClosest(Vector2.right);

        if(Input.GetKeyDown(KeyCode.Return))
        {
            currentNode.GetComponent<SkillNode>().OnClick();
        }
    }

    public void ChangeNodeList()
    {
        switch (UIManager.instance.skillPageCount)
        {
            case 0:
                currentNodeList.Clear();
                currentNodeList = teNodes.ToList();
                break;
            case 1:
                currentNodeList.Clear();
                currentNodeList = tpNodes.ToList();
                break;
            case 2:
                currentNodeList.Clear();
                currentNodeList = taNodes.ToList();
                break;
            case 3:
                currentNodeList.Clear();
                currentNodeList = tbNodes.ToList();
                break;
            case 4:
                currentNodeList.Clear();
                currentNodeList = tdNodes.ToList();
                break;
            default:
                break;
        }

        foreach(var node in currentNodeList)
        {
            Unhighlight(node);
        }

        currentNode = currentNodeList[0];
        Highlight(currentNode);

        UIManager.instance.SkillNodeDescription(currentNode);
    }

    void MoveToClosest(Vector2 direction)
    {
        SkillNode best = null;
        float bestDot = directionThreshold;
        float closestDistance = float.MaxValue;

        foreach (var node in currentNodeList)
        {
            if (node == currentNode) continue;

            Vector2 toNode = (node.transform.position - currentNode.transform.position);
            float dot = Vector2.Dot(direction.normalized, toNode.normalized);
            float dist = toNode.magnitude;

            if (dot > bestDot || (Mathf.Approximately(dot, bestDot) && dist < closestDistance))
            {
                best = node;
                bestDot = dot;
                closestDistance = dist;
            }
        }

        if (best != null)
        {
            Unhighlight(currentNode);
            currentNode = best;
            Highlight(currentNode);
        }

        UIManager.instance.SkillNodeDescription(currentNode);
    }

    void Highlight(SkillNode node)
    {
        // 선택된 노드 강조 표시 (예: 색상 변경)
        node.SetHighlight(true);
    }

    void Unhighlight(SkillNode node)
    {
        node.SetHighlight(false);
    }

    void InitCurrentPoints()
    {
        foreach(var cPoints in allNodes)
        {
            cPoints.GetComponent<SkillNode>().skill.currentPoints = 0;
        }
    }

    public void resetPoints()
    {
        int sum = 0;
        foreach(var cPoinits in allNodes)
        {
            sum += cPoinits.GetComponent<SkillNode>().skill.currentPoints;
            cPoinits.GetComponent<SkillNode>().skill.currentPoints = 0;
        }
        availablePoints += sum;

        foreach (var node in allNodes)
        {
            Debug.Log($"Node: {node.name}, Skill: {node.skill?.skillName}");
            node.Initialize(this);
        }

        foreach(var node in allNodes)
        {
            Unhighlight(node);
        }
        currentNode = currentNodeList[0];
        Highlight(currentNode);

        UIManager.instance.SkillNodeDescription(currentNode);
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
