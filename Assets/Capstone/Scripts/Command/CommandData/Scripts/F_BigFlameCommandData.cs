using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_BigFlame", menuName = "Player/Commands/F_BigFlame")]
public class F_BigFlameCommandData : CommandData
{
    //public GameObject secondEffectPrefab;
    //public float secondDuration;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        //Debug.Log($"{commandName}(F_BigFlame) ���");

        //if (effectPrefab != null)
        //{
        //    // ù��° ���߹��� ���� �� ����
        //    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            
        //    // �ι��� ���� ���� �� ����
        //    PlayerAttack.instance.DelayInstantiate(secondEffectPrefab, castPoint.transform.position, destroyTime);
        //    Destroy(effect, destroyTime);

        //    Destroy(secondEffectPrefab, destroyTime + secondDuration);
        //}

        //// ������ġ �浹 üũ �� ���̸� ������
        //// ù��° ���߹��� ���� ������ ������
        //// �ι�° ������ ���� ������Ʈ ����
        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, 0, LayerMask.GetMask("Enemy"));
        //if(effectPrefab.activeSelf)
        //{
        //    foreach (Collider2D enemy in hitEnemies)
        //    {
        //        Debug.Log($"{enemy.name}���� {PlayerAttack.instance.playerDamage + damage}�� ���ظ� ����!");
        //    }
        //}
    }
}
