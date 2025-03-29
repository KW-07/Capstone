using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler OnSkillPointsChanged;
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType
    {
        None,
        DoubleJump,
        MoveSpeed_1,
        MoveSpeed_2
    }

    private List<SkillType> unlockedSkillTypeList;
    private int skillPoints;

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    // 스킬포인트 획득
    public void AddSkillPoint()
    {
        skillPoints++;
        OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSkillPoints()
    {
        return skillPoints;
    }

    // 스킬 해금
    private void unlockSkill(SkillType skillType)
    {
        if(!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
        }
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    // 해당 스킬을 오픈할 수 있는가
    public bool CanUnlock(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    // 해당 스킬을 오픈하기 위해 필요한 조건
    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch(skillType)
        {
            case SkillType.MoveSpeed_2: return SkillType.MoveSpeed_1;
        }
        return SkillType.None;
    }

    // 스킬 해금을 위해 조건 확인
    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            if(skillPoints > 0)
            {
                skillPoints--;
                OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
                unlockSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
