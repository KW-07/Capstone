using UnityEngine;

[CreateAssetMenu(fileName = "sfFlare", menuName = "CommandData/sfFlare")]
public class sfFlare : CommandData
{
    public int barrierCount;
    public int barrierPercent;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerStats.isBarrier = true;
        playerStats.barrierCount = barrierCount;
        playerStats.barrierPercent = barrierPercent;
    }
}