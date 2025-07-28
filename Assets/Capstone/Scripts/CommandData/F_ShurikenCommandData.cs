using UnityEngine;

[CreateAssetMenu(fileName = "F_Shuriken", menuName = "Player/Commands/F_Shuriken")]
public class F_ShurikenCommandData : CommandData
{
    public float projectileSpeed;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName} น฿ป็!");

        if (effectPrefab != null)
        {
            GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Initialize(target, damage, projectileSpeed, destroyTime);
        }
    }
}
