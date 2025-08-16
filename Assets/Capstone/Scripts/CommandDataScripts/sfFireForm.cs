using System.ComponentModel.Design;
using UnityEngine;

[CreateAssetMenu(fileName = "sfFireForm", menuName = "CommandData/sfFireForm")]
public class sfFireForm : CommandData
{
    // 공격력 20% 증가
    public Buff attackPowerBuff;
    
    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if(effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);

            var follow = effectInstance.AddComponent<FollowCaster>();
            follow.caster = castPoint;

            Destroy(effectInstance, destroyTime);
        }

        BuffManager buffManager = castPoint.GetComponent<BuffManager>();
        if(buffManager != null && attackPowerBuff != null)
        {
            buffManager.AddBuff(attackPowerBuff);
        }
    }
}