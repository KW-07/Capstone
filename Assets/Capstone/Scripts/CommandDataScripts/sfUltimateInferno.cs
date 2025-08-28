using UnityEngine;

[CreateAssetMenu(fileName = "sfUltimateInferno", menuName = "CommandData/sfUltimateInferno")]
public class sfUltimateInferno : CommandData
{
    public int hitCount;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Debug.Log($"{commandNameKor} »ç¿ë");

            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot);

            var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

            var hitComponent = effect.AddComponent<DamageEffect>();
            hitComponent.damage = playerStats.finalDamage * damage;
            hitComponent.hitCount = hitCount;

            Destroy(effect, destroyTime);
        }
    }
}