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
    
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private MeleeAttackData normalAttack;
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

    public Vector2 boxSize;
    private Vector2 normalAttackBoxSize;

    // �ʿ� ������ ��ü �� ��
    public GameObject[] allEnemyArray;
    // �÷��̾� ���� ���濡 �����ϴ� �� ��
    public List<GameObject> visibleEnemy;
    [Space(10f)]
    // ���濡 �����ϸ� ���� ����� ��
    public GameObject neareastEnemy;

    SkillSystem skillSystem;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
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

                Debug.Log("attack");
            }
        }
    }

    public void RepeatAttack(Collider2D enemy, int repeatCount, float repeatDelay, float damage)
    {
        if (repeatCount <= 0)
            return;

        StartCoroutine(RepeatAttackCoroutine(enemy, repeatCount, repeatDelay, damage));
    }

    // �ݺ����� coroutine
    private IEnumerator RepeatAttackCoroutine(Collider2D enemy, int repeatCount, float repeatDelay, float damage)
    {
        for(int i = 0;i < repeatCount;i++)
        {
            Debug.Log($"{enemy.name}���� {PlayerAttack.instance.playerDamage + damage}�� ���ظ� ����!");

            yield return new WaitForSeconds(repeatDelay);
        }
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
            if (GameManager.instance.isCommand == false)
            {
                Instantiate(normalProjectile, shootPoint.position, shootPoint.rotation);
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

        // ������ �����ִٸ� ������ ���� ������ �����ִٸ� �������� '�� �� �ִ�' ������ �Ǵ�
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

        // '�� �� �ִ�' �� �� ���� ����� ���� �Ǻ�
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
