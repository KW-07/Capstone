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
            Debug.LogError("[Stat] skillTreeManager�� null�Դϴ�! ������ Ȯ���ϼ���.");
            return;
        }

        var unlockedSkills = skillTreeManager.GetUnlockedSkills();
        if (unlockedSkills == null)
        {
            Debug.LogWarning("[Stat] unlockedSkills�� null�Դϴ�.");
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

        // ��� �� ���� �ð� �� ������
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        // ���� ���� ������ ��� ����������Ʈ����, �ƴҰ�� ���� �� ����������Ʈ���� ������
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
