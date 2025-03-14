using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance { get; private set; }

    public Transform shootPoint;

    [SerializeField] private MeleeAttackData normalAttack;
    [SerializeField] private RangeAttackData normalProjectile;
    [SerializeField] private float initTimeMultipleAttack;
    float cooldownTimer;

    [SerializeField]
    private float _playerDamage;
    public float playerDamage
    {
        get
        {
            return _playerDamage;
        }
        private set
        {
            _playerDamage = value;
        }
    }

    [SerializeField]
    private int _attackCount = 0;
    public int attackCount
    {
        get
        {
            return _attackCount;
        }
        set
        {
            _attackCount = value;
            if(value == 1)
            {
                animator.SetInteger("attackCount", 1);
            }
            else if(value == 2)
            {
                animator.SetInteger("attackCount", 2);
            }
            else if(value == 3)
            {
                animator.SetInteger("attackCount", 3);
            }
        }
    }

    public Vector2 boxSize;
    private Vector2 normalAttackBoxSize;

    // 맵에 잡히는 전체 적 수
    public GameObject[] allEnemyArray;
    // 플레이어 기준 전방에 존재하는 적 수
    public List<GameObject> visibleEnemy;
    [Space(10f)]
    // 전방에 존재하며 가장 가까운 적
    public GameObject neareastEnemy;

    Animator animator;
    SkillSystem skillSystem;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        normalAttackBoxSize = boxSize;
        skillSystem = gameObject.GetComponent<SkillSystem>();

    }

    private void Update()
    {
        FindEnemy();

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            cooldownTimer = initTimeMultipleAttack;
            normalAttack.multipleAttack = 0;
        }
    }

    // Melee
    public void OnMeleeAttack(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            boxSize = normalAttackBoxSize;

            if(GameManager.instance.nothingState())
            {
                SkillSystem.instance.command = normalAttack;

                cooldownTimer = initTimeMultipleAttack;
                normalAttack.multipleAttack++;
                if (normalAttack.multipleAttack > 3)
                    normalAttack.multipleAttack = 1;

                skillSystem.UseSkill(shootPoint.gameObject, neareastEnemy);
                attackCount = normalAttack.multipleAttack;

                Debug.Log("count :" + _attackCount);
                Debug.Log(normalAttack.multipleAttack);
               
            }
        }
    }

    // 반복공격 실행
    public void RepeatAttack(GameObject enemy, int repeatCount, float repeatDelay, float damage)
    {
        if (repeatCount <= 0)
            return;

        StartCoroutine(RepeatAttackCoroutine(enemy, repeatCount, repeatDelay, damage));
    }

    // 반복공격 coroutine
    private IEnumerator RepeatAttackCoroutine(GameObject enemy, int repeatCount, float repeatDelay, float damage)
    {
        for(int i = 0;i < repeatCount;i++)
        {
            Debug.Log($"{enemy.name}에게 {PlayerAttack.instance.playerDamage + damage}의 피해를 입힘!");

            yield return new WaitForSeconds(repeatDelay);
        }
    }

    public void DelayInstantiate(GameObject obj, Vector2 position, float delayTIme)
    {
        if (delayTIme <= 0)
            return;

        StartCoroutine(DelayInstantiateCoroutine(obj, position, delayTIme));
    }

    // n초 뒤 오브젝트 생성
    private IEnumerator DelayInstantiateCoroutine(GameObject obj, Vector2 position, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Instantiate(obj, position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(shootPoint.position, boxSize);
    }

    // Range
    public void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(GameManager.instance.nothingState())
            {
                SkillSystem.instance.command = normalProjectile ;

                skillSystem.UseSkill(shootPoint.gameObject, neareastEnemy);

                //Debug.Log("Shoot");
            }
        }
    }

    void FindEnemy()
    {
        allEnemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        float distance = 0;

        visibleEnemy.Clear();
        neareastEnemy = null;
        foreach (GameObject enemy in allEnemyArray)
            enemy.GetComponent<SpriteRenderer>().color = Color.white;

        // 우측을 보고있다면 오른쪽 적을 좌측을 보고있다면 왼쪽적을 '볼 수 있는' 적으로 판단
        if (PlayerMove.instance.facingRight)
        {
            foreach (GameObject enemy in allEnemyArray)
            {
                if (enemy.transform.position.x > this.gameObject.transform.position.x)
                {
                    visibleEnemy.Add(enemy);
                }
            }
        }
        else
        {
            foreach (GameObject enemy in allEnemyArray)
            {
                if (enemy.transform.position.x < this.gameObject.transform.position.x)
                {
                    visibleEnemy.Add(enemy);
                }
            }
        }

        // '볼 수 있는' 적 중 가장 가까운 적을 판별
        for(int i = 0; i< visibleEnemy.Count;i++)
        {
            if (i == 0)
            {
                distance = Vector2.Distance(this.gameObject.transform.position, visibleEnemy[i].transform.position);
                neareastEnemy = visibleEnemy[i];
            }
            else
            {
                if(Vector2.Distance(this.gameObject.transform.position, visibleEnemy[i].transform.position) < distance)
                {
                    distance = Vector2.Distance(this.gameObject.transform.position, visibleEnemy[i].transform.position);
                    neareastEnemy = visibleEnemy[i];
                }
            }
        }
        if(neareastEnemy!=null)
            neareastEnemy.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
