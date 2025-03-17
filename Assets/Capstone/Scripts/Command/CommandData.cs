using System.Collections;
using System.Collections.Generic;
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
    public string commandName;
    public CommandType commandType;
    [TextArea(3, 5)][SerializeField] private string commandEffect;
    [TextArea(3, 5)][SerializeField] private string commandDescription;
    public float cooldown;
    public int damage;

    public bool castPlayerPosition;

    public GameObject effectPrefab;
    public float destroyTime;

    [Header("좌하우상 순서대로 1 ~ 4")]
    [Header("A,S,D,W 순서대로 5 ~ 8")]
    
    public int[] command = new int[8];

    public virtual void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName} 커맨드 사용");
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            Destroy(effect, destroyTime);
        }
    }
}