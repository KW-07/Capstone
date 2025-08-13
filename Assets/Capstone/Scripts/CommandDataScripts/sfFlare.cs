using UnityEngine;

[CreateAssetMenu(fileName = "sfFlare", menuName = "CommandData/sfFlare")]
public class sfFlare : CommandData
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