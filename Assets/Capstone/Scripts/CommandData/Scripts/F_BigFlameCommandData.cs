using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_BigFlame", menuName = "Player/Commands/F_BigFlame")]
public class F_BigFlameCommandData : CommandData
{
    public float firstEffectDuration;
    public float secondEffectDuration;
    public GameObject secondEffectPrefab;

    public Vector3 secondEffectSpawnPoint;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        castPoint.GetComponent<MonoBehaviour>().StartCoroutine(ExecuteSequentialSpawn(castPoint));
    }

    private IEnumerator ExecuteSequentialSpawn(GameObject caster)
    {
        GameObject firstObj = Instantiate(effectPrefab, caster.transform.position, Quaternion.identity);
        Debug.Log("1번 오브젝트 생성");

        yield return new WaitForSeconds(firstEffectDuration);

        if (firstObj != null)
        {
            Destroy(firstObj);
            Debug.Log("1번 오브젝트 삭제");
        }

        GameObject secondObj = Instantiate(secondEffectPrefab, caster.transform.position + secondEffectSpawnPoint, Quaternion.identity);
        Debug.Log("2번 오브젝트 생성");

        yield return new WaitForSeconds(secondEffectDuration);

        if (secondObj != null)
        {
            Destroy(secondObj);
            Debug.Log("1번 오브젝트 삭제");
        }
    }
}
