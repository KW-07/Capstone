using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    //[SerializeField] private CommandData usedCommandData;
    //[SerializeField] private bool monoBehaviour = true;
    //public int repeatCount;
    //public float repeatDelay;

    //public List<GameObject> enemyInArea = new List<GameObject> ();
    //public List<GameObject> damageEnemy;
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Enemy" && !enemyInArea.Contains(collision.gameObject))
    //    {
    //        enemyInArea.Add(collision.gameObject);
    //        Debug.Log($"{collision.gameObject.name}�� ����, ���� ���� �� : {enemyInArea.Count}");
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (!enemyInArea.Contains(collision.gameObject))
    //    {
    //        enemyInArea.Add(collision.gameObject);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!enemyInArea.Contains(collision.gameObject))
    //    {
    //        enemyInArea.Remove(collision.gameObject);
    //        Debug.Log($"{collision.gameObject.name}�� ����, ���� ���� �� : {enemyInArea.Count}");
    //    }
    //}

    //public List<GameObject> GetObjectsInArea()
    //{
    //    return enemyInArea;
    //}

    //private void Start()
    //{
    //    if (monoBehaviour)
    //    {
    //        StartCoroutine(AreaDamage());
    //    }
    //}
    //private void Update()
    //{
    //    // ��ü������ �������� ��ߵ� ��� �ش� �ڵ� ����
    //    //if(monoBehaviour)
    //    //{
    //    //    StartCoroutine(AreaDamage());
    //    //}
    //}

    //private IEnumerator AreaDamage()
    //{
    //    damageEnemy = enemyInArea;

    //    for (int i = 0; i < damageEnemy.Count; i++)
    //    {
    //        Debug.Log($"{damageEnemy[i].name}���� {PlayerAttack.instance.playerDamage + usedCommandData.damage}��ŭ�� �������� �������ϴ�.");

    //    }
    //    yield return new WaitForSeconds(repeatDelay);

    //    repeatCount--;

    //    if (repeatCount <= 0)
    //    {
    //        StopCoroutine(AreaDamage());
    //    }
    //}
}