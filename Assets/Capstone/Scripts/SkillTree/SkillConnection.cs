using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillConnection : MonoBehaviour
{
    public SkillNode fromNode;
    public SkillNode toNode;
    public Image lineImage;

    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(1f, 1f, 1f, 0.2f);

    private SkillTreeManager manager;

    void Start()
    {
        manager = FindObjectOfType<SkillTreeManager>();
        UpdateLine();
    }

    public void UpdateLine()
    {
        if (fromNode == null || toNode == null || lineImage == null) return;

        if (manager != null && manager.IsUnlocked(fromNode.skill))
            lineImage.color = unlockedColor;
        else
            lineImage.color = lockedColor;

        UpdateLinePosition();
    }

    private void UpdateLinePosition()
    {
        Vector3 start = fromNode.transform.position;
        Vector3 end = toNode.transform.position;

        Vector3 mid = (start + end) / 2f;
        transform.position = mid;

        RectTransform rt = lineImage.GetComponent<RectTransform>();
        float length = Vector3.Distance(start, end);
        rt.sizeDelta = new Vector2(length, 5f);

        Vector3 dir = (end - start).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);
    }
}
