using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private bool isAlive;

    public int maxHealth;
    public int currentHealth;

    public int maxStamina;
    public int currentStamina;

    public float baseMoveSpeed;
    public float finalMoveSpeed;

    public float baseDamage;
    public float finalDamage;

    public float baseDefense;
    public float finalDefense;

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
