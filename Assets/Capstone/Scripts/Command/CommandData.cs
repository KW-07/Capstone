using UnityEngine;

public enum CommandType
{
    Melee,
    Range,
    Dash,
    Buff
}

[CreateAssetMenu(fileName = "Command", menuName = "Player/Commands")]
public class CommandData : ScriptableObject
{
    public float commandID;
    public string commandNameEng;
    public string commandNameKor;
    public CommandType commandType;
    [Header("���Ͽ�� ������� 1 ~ 4\nA,S,D,W ������� 5 ~ 8")]
    public string stringCommand;
    public int[] command = new int[8];

    public string element;
    public float skillRatio;
    public float cooldown;
    public float baseDamage;
    public float damage;
    [TextArea(3, 5)] public string effectDescription;
    [TextArea(3, 5)] public string description;
    [TextArea(3, 5)] public string animDescription;

    public bool castPlayerPosition;

    public GameObject effectPrefab;
    public float spawnDelay;
    public float destroyTime;

    public virtual void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandNameKor} Ŀ�ǵ� ���");
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            Destroy(effect, destroyTime);
        }
    }
}