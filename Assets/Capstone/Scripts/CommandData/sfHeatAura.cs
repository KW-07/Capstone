using UnityEngine;

[CreateAssetMenu(fileName = "sfHeatAura", menuName = "CommandData/sfHeatAura")]
public class sfHeatAura : CommandData
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