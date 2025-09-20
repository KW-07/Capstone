using UnityEngine;

[CreateAssetMenu(fileName = "swBomb", menuName = "CommandData/swBomb")]
public class swBomb : CommandData
{
    public int hitCount;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Debug.Log($"{commandNameKor} 사용"); // 해당 스킬 잘 사용하는지 디버그 확인

            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180); // 보통 전방에 생성하기에 플레이어 방향에 따라 맞는 방향값 저장, 다를 경우 알아서 변경
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot); // 위 방향대로 생성

            var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>(); // 플레이어의 playerstats 스크립트 참조

            var hitComponent = effect.AddComponent<DamageEffect>(); // 이펙트에 근접시 데미지 부여하는 스크립트 부착
            hitComponent.damage = playerStats.finalDamage * damage; // 해당 이펙트의 데미지는 플레이어의 데미지 * 스킬데미지(flaot, <1) 값으로 변경
                                                                    // 추후 데미지 부여는 위에서 부착한 DamagerEffect에서 할 것.
            hitComponent.hitCount = hitCount; // 만약 추가데미지가 있을 경우 해당 추가횟수만큼 추가 타수 생기도록 변경

            Destroy(effect, destroyTime); // 데미지 부여 후 이펙트 삭제
        }

    }
}