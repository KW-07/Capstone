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
    public Image skillImage;
    public TMP_Text nameText;
    public TMP_Text pointText;
    public TMP_Text effectText;

    private SkillTreeManager skillTreeManager;

    public void SetHighlight(bool isOn)
    {
        // 예: 배경 색 바꾸기
        var image = GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.color = isOn ? Color.yellow : Color.white;
        }
    }

    public void Initialize(SkillTreeManager manager)
    {
        skillTreeManager = manager;
        if(skill.skillImage!= null)
        {
            skillImage.sprite = skill.skillImage;
        }
        nameText.text = skill.skillName;

        button.onClick.AddListener(OnClick);
        Refresh();
    }

    public void Refresh()
    {
        pointText.text = $"{skill.currentPoints}/{skill.maxPoints}";

        bool canClick = skillTreeManager.CanUnlock(skill);
        button.interactable = canClick && !skill.IsMaxed;
    }

    public void OnClick()
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
