using UnityEngine;

[CreateAssetMenu(fileName = "swPenetrat", menuName = "CommandData/swPenetrat")]
public class swPenetrat : CommandData
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