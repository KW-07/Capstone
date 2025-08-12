using UnityEngine;

[CreateAssetMenu(fileName = "sfFireShot", menuName = "CommandData/sfFireShot")]
public class sfFireShot : CommandData
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