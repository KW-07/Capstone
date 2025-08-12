using UnityEngine;

[CreateAssetMenu(fileName = "sfFlameBlood", menuName = "CommandData/sfFlameBlood")]
public class sfFlameBlood : CommandData
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