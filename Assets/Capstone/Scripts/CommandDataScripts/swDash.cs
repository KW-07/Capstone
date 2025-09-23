using UnityEngine;

[CreateAssetMenu(fileName = "swDash", menuName = "CommandData/swDash")]
public class swDash : CommandData
{
    public CommandData command_sfScorch;
    public int stackCount;
    public float stackDamage;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);

            command_sfScorch.damage = command_sfScorch.baseDamage;
            SkillSystem.instance.command = command_sfScorch;
            Player.instance.attackStackCount = stackCount;
            Player.instance.stackDamage = stackDamage;

            Destroy(effect, destroyTime);
        }
    }

}