using UnityEngine;

[CreateAssetMenu(fileName = "sfFireForm", menuName = "CommandData/sfFireForm")]
public class sfFireForm : CommandData
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