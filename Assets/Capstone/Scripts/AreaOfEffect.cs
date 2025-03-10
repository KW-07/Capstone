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
    //        Debug.Log($"{collision.gameObject.name}가 들어옴, 현재 몬스터 수 : {enemyInArea.Count}");
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
    //        Debug.Log($"{collision.gameObject.name}가 나감, 현재 몬스터 수 : {enemyInArea.Count}");
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
    //    // 자체적으로 데미지를 줘야될 경우 해당 코드 실행
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
    //        Debug.Log($"{damageEnemy[i].name}에게 {PlayerAttack.instance.playerDamage + usedCommandData.damage}만큼의 데미지를 입혔습니다.");

    //    }
    //    yield return new WaitForSeconds(repeatDelay);

    //    repeatCount--;

    //    if (repeatCount <= 0)
    //    {
    //        StopCoroutine(AreaDamage());
    //    }
    //}
}