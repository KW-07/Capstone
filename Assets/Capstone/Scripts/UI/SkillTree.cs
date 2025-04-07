using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private Material skillLockedMaterial;
    [SerializeField] private Material skillUnlockableMaterial;
    [SerializeField] private SkillUnlockPath[] skillUnlockPathArray;
    [SerializeField] private Sprite lineSprite;
    [SerializeField] private Sprite lineGlowSprite;

    private PlayerSkills playerSkills;
    private List<SkillButton> skillButtonList;
    private TMPro.TextMeshProUGUI skillPointText;

    private void Awake()
    {
        skillPointText = transform.Find("SkillPointsText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // 플레이어 스킬 세팅
    public void SetPlayerSkills(PlayerSkills playerSkils)
    {
        this.playerSkills = playerSkils;

        skillButtonList = new List<SkillButton>();
        //skillButtonList.Add(new SkillButton(transform.Find("DoubleJumpBtn"), playerSkills, PlayerSkills.SkillType.DoubleJump, skillLockedMaterial, skillUnlockableMaterial));
        //skillButtonList.Add(new SkillButton(transform.Find("MoveSpeed1Btn"), playerSkills, PlayerSkills.SkillType.MoveSpeed_1, skillLockedMaterial, skillUnlockableMaterial));
        //skillButtonList.Add(new SkillButton(transform.Find("MoveSpeed2Btn"), playerSkills, PlayerSkills.SkillType.MoveSpeed_2, skillLockedMaterial, skillUnlockableMaterial));

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnSkillPointsChanged += playerSkills_OnSkillPointsChanged;
        
        UpdateVisuals();
        UpdateSkillPoints();
    }

    // 포인트가 변경되었을 때 스킬포인트 업데이트
    private void playerSkills_OnSkillPointsChanged(object sender, System.EventArgs e)
    {
        UpdateSkillPoints();
    }

    // 스킬이 해금되었을 때 해당 비주얼 업데이트
    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    // 스킬포인트 업데이트
    private void UpdateSkillPoints()
    {
        //skillPointText.SetText(playerSkills.GetSkillPoints().ToString());
    }

    // 비주얼 업데이트
    private void UpdateVisuals()
    {
        foreach(SkillButton skillButton in skillButtonList)
        {
            skillButton.UpdateVisual();
        }

        // 잠금해제할 수 없는 검은 선
        foreach(SkillUnlockPath skillUnlockPath in skillUnlockPathArray)
        {
            foreach(Image linkImage in skillUnlockPath.linkImageArray)
            {
                linkImage.color = Color.black;
                linkImage.sprite = lineSprite;
            }
        }

        foreach(SkillUnlockPath skillUnlockPath in skillUnlockPathArray)
        {
            if(playerSkills.IsSkillUnlocked(skillUnlockPath.skillType) || playerSkills.CanUnlock(skillUnlockPath.skillType))
            {
                // 잠금해제 되었거나 잠금해제할 수 있는 하얀 선
                foreach (Image linkImage in skillUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                    linkImage.sprite = lineGlowSprite;
                }
            }
        }
    }

    // 버튼 세팅
    private class SkillButton
    {
        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private PlayerSkills playerSkills;
        private PlayerSkills.SkillType skillType;
        private Material skillLockedMaterial;
        private Material skillUnlockableMaterial;

        public SkillButton(Transform transform, PlayerSkills playerSkills, PlayerSkills.SkillType skillType, Material skillLockedMaterial, Material skillUnlockableMaterial)
        {
            this.transform = transform;
            this.playerSkills = playerSkills;
            this.skillType = skillType;
            this.skillLockedMaterial = skillLockedMaterial;
            this.skillUnlockableMaterial = skillUnlockableMaterial;

            transform.GetComponent<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(skillType));
        }

        // 해금되었을 때 Material 변경
        public void UpdateVisual()
        {
            if (playerSkills.IsSkillUnlocked(skillType))
            {
                image.material = null;
                backgroundImage.material = null;
            }
            else
            {
                if (playerSkills.CanUnlock(skillType))
                {
                    image.material = skillUnlockableMaterial;
                    backgroundImage.material = skillUnlockableMaterial;
                }
                else
                {
                    image.material = skillLockedMaterial;
                    backgroundImage.material = skillUnlockableMaterial;
                }
            }
        }
    }

    // 다음 트리의 스킬 연결 줄
    [System.Serializable]
    public class SkillUnlockPath
    {
        public PlayerSkills.SkillType skillType;
        public Image[] linkImageArray;
    }
}
