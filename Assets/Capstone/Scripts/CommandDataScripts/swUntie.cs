using UnityEngine;

[CreateAssetMenu(fileName = "swUntie", menuName = "CommandData/swUntie")]
public class swUntie : CommandData
{
    public Buff evadeChanceBuff;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Debug.Log($"{commandNameKor} ���"); // �ش� ��ų �� ����ϴ��� ����� Ȯ��

            GameObject effectInstance = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity); // ������Ʈ ����

            var follow = effectInstance.AddComponent<FollowCaster>(); // �÷��̾� ����ٴ� ����Ʈ ����
            follow.caster = castPoint; // ����Ʈ ��ġ�� �÷��̾���� �ʱ�ȭ

            Destroy(effectInstance, destroyTime); // �ʱ�����Ʈ ������Ʈ ����
        }

        BuffManager buffManager = castPoint.GetComponent<BuffManager>(); // �����Ŵ��� ��ũ��Ʈ
        if (buffManager != null && evadeChanceBuff != null)
        {
            buffManager.AddBuff(evadeChanceBuff); // �����Ŵ����� �ش� ��ų�� ���� ���
        }

    }
}