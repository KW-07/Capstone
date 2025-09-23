using UnityEngine;

[CreateAssetMenu(fileName = "swLead", menuName = "CommandData/swLead")]
public class swLead : CommandData
{
    public int initCount;
    public int projectileSpeed;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        damage = GameObject.Find("Player").GetComponent<PlayerStats>().finalDamage * damage; // ������ �ʱ�ȭ

        if (effectPrefab != null)
        {
            for (int i = 0; i < initCount; i++) // ���Ÿ� ����Ʈ ������ŭ ���� �� �߻�
            {
                GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);   // ����ü ����
                Projectile projectileScript = projectile.GetComponent<Projectile>(); // ����ü ��ũ��Ʈ ����
                projectileScript.Initialize(target, damage, projectileSpeed, destroyTime, true); // ����ü ��ũ��Ʈ�� �ʿ��� ����(Ÿ��, ������, ����ü�ӵ�, �����ð�, ���� T/F) �ο�, ������ ���̸� T, �׳� ����ü�� F

                var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>(); // �÷��̾� ����

                var hitComponent = projectile.AddComponent<DamageEffect>();
                hitComponent.damage = playerStats.finalDamage * damage;
            }
        }

    }
}