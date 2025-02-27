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

    public Vector2 boxSize;
    private Vector2 normalAttackBoxSize;
    public int damage;

    [SerializeField] private GameObject projectilePrefab;
    public Transform target;
    [SerializeField] private float projectileMoveSpeed;

    // �ʿ� ������ ��ü �� ��
    public GameObject[] allEnemyArray;
    // �÷��̾� ���� ���濡 �����ϴ� �� ��
    public List<GameObject> visibleEnemy;
    [Space(10f)]
    // ���濡 �����ϸ� ���� ����� ��
    public GameObject neareastEnemy;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }
    private void Start()
    {
        normalAttackBoxSize = boxSize;
    }

    private void Update()
    {
        FindEnemy();
    }

    // Melee
    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        boxSize = normalAttackBoxSize;

        if (context.performed)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(shootPoint.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Enemy")
                {

                }
            }
            Debug.Log("attack");
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
                NormalProjectileAttack();
            }
        }
    }
    
    // The code at the bottom is related to the attack
    public void NormalProjectileAttack()
    {
        Instantiate(normalProjectile, shootPoint.position, shootPoint.rotation);
    }

    //public void GuidedProjectileAttack()
    //{
    //    GuidedProjectile projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity).GetComponent<GuidedProjectile>();
    //    projectile.InitializeProjectile(target, projectileMoveSpeed);
    //}

    //public void ParabolicProjectileAttack()
    //{
    //    Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    //}

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
