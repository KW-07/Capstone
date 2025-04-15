using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Fist", menuName = "Player/Commands/F_Fist")]
public class F_FistCommandData : CommandData
{
    public float attackRange;

    GameObject effect;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Fist) »ç¿ë");
        if (Player.instance.facingRight)
        {
            effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
        }
        else
        {
            effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.Euler(0,0,180));
        }
        
        Destroy(effect, destroyTime);

    }
}
