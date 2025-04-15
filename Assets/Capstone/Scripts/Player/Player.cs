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

    public float jumpPower = 1f;    // 점프 높이
    public float teleportdis;       // 대쉬 거리

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
    public GameObject[] allEnemyArray;      // 맵에 잡히는 전체 적 수

    public List<GameObject> visibleEnemy;   // 플레이어 기준 전방에 존재하는 적 수
    [Space(10f)]
    public GameObject neareastEnemy;        // 전방에 존재하며 가장 가까운 적
    public Transform shootPoint;
    public bool facingRight = true;

    public int[] pCommand = new int[8];

    [SerializeField] private float limitCommandTime;
    [SerializeField] private float inCommandingTimeScale;
    float initTime = 0;
    public float commandingTime = 0;
    private int commandCount;
    private bool movePossible; // 합치고 이동관련 스크립트에 넣을 예정

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
        // 커맨드 시작 시
        if (GameManager.instance.isCommand)
        {
            if (initTime == 0)
            {
                // 기존 pCommand 값 초기화
                CommandInitialization(pCommand);

                currentPCommandSize = new Vector2(0, 0);
                // pCommandUI 사이즈 초기값 설정
                pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

                // pCommandGrid 사이즈 초기값 설정
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

                // 커맨드 시간 초과 시
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

                    // 커맨드의 합이 0 이하라면 즉, 아무것도 누르지 않았다면
                    if (sum <= 0)
                    {
                        Debug.Log("Do not enter!");
                    }
                    // 커맨드의 합이 0 초과하면 즉, 무언가가 눌렸다면
                    else
                    {
                        Debug.Log("Entered!");

                        // 커맨드 리스트 탐색
                        for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
                        {
                            // 커맨드가 존재한다면
                            if (Enumerable.SequenceEqual(pCommand, CommandManager.instance.commandList[i].command))
                            {
                                Debug.Log("커맨드 : " + CommandManager.instance.commandList[i].commandName);
                                commandCount = true;

                                SkillSystem.instance.command = CommandManager.instance.commandList[i];

                                // 스킬 사용
                                // 버프일 경우 플레이어 위치에서 생성
                                if (CommandManager.instance.commandList[i].castPlayerPosition)
                                {
                                    skillSystem.UseSkill(gameObject, neareastEnemy);
                                }
                                // 버프가 아닐 경우 플레이어 전방에서 생성
                                else
                                {
                                    skillSystem.UseSkill(shootPoint.gameObject, neareastEnemy);
                                }

                                // 커맨드 타이머 삭제
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

    #region 이동함수
    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }
    void Move()
    {
        // 대화 중 움직임 제어
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

    // 버프 사용
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        if (multiplier <= 0) return;

        // List에 버프 효과 추가
        activeSpeedMultipliers.Add(multiplier);
        // 가장 높은 배율의 버프효과 적용
        UpdateMoveSpeed();

        StartCoroutine(RemoveSpeedBuffAfterDelay(multiplier, duration));
    }

    private IEnumerator RemoveSpeedBuffAfterDelay(float multiplier, float duration)
    {
        // 버프 시간 초과 후 적용중이던 버프 삭제
        yield return new WaitForSeconds(duration);
        activeSpeedMultipliers.Remove(multiplier);

        // 기존 버프가 해제되고 다음 순위의 가장 높은 버프 사용
        UpdateMoveSpeed();
    }

    // 이동속도 증감
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

        Debug.Log($"현재 이동 속도: {moveSpeed}");
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
            // 대쉬의 방향 및 거리
            Vector2 dashDirection = facingRight ? Vector2.right : Vector2.left;
            float dashDistance = teleportdis;

            // 플레이어의 콜라이더 크기 체크
            Vector2 playerColliderSize = gameObject.GetComponent<BoxCollider2D>().size;

            // 박스형 Raycast 좀 더 안전할 듯
            RaycastHit2D hit = Physics2D.BoxCast(rb.position, playerColliderSize, 0f, dashDirection, dashDistance, LayerMask.GetMask("Ground"));

            // Ground의 레이어를 가진 무언가가 잡힌다면 해당 오브젝트 앞까지 대쉬
            if (hit.collider != null)
            {
                float safeDistance = hit.distance - 0.1f;
                rb.MovePosition(rb.position + dashDirection * safeDistance);
                Debug.Log("Dash - 충돌 감지!");
            }
            // 없다면 정상적으로 대쉬
            else
            {
                rb.MovePosition(rb.position + dashDirection * dashDistance);
                Debug.Log("Dash - 정상");
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

    #region 공격 함수
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
        for (int i = 0; i < repeatCount; i++)
        {
            Debug.Log($"{enemy.name}에게 {playerDamage + damage}의 피해를 입힘!");

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

        // 우측을 보고있다면 오른쪽 적을 좌측을 보고있다면 왼쪽적을 '볼 수 있는' 적으로 판단
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

        // '볼 수 있는' 적 중 가장 가까운 적을 판별
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

    #region 커맨드 함수

    public void PlayerCommanding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 아무 상황이 아닐 경우에만 커맨드 실행
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

    // 초기 사이즈 변환
    private void PCommandInitSize()
    {
        currentPCommandSize = new Vector2(
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.left + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.right,
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.y + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.top + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.bottom);

        // pCommand값 초기값 조정
        pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

        // pCommandGrid값 초기값 조정
        pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
            pCommandUI.GetComponent<RectTransform>().sizeDelta.x,
            pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void ShowPCommand(int i)
    {
        // 첫번째 크기은 기본값으로 설정
        if (commandCount == 1)
        {
            PCommandInitSize();
        }
        // 이후 크기는 일정 값에 따른 조정
        else
        {
            // 현재UI 길이값 + 다음 셀의 크기값 + 사이 간격
            pCommandUI.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().spacing.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
            // 현재UIGrid 길이값 + 다음 셀의 크기값 + 사이 간격
            pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
        }

        // 이미지 삽입
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