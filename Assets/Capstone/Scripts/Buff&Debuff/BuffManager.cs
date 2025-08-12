using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<BuffInstance> activeBuffs = new List<BuffInstance>();
    private CharacterStats targetStats;

    private void Awake()
    {
        targetStats = GetComponent<CharacterStats>();
    }

    public void AddBuff(Buff newBuff)
    {
        Debug.Log($"{newBuff.name} Buff On");
        var existing = activeBuffs.Find(b => b.Buff.buffName == newBuff.buffName);

        if (existing != null)
        {
            Debug.Log($"���� {newBuff.buffName} �ߺ� ���� �� �����");
            existing.Refresh(); // ���ӽð� �ʱ�ȭ
            return;
        }

        var instance = new BuffInstance(newBuff, targetStats);
        activeBuffs.Add(instance);
    }

    private void Update()
    {
        foreach (var buff in activeBuffs)
        {
            buff.Update(Time.deltaTime);
        }
        activeBuffs.RemoveAll(b => b.IsFinished);
    }
}
