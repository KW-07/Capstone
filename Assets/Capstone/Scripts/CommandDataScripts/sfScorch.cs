using UnityEngine;

[CreateAssetMenu(fileName = "sfScorch", menuName = "CommandData/sfScorch")]
public class sfScorch : CommandData
{
    public int hitCount;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Debug.Log($"{commandNameKor} »ç¿ë");

            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot);

            var hitComponent = effect.AddComponent<ScorchHit>();
            hitComponent.damage = damage;
            hitComponent.hitCount = hitCount;

            Destroy(effect, destroyTime);
        }
    }
}