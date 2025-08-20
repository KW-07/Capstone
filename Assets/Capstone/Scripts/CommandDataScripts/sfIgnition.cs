using UnityEngine;

[CreateAssetMenu(fileName = "sfIgnition", menuName = "CommandData/sfIgnition")]
public class sfIgnition : CommandData
{
    [Header("DamageCountPerTick")]
    public int hitCount;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.3f;
    
    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot);

            var hitComponent = effect.AddComponent<DamageEffect>();
            hitComponent.damage = damage;
            hitComponent.hitCount = hitCount;

            KnockbackEffect kb = effect.AddComponent<KnockbackEffect>();
            kb.Setup(knockbackForce, knockbackDuration);

            Destroy(effect, destroyTime);
        }
    }
}