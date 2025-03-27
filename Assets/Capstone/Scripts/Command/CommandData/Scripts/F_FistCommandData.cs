using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Fist", menuName = "Player/Commands/F_Fist")]
public class F_FistCommandData : CommandData
{
    public float attackRange;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Fist) »ç¿ë");

        GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
        Destroy(effect, destroyTime);

    }
}
