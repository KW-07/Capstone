using UnityEngine;

[CreateAssetMenu(fileName = "sfIntenseHeat", menuName = "CommandData/sfIntenseHeat")]
public class sfIntenseHeat : CommandData
{
    public Buff attackPowerBuff;
    public Buff movementSpeedBuff;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);

            var follow = effectInstance.AddComponent<FollowCaster>();
            follow.caster = castPoint;

            Destroy(effectInstance, destroyTime);
        }

        BuffManager buffManager = castPoint.GetComponent<BuffManager>();
        if (buffManager != null && attackPowerBuff != null)
        {
            buffManager.AddBuff(attackPowerBuff);
        }
        if (buffManager != null && movementSpeedBuff != null)
        {
            buffManager.AddBuff(movementSpeedBuff);
        }
    }
}