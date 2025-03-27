using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Kick", menuName = "Player/Commands/F_Kick")]
public class F_KickCommandData : CommandData
{
    public float attackRange;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Fist) »ç¿ë");

        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            Destroy(effect, destroyTime);
        }

    }
}
