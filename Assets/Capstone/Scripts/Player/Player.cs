using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public static Player instance { get; private set; }

    [Header("����ġ")]
    public Transform maskAttachPoint;

    [Header("�̵�")]
    public float dir;
    public float originalMoveSpeed = 1f;
    private float moveSpeed;
    private Vector3 stopPosition;
    public bool facingRight = true;

    [Header("����")]
    public float jumpPower = 1f;
    [SerializeField] private int _jumpCount = 0;
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

    [Header("�뽬")]
    public float teleportdis;

    [Header("����")]
    [SerializeField] private List<float> activeSpeedMultipliers = new List<float>();

    [Header("Ŀ�ǵ�")]
    public int[] pCommand = new int[8];
    [SerializeField] private float limitCommandTime;
    [SerializeField] private float inCommandingTimeScale;
    float initTime = 0;
    public float commandingTime = 0;
    [SerializeField] private int commandCount;
    private bool movePossible; // ��ġ�� �̵����� ��ũ��Ʈ�� ���� ����
    private SkillSystem skillSystem;

    [Header("UI")]
    public GameObject commandTimeUI;
    private Vector2 currentPCommandSize = new Vector2(0, 0);
    public GameObject pCommandUI;
    public GameObject pCommandUIGrid;
    public Sprite[] commandIcon;
    [SerializeField] private Image[] pCommandIcon;
    public CommandData[] usableCommandList;

    [Header("����")]
    public Transform shootPoint;
    [SerializeField] private MeleeAttackData normalAttack;
    [SerializeField] private RangeAttackData normalProjectile;
    private bool canRangeAttack = true;
    [SerializeField] private float rangeCooldown;
    [SerializeField] private float initTimeMultipleAttack;
    float cooldownTimer;
    float rangeCooldownTimer;
    [SerializeField] private float attackCountInitTime;

    [Header("������")]
    [SerializeField]private float _playerDamage;
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

    [Header("�� Ž��")] // ���� private���� ���� ����
    // �ʿ� ������ ��ü �� ��
    public GameObject[] allEnemyArray;
    // �÷��̾� ���� ���濡 �����ϴ� �� ��
    public List<GameObject> visibleEnemy;
    [Space(10f)]
    // ���濡 �����ϸ� ���� ����� ��
    public GameObject neareastEnemy;

    [Header("����(or �����)SO")]
    public Buff PoisonBuff;
    
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
        skillSystem = gameObject.GetComponent<SkillSystem>();


        animator.SetFloat("attackCount", normalAttack.multipleAttack);
    }

    private void Start()
    {
        moveSpeed = originalMoveSpeed;

        initTime = 0;
        commandCount = 0;
        GameManager.instance.isCommand = false;
        commandTimeUI.SetActive(false);
        pCommandUI.SetActive(false);
        CommandInitialization(pCommand);
    }

    private void Update()
    {
        if (GameManager.instance.isCommand)
            playerTransform.position = new Vector3(stopPosition.x, playerTransform.position.y, -5);
        else
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -5);

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

        if (GameManager.instance.isCommand)
        {
            InputCommand();
        }
    }

    private void FixedUpdate()
    {
        Move();
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public Transform GetMaskAttachPoint()   //Mask���� ����� ����ġ �Ѱ��ִ� �Լ�
    {
        return maskAttachPoint;
    }

    #region Move

    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }

    // �̵�
    void Move()
    {
        // ��ȭ �� ������ ����
        if (!GameManager.instance.isUI)
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

    // ������ ����
    public void moveStop()
    {
        stopPosition = new Vector3(transform.position.x, transform.position.y, -5);
    }

    // ���� ������
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        commandTimeUI.GetComponent<Transform>().Rotate(0, 180, 0);
        pCommandUIGrid.GetComponent<Transform>().Rotate(0, 180, 0);
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
            moveSpeed = moveSpeed * maxMultiplier;
        }
        else
        {
            //moveSpeed = moveSpeed;
        }

        Debug.Log($"���� �̵� �ӵ�: {moveSpeed}");
    }

    // ����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!GameManager.instance.isUI)
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

    // �Ʒ�����
    public void OnDownJump(InputAction.CallbackContext context)
    {
        if (context.performed && GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == true)
        {
            if (!GameManager.instance.isUI)
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

    // �뽬
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!GameManager.instance.isUI)
            {
                // �뽬�� ���� �� �Ÿ�
                Vector2 dashDirection = facingRight ? Vector2.right : Vector2.left;
                float dashDistance = teleportdis;

                // �÷��̾��� �ݶ��̴� ũ�� üũ
                Vector2 playerColliderSize = gameObject.GetComponent<BoxCollider2D>().size;

                // �ڽ��� Raycast
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
    }
    #endregion

    #region Save&Load
    public void Save(ref PlayerMoveSavaData data)
    {
        data.position = transform.position;
    }

    public void Load(PlayerMoveSavaData data)
    {
        transform.position = data.position;
    }

    #endregion

    #region Collision&Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            GameManager.instance.isGrounded = true;
            animator.SetBool("isJumping", !GameManager.instance.isGrounded);
            animator.SetBool("jumpCommanding", false);

            jumpCount = 0;
            animator.SetFloat("jumpCount", jumpCount);
        }
    }
    #endregion

    #region Attack
    // Melee
    public void OnMeleeAttack(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            if (!GameManager.instance.isUI && GameManager.instance.isGrounded && !GameManager.instance.isCommandAction)
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

    // Range
    public void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!GameManager.instance.isUI && canRangeAttack)
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

    #region RepeatAttack
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
    #endregion

    #region DelaySpawn
    // n�� �� ������Ʈ ����
    public void DelayInstantiate(GameObject obj, Vector2 position, float delayTIme)
    {
        if (delayTIme <= 0)
            return;

        StartCoroutine(DelayInstantiateCoroutine(obj, position, delayTIme));
    }

    private IEnumerator DelayInstantiateCoroutine(GameObject obj, Vector2 position, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Instantiate(obj, position, Quaternion.identity);
    }
    #endregion

    #region Command

    private void InputCommand()
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

            UIManager.instance.deleteCommandCandidate();

            initTime++;
        }
        else if (initTime > 0)
        {

            animator.SetBool("isCommanding", true);
            if (!GameManager.instance.isGrounded && GameManager.instance.isCommand)
                animator.SetBool("jumpCommanding", true);

            commandingTime -= Time.unscaledDeltaTime;
            movePossible = BooleanOnOff(movePossible);

            PCommandCandidate();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                for (int i = 0; i < pCommand.Length; i++)
                {
                    if (pCommand[i] == 0)
                    {
                        pCommand[i] = 1;
                        commandCount++;
                        ShowPCommand(i);

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
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

                        UIManager.instance.deleteCommandCandidate();
                        break;
                    }
                }
            }
            UIManager.instance.CommandCandidate();

            // Ŀ�ǵ� �ð� �ʰ� ��
            if (commandingTime <= 0)
            {
                bool bCommandCount = false;

                animator.SetBool("isCommanding", false);

                animator.SetBool("jumpCommanding", false);
                GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                commandTimeUI.SetActive(false);
                pCommandUI.SetActive(false);

                if (commandCount <= 0)
                {

                }
                // Ŀ�ǵ��� ���� 0 �ʰ��ϸ� ��, ���𰡰� ���ȴٸ�
                else
                {

                    // Ŀ�ǵ� ����Ʈ Ž��
                    for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
                    {
                        // Ŀ�ǵ尡 �����Ѵٸ�
                        if (Enumerable.SequenceEqual(pCommand, CommandManager.instance.commandList[i].command))
                        {
                            Debug.Log("Ŀ�ǵ� : " + CommandManager.instance.commandList[i].commandName);
                            bCommandCount = true;

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

                            CommandInitialization(usableCommandList);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (!bCommandCount)
                    {
                        Debug.Log("Nothing Command!");

                        CommandInitialization(usableCommandList);
                    }
                }
                // End
                initTime = 0;
            }
        }
    }

    public void PlayerCommanding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // �ƹ� ��Ȳ�� �ƴ� ��쿡�� Ŀ�ǵ� ����
            if (!GameManager.instance.isUI)
            {
                GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);
                GameManager.instance.isCommand = true;

                moveStop();

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
    private void CommandInitialization(CommandData[] command)
    {
        for (int i = 0; i < command.Length; i++)
        {
            command[i] = null;
        }
        commandCount = 0;
    }
    #endregion

    #region pCommandUI
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
    #endregion

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

    #region Candidate
    private void PCommandCandidate()
    {
        // Ŀ�ǵ帮��Ʈ�� ������ŭ �Ҵ�
        usableCommandList = new CommandData[CommandManager.instance.commandList.Length];
        int j = 0;

        if (commandCount > 0)
        {
            for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
            {
                if (pCommand.Take(commandCount).SequenceEqual(CommandManager.instance.commandList[i].command.Take(commandCount)))
                {
                    usableCommandList[j] = CommandManager.instance.commandList[i];
                    j++;
                }
            }
        }

        // �ʿ���� �κ��� ����
        Array.Resize(ref usableCommandList, j);
    }
    #endregion
}

[System.Serializable]
public struct PlayerMoveSavaData
{
    public Vector3 position;
}
