using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Kick", menuName = "Player/Commands/F_Kick")]
public class F_KickCommandData : CommandData
{
    public float attackRange;
    GameObject effect;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {   
        Debug.Log($"{commandNameKor}(F_Fist) ���");

        if (Player.instance.facingRight)
        {
            effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
        }
        else
        {
            effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.Euler(0, 0, 180));
        }

        Destroy(effect, destroyTime);

    }
}
