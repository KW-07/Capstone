using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public int maxStamina;
    public int currentStamina;

    public bool isBarrier = false;
    public int barrierCount = 0;
    public int barrierPercent = 0;

    public float baseMoveSpeed;
    public float finalMoveSpeed;

    public float baseDamage;
    public float finalDamage;

    public float baseDefense;
    public float finalDefense;

    public float baseChriticalChance;
    public float finalChriticalChance;

    public float baseEvadeChance;
    public float finalEvadeChance;

    private List<BuffInstance> activeBuffs = new List<BuffInstance>();

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void RecalculateStats()
    {
        Debug.Log("RecalculateStats ȣ���");

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        finalMoveSpeed = baseMoveSpeed;
        finalDamage = baseDamage;
        finalDefense = baseDefense;
        finalChriticalChance = baseChriticalChance;
        finalEvadeChance = baseEvadeChance;

        Debug.Log("Initialize Complete!");

        foreach (var buff in activeBuffs)
        {
            switch (buff.Buff.targetStat)
            {
                case BuffTargetStat.Health:
                    currentHealth += buff.ModifierValue;
                    break;
                case BuffTargetStat.MoveSpeed:
                    finalMoveSpeed += buff.ModifierValue;
                    break;
                case BuffTargetStat.Damage:
                    finalDamage += buff.ModifierValue;
                    break;
                case BuffTargetStat.Defence:
                    finalDefense += buff.ModifierValue;
                    break;
                case BuffTargetStat.ChriticalChance:
                    finalChriticalChance += buff.ModifierValue;
                    break;
                case BuffTargetStat.Evade:
                    finalEvadeChance += buff.ModifierValue;
                    break;
            }
        }

        Debug.Log($"���� ������: {finalDamage}");
    }

    public void AddBuffModifier(BuffInstance buff)
    {
        Debug.Log($"AddBuffModifier ȣ��: {buff.Buff.buffName}, ModifierValue: {buff.ModifierValue}");
        activeBuffs.Add(buff);
        RecalculateStats();
    }

    // ���� ���� ��
    public void RemoveBuffModifier(BuffInstance buff)
    {
        activeBuffs.Remove(buff);
        RecalculateStats();
    }

    public void TakeDamage(float amount)
    {
        if (barrierCount <= 0)
            isBarrier = false;

        if (isBarrier)
        {
            currentHealth -= (amount * (barrierPercent / 100));
            barrierCount--;
        }
        else
        {
            currentHealth -= amount;
        }

        Debug.Log($"{gameObject.name}��(��) {amount} �������� ����. ���� ü��: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
