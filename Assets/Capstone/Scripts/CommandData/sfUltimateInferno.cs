using UnityEngine;

[CreateAssetMenu(fileName = "sfUltimateInferno", menuName = "CommandData/sfUltimateInferno")]
public class sfUltimateInferno : CommandData
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