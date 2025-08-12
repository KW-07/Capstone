using UnityEngine;

[CreateAssetMenu(fileName = "sfIntenseHeat", menuName = "CommandData/sfIntenseHeat")]
public class sfIntenseHeat : CommandData
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