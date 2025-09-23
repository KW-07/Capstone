using UnityEngine;

[CreateAssetMenu(fileName = "swBomb", menuName = "CommandData/swBomb")]
public class swBomb : CommandData
{
    public int hitCount;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Debug.Log($"{commandNameKor} ���"); // �ش� ��ų �� ����ϴ��� ����� Ȯ��

            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180); // ���� ���濡 �����ϱ⿡ �÷��̾� ���⿡ ���� �´� ���Ⱚ ����, �ٸ� ��� �˾Ƽ� ����
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot); // �� ������ ����

            var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>(); // �÷��̾��� playerstats ��ũ��Ʈ ����

            var hitComponent = effect.AddComponent<DamageEffect>(); // ����Ʈ�� ������ ������ �ο��ϴ� ��ũ��Ʈ ����
            hitComponent.damage = playerStats.finalDamage * damage; // �ش� ����Ʈ�� �������� �÷��̾��� ������ * ��ų������(flaot, <1) ������ ����
                                                                    // ���� ������ �ο��� ������ ������ DamagerEffect���� �� ��.
            hitComponent.hitCount = hitCount; // ���� �߰��������� ���� ��� �ش� �߰�Ƚ����ŭ �߰� Ÿ�� ���⵵�� ����

            Destroy(effect, destroyTime); // ������ �ο� �� ����Ʈ ����
        }

    }
}