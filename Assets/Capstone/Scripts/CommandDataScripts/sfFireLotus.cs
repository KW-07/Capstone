using UnityEngine;

[CreateAssetMenu(fileName = "sfFireLotus", menuName = "CommandData/sfFireLotus")]
public class sfFireLotus : CommandData
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