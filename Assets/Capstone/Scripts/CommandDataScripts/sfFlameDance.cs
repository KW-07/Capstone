using UnityEngine;

[CreateAssetMenu(fileName = "sfFlameDance", menuName = "CommandData/sfFlameDance")]
public class sfFlameDance : CommandData
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