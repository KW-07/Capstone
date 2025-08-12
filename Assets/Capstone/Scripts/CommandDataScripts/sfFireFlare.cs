using UnityEngine;

[CreateAssetMenu(fileName = "sfFireFlare", menuName = "CommandData/sfFireFlare")]
public class sfFireFlare : CommandData
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