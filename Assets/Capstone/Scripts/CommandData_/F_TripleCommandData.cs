using UnityEngine;

[CreateAssetMenu(fileName = "F_Triple", menuName = "Player/Commands/F_Triple")]
public class F_TripleCommandData : CommandData
{
    public float projectileSpeed;
    public float projectileAngle;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandNameKor} น฿ป็!");

        if (effectPrefab != null)
        {
            spawnProjectile(castPoint.transform.rotation, castPoint, target);

            spawnProjectile(Quaternion.Euler(0, 0, projectileAngle) * castPoint.transform.rotation, castPoint, target);

            spawnProjectile(Quaternion.Euler(0, 0, -projectileAngle) * castPoint.transform.rotation, castPoint, target);
        }
    }

    private void spawnProjectile(Quaternion rotaion, GameObject castPoint, GameObject target)
    {
        GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, rotaion);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(target, damage, projectileSpeed, destroyTime);
    }
}
