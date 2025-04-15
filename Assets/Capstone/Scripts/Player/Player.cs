using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : LivingEntity 
{
    public static Player instance { get; private set; }

    [Header("Move")]
    public float moveSpeed = 1f;    
    private float currentSpeed;

    public float jumpPower = 1f;    // ���� ����
    public float teleportdis;       // �뽬 �Ÿ�

    private float dir;
    [SerializeField] private int _jumpCount = 0;
    [SerializeField]
    public int jumpCount 
    {
        get
        {
            return _jumpCount;
        }
        set
        {
            _jumpCount = value;
        }
    }

    [Header("Attack")]
    [SerializeField] private List<float> activeSpeedMultipliers = new List<float>();
    [SerializeField] private MeleeAttackData normalAttack;
    [SerializeField] private RangeAttackData normalProjectile;

    private bool canRangeAttack = true;
    [SerializeField] private float rangeCooldown;
    [SerializeField] private float initTimeMultipleAttack;

    float cooldownTimer;
    float rangeCooldownTimer;

    [SerializeField]
    private float _playerDamage;
    [SerializeField] private float attackCountInitTime;

    public Vector2 boxSize;
    private Vector2 normalAttackBoxSize;
    public GameObject[] allEnemyArray;      // �ʿ� ������ ��ü �� ��

    public List<GameObject> visibleEnemy;   // �÷��̾� ���� ���濡 �����ϴ� �� ��
    [Space(10f)]
    public GameObject neareastEnemy;        // ���濡 �����ϸ� ���� ����� ��
    public Transform shootPoint;
    public bool facingRight = true;

    public int[] pCommand = new int[8];

    [SerializeField] private float limitCommandTime;
    [SerializeField] private float inCommandingTimeScale;
    float initTime = 0;
    public float commandingTime = 0;
    private int commandCount;
    private bool movePossible; // ��ġ�� �̵����� ��ũ��Ʈ�� ���� ����

    [Header("Skill")]
    public SkillSystem skillSystem;

    [Header("UI")]
    public GameObject commandTimeUI;
    private Vector2 currentPCommandSize = new Vector2(0, 0);
    public GameObject pCommandUI;
    public GameObject pCommandUIGrid;
    [SerializeField] private Sprite[] commandIcon;
    [SerializeField] private Image[] pCommandIcon;


    Rigidbody2D rb;
    CapsuleCollider2D capsule;
    Transform playerTransform;
    Animator animator;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;

        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        playerTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
        normalAttackBoxSize = boxSize;

        skillSystem = gameObject.GetComponent<SkillSystem>();
        initTime = 0;
        commandCount = 0;
        GameManager.instance.isCommand = false;
        commandTimeUI.SetActive(false);
        pCommandUI.SetActive(false);
        CommandInitialization(pCommand);

        animator = gameObject.GetComponent<Animator>();

        animator.SetFloat("attackCount", normalAttack.multipleAttack);
    }
    private void Update()
    {
        FindEnemy();

        cooldownTimer -= Time.deltaTime;
        rangeCooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            // Multiple Attack
            cooldownTimer = initTimeMultipleAttack;
            normalAttack.multipleAttack = 0;
            animator.SetFloat("attackCount", normalAttack.multipleAttack);
        }

        if (rangeCooldownTimer < 0)
        {
            canRangeAttack = true;
        }
        // Ŀ�ǵ� ���� ��
        if (GameManager.instance.isCommand)
        {
            if (initTime == 0)
            {
                // ���� pCommand �� �ʱ�ȭ
                CommandInitialization(pCommand);

                currentPCommandSize = new Vector2(0, 0);
                // pCommandUI ������ �ʱⰪ ����
                pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

                // pCommandGrid ������ �ʱⰪ ����
                pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = pCommandUI.GetComponent<RectTransform>().sizeDelta;

                initTime++;
            }
            else if (initTime > 0)
            {
                commandingTime -= Time.unscaledDeltaTime;
                movePossible = BooleanOnOff(movePossible);

                Debug.Log(commandCount);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 1;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 2;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 3;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 4;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 5;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 6;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 7;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 8;
                            commandCount++;
                            ShowPCommand(i);
                            break;
                        }
                    }
                }

                // Ŀ�ǵ� �ð� �ʰ� ��
                if (commandingTime <= 0)
                {
                    int sum = 0;
                    bool commandCount = false;

                    GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                    commandTimeUI.SetActive(false);
                    pCommandUI.SetActive(false);

                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        sum += pCommand[i];
                    }

                    // Ŀ�ǵ��� ���� 0 ���϶�� ��, �ƹ��͵� ������ �ʾҴٸ�
                    if (sum <= 0)
                    {
                        Debug.Log("Do not enter!");
                    }
                    // Ŀ�ǵ��� ���� 0 �ʰ��ϸ� ��, ���𰡰� ���ȴٸ�
                    else
                    {
                        Debug.Log("Entered!");

                        // Ŀ�ǵ� ����Ʈ Ž��
                        for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
                        {
                            // Ŀ�ǵ尡 �����Ѵٸ�
                            if (Enumerable.SequenceEqual(pCommand, CommandManager.instance.commandList[i].command))
                            {
                                Debug.Log("Ŀ�ǵ� : " + CommandManager.instance.commandList[i].commandName);
                                commandCount = true;

                                SkillSystem.instance.command = CommandManager.instance.commandList[i];

                                // ��ų ���
                                // ������ ��� �÷��̾� ��ġ���� ����
                                if (CommandManager.instance.commandList[i].castPlayerPosition)
                                {
                                    skillSystem.UseSkill(gameObject, neareastEnemy);
                                }
                                // ������ �ƴ� ��� �÷��̾� ���濡�� ����
                                else
                                {
                                    skillSystem.UseSkill(shootPoint.gameObject, neareastEnemy);
                                }

                                // Ŀ�ǵ� Ÿ�̸� ����
                                commandTimeUI.SetActive(false);
                                pCommandUI.SetActive(false);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (!commandCount)
                        {
                            Debug.Log("Nothing Command!");
                        }
                    }
                    // End
                    initTime = 0;
                    //Time.timeScale = 1.0f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    #region �̵��Լ�
    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }
    void Move()
    {
        // ��ȭ �� ������ ����
        if (GameManager.instance.nothingUI())
        {
            // spriteFlip
            if (dir < 0 && facingRight)
            {
                Flip();
            }
            else if (dir > 0 && !facingRight)
            {
                Flip();
            }

            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

            animator.SetFloat("xVelocity", dir);
        }
    }

    // ���� ���
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        if (multiplier <= 0) return;

        // List�� ���� ȿ�� �߰�
        activeSpeedMultipliers.Add(multiplier);
        // ���� ���� ������ ����ȿ�� ����
        UpdateMoveSpeed();

        StartCoroutine(RemoveSpeedBuffAfterDelay(multiplier, duration));
    }

    private IEnumerator RemoveSpeedBuffAfterDelay(float multiplier, float duration)
    {
        // ���� �ð� �ʰ� �� �������̴� ���� ����
        yield return new WaitForSeconds(duration);
        activeSpeedMultipliers.Remove(multiplier);

        // ���� ������ �����ǰ� ���� ������ ���� ���� ���� ���
        UpdateMoveSpeed();
    }

    // �̵��ӵ� ����
    private void UpdateMoveSpeed()
    {
        if (activeSpeedMultipliers.Count > 0)
        {
            float maxMultiplier = Mathf.Max(activeSpeedMultipliers.ToArray());
            moveSpeed = currentSpeed * maxMultiplier;
        }
        else
        {
            moveSpeed = currentSpeed;
        }

        Debug.Log($"���� �̵� �ӵ�: {moveSpeed}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.instance.nothingUI())
            {
                if (jumpCount < 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    GameManager.instance.isGrounded = false;

                    jumpCount += 1;
                    animator.SetFloat("jumpCount", jumpCount);

                    animator.SetBool("isJumping", !GameManager.instance.isGrounded);
                }
            }
        }
    }
    public void OnDownJump(InputAction.CallbackContext context)
    {
        if (context.performed && GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == true)
        {
            if (GameManager.instance.nothingUI())
            {
                StartCoroutine("coDownJump");
            }
        }
    }
    IEnumerator coDownJump()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        capsule.isTrigger = true;
        float y = transform.position.y;
        while (transform.position.y > y - 1.6f && transform.position.y <= y)
        {
            yield return wait;
        }
        capsule.isTrigger = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // �뽬�� ���� �� �Ÿ�
            Vector2 dashDirection = facingRight ? Vector2.right : Vector2.left;
            float dashDistance = teleportdis;

            // �÷��̾��� �ݶ��̴� ũ�� üũ
            Vector2 playerColliderSize = gameObject.GetComponent<BoxCollider2D>().size;

            // �ڽ��� Raycast �� �� ������ ��
            RaycastHit2D hit = Physics2D.BoxCast(rb.position, playerColliderSize, 0f, dashDirection, dashDistance, LayerMask.GetMask("Ground"));

            // Ground�� ���̾ ���� ���𰡰� �����ٸ� �ش� ������Ʈ �ձ��� �뽬
            if (hit.collider != null)
            {
                float safeDistance = hit.distance - 0.1f;
                rb.MovePosition(rb.position + dashDirection * safeDistance);
                Debug.Log("Dash - �浹 ����!");
            }
            // ���ٸ� ���������� �뽬
            else
            {
                rb.MovePosition(rb.position + dashDirection * dashDistance);
                Debug.Log("Dash - ����");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            GameManager.instance.isGrounded = true;
            animator.SetBool("isJumping", !GameManager.instance.isGrounded);

            jumpCount = 0;
            animator.SetFloat("jumpCount", jumpCount);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

       commandTimeUI.GetComponent<Transform>().Rotate(0, 180, 0);
       pCommandUIGrid.GetComponent<Transform>().Rotate(0, 180, 0);
    }

    #endregion

    #region ���� �Լ�
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

    // Melee
    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            boxSize = normalAttackBoxSize;

            if (GameManager.instance.nothingUI() && GameManager.instance.isGrounded && !GameManager.instance.isCommandAction)
            {
                SkillSystem.instance.command = normalAttack;

                cooldownTimer = initTimeMultipleAttack;
                normalAttack.multipleAttack++;
                if (normalAttack.multipleAttack > 3)
                    normalAttack.multipleAttack = 1;

                skillSystem.UseSkill(shootPoint.gameObject, neareastEnemy);

                Debug.Log(normalAttack.multipleAttack);

                animator.SetTrigger("isMeleeAttack");
                animator.SetFloat("attackCount", normalAttack.multipleAttack);
            }
        }
    }

    // �ݺ����� ����
    public void RepeatAttack(GameObject enemy, int repeatCount, float repeatDelay, float damage)
    {
        if (repeatCount <= 0)
            return;

        StartCoroutine(RepeatAttackCoroutine(enemy, repeatCount, repeatDelay, damage));
    }

    // �ݺ����� coroutine
    private IEnumerator RepeatAttackCoroutine(GameObject enemy, int repeatCount, float repeatDelay, float damage)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            Debug.Log($"{enemy.name}���� {playerDamage + damage}�� ���ظ� ����!");

            yield return new WaitForSeconds(repeatDelay);
        }
    }

    public void DelayInstantiate(GameObject obj, Vector2 position, float delayTIme)
    {
        if (delayTIme <= 0)
            return;

        StartCoroutine(DelayInstantiateCoroutine(obj, position, delayTIme));
    }

    // n�� �� ������Ʈ ����
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
            if (GameManager.instance.nothingUI() && canRangeAttack)
            {
                rangeCooldownTimer = rangeCooldown;
                canRangeAttack = false;

                animator.SetTrigger("isRangeAttack");

                SkillSystem.instance.command = normalProjectile;

                skillSystem.UseSkill(shootPoint.gameObject, neareastEnemy);
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
        if (facingRight)
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
        for (int i = 0; i < visibleEnemy.Count; i++)
        {
            if (i == 0)
            {
                distance = Vector2.Distance(this.gameObject.transform.position, visibleEnemy[i].transform.position);
                neareastEnemy = visibleEnemy[i];
            }
            else
            {
                if (Vector2.Distance(this.gameObject.transform.position, visibleEnemy[i].transform.position) < distance)
                {
                    distance = Vector2.Distance(this.gameObject.transform.position, visibleEnemy[i].transform.position);
                    neareastEnemy = visibleEnemy[i];
                }
            }
        }
        if (neareastEnemy != null)
            neareastEnemy.GetComponent<SpriteRenderer>().color = Color.red;
    }
    #endregion

    #region Ŀ�ǵ� �Լ�

    public void PlayerCommanding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // �ƹ� ��Ȳ�� �ƴ� ��쿡�� Ŀ�ǵ� ����
            if (GameManager.instance.nothingUI())
            {
                GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                GameManager.instance.isCommand = true;

                if (GameManager.instance.isCommand)
                {
                    commandTimeUI.SetActive(true);
                    pCommandUI.SetActive(true);
                    commandingTime = limitCommandTime;
                    //Time.timeScale = inCommandingTimeScale;
                }
                else
                {
                    commandingTime = limitCommandTime;
                    //Time.timeScale = 1.0f;
                    commandTimeUI.SetActive(false);
                    pCommandUI.SetActive(false);

                }
            }
        }
    }

    private void CommandInitialization(int[] command)
    {
        for (int i = 0; i < command.Length; i++)
        {
            command[i] = 0;
        }
        commandCount = 0;
    }

    // �ʱ� ������ ��ȯ
    private void PCommandInitSize()
    {
        currentPCommandSize = new Vector2(
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.left + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.right,
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.y + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.top + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.bottom);

        // pCommand�� �ʱⰪ ����
        pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

        // pCommandGrid�� �ʱⰪ ����
        pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
            pCommandUI.GetComponent<RectTransform>().sizeDelta.x,
            pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void ShowPCommand(int i)
    {
        // ù��° ũ���� �⺻������ ����
        if (commandCount == 1)
        {
            PCommandInitSize();
        }
        // ���� ũ��� ���� ���� ���� ����
        else
        {
            // ����UI ���̰� + ���� ���� ũ�Ⱚ + ���� ����
            pCommandUI.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().spacing.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
            // ����UIGrid ���̰� + ���� ���� ũ�Ⱚ + ���� ����
            pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
        }

        // �̹��� ����
        pCommandIcon[i].sprite = commandIcon[pCommand[i] - 1];
    }

    private bool BooleanOnOff(bool boolean)
    {
        if (boolean)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
}