using UnityEngine;

[CreateAssetMenu(fileName = "sfBlazeLight", menuName = "CommandData/sfBlazeLight")]
public class sfBlazeLight : CommandData
{
    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        // if (effectPrefab != null)
        // {
        //     GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
        //     Destroy(effect, destroyTime);
        // }
    }
}