using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillNode : MonoBehaviour
{
    public Skill skill;
    public List<SkillNode> connectedNodes = new List<SkillNode>();

    public Button button;
    public TMP_Text nameText;
    public TMP_Text pointText;
    public TMP_Text effectText;

    private SkillTreeManager skillTreeManager;

    public void Initialize(SkillTreeManager manager)
    {
        skillTreeManager = manager;
        nameText.text = skill.skillName;

        button.onClick.AddListener(OnClick);
        Refresh();
    }

    public void Refresh()
    {
        pointText.text = $"포인트: {skill.currentPoints}/{skill.maxPoints}";
        //effectText.text = $"효과: {skill.GetCurrentEffect()}";

        bool canClick = skillTreeManager.CanUnlock(skill);
        button.interactable = canClick && !skill.IsMaxed;
    }

    void OnClick()
    {
        if (skillTreeManager.UnlockSkill(skill))
        {
            Refresh();
            skillTreeManager.RefreshAllNodes();
        }
    }

    // 자동 연결 실행용
    [ContextMenu("AutoConnecting")]
    public void GenerateConnections()
    {
        foreach (SkillNode target in connectedNodes)
        {
            if (target == null) continue;

            GameObject lineGO = new GameObject($"Line_{skill.name}_to_{target.skill.name}");
            lineGO.transform.SetParent(this.transform.parent); // SkillNode들과 같은 부모에 생성

            Image lineImage = lineGO.AddComponent<Image>();
            lineImage.color = new Color(1f, 1f, 1f, 0.2f); // 기본 옅은 색

            RectTransform rt = lineImage.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localScale = Vector3.one;

            SkillConnection conn = lineGO.AddComponent<SkillConnection>();
            conn.fromNode = this;
            conn.toNode = target;
            conn.lineImage = lineImage;

            // 스킬트리 매니저에 연결 등록 (자동 발견하거나 수동 등록 필요)
            SkillTreeManager manager = FindObjectOfType<SkillTreeManager>();
            if (manager != null && !manager.allConnections.Contains(conn))
            {
                manager.allConnections.Add(conn);
            }

            conn.UpdateLine();
        }
    }
}
