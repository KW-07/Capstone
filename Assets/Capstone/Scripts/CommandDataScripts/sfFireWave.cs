using UnityEngine;

[CreateAssetMenu(fileName = "sfFireWave", menuName = "CommandData/sfFireWave")]
public class sfFireWave : CommandData
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