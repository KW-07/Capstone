using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
    [SerializeField] private bool isAlive;

    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private float respawnTime;

    SkillTreeManager skillTreeManager;

    private void Start()
    {
        skillTreeManager = SkillTreeManager.instance;

        isAlive = true;
    }

    public void RecalculateStats()
    {
        //finalMoveSpeed = baseMoveSpeed;
        //finalDamage = baseDamage;
        //finalDefense = baseDefense;

        if (skillTreeManager == null)
        {
            Debug.LogError("[Stat] skillTreeManager가 null입니다! 연결을 확인하세요.");
            return;
        }

        var unlockedSkills = skillTreeManager.GetUnlockedSkills();
        if (unlockedSkills == null)
        {
            Debug.LogWarning("[Stat] unlockedSkills가 null입니다.");
            return;
        }

        foreach (var skill in unlockedSkills)
        {
            //finalMoveSpeed += skill.GetStatValue(StatType.MoveSpeed);
            //finalDamage += skill.GetStatValue(StatType.Damage);
            //finalDefense += skill.GetStatValue(StatType.Defense);
        }
    }

    public void GameOver()
    {
        isAlive = false;

        // 사망 시 일정 시간 후 리스폰
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        // 현재 맵이 마을일 경우 리스폰포인트에서, 아닐경우 마을 및 리스폰포인트에서 리스폰
        if (SceneManager.GetActiveScene().name == "Village")
        {
            gameObject.transform.position = respawnPoint.transform.position;
            isAlive = true;
        }
        else
        {
            SceneManager.LoadScene("Village");
            gameObject.transform.position = respawnPoint.transform.position;
            isAlive = true;
        }
    }
}
