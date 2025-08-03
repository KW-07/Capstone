using UnityEngine;

[CreateAssetMenu(fileName = "sfFlameLance", menuName = "CommandData/sfFlameLance")]
public class sfFlameLance : CommandData
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