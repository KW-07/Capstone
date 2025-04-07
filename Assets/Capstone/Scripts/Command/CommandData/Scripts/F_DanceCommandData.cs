using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Dance", menuName = "Player/Commands/F_Dance")]
public class F_DanceCommandData : CommandData
{
    public float attackRange;
    public int repeatCount;
    public float repeatDelay;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Dance) ���");

        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            Destroy(effect, destroyTime);
        }
    }
}
