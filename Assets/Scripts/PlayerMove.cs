using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using SysRandom = System.Random; // System의 Random에 대해 별칭 설정

public class PlayerMove : MonoBehaviour
{

    // 플레이어의 기본 속성
    [Header("플레이어 속성")]
    public float playerScale = 0.3f; //플레이어 크기
    public int maxHp = 100; // 최대 체력값 
    public float hp; // 플레이어 현재 체력
    private float healthDecreaseRate = 4f; // 체력이 감소하는 비율 (초당)
    private bool isMoveOk = true; //움직임 가능 체크 변수  
    public Transform mouth;
    public GameObject eatEffect;
    // 플레이어 이동 관련 속성
    [Header("플레이어 이동")]
    public float maxSpeedNormal;    // 일반 이동 시 최대 속도
    public float maxClampTop;       // 플레이어 Y축 상단 제한 범위
    public float maxClampBottom;    // 플레이어 Y축 하단 제한 범위
    public float speed;             // 플레이어 현재 속도
    private Vector2 playerDir;              // 플레이어의 이동 방향
    private Vector2 lastDirection = new Vector2(-1,0); // 마지막 방향을 저장하는 변수
    public float currentMaxSpeed;   // 현재 적용되는 최대 속도
    public float x;
    public float y;

    // 대쉬 관련 속성
    [Header("대쉬")]
    public float dashCoolTime = 5f;     // 대쉬 쿨타임
    public float maxDashSpeed = 10f;    // 대쉬 중 최대 속도
    public bool isDashing = false;     // 대쉬 상태인지 여부
    public float dashTime = 0.2f;       // 대쉬 지속 시간
    private Vector2 dashDirection;      // 대쉬 방향

    // 외부 참조
    [Header("외부 참조")]
    public UIManager uiManager;         // UI 매니저 참조
    public Joystick joystick;   // 조이스틱 컨트롤러 참조
    public GameObject spawner;          // 물고기 스포너 오브젝트 참조
    public PoolManager pm;              // 풀매니저 참조
    private GameManager gm;

    // 내부 컴포넌트
    [Header("내부 컴포넌트")]
    private Rigidbody2D rb;     // Rigidbody2D     
    [HideInInspector] public Animator playerAni; // 플레이어 애니메이터
    private SpriteRenderer spriteRenderer; // 스프라이트 랜더러

    private void Awake()
    {
        playerAni = GetComponent<Animator>(); // Animator 컴포넌트 할당
    }
    private void Start()
    {
        gm = GameManager.Instance; // 게임 매니저 참조
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 할당
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 할당
        hp = maxHp; // 초기 체력 설정
        currentMaxSpeed = maxSpeedNormal; // 초기 최대 속도 설정
    }
    
    private void Update()
    {
        // 게임오버 상태가 아니고 일시정지 화면이 아닐때        
        if (!uiManager.isPauseScreenOn && gm.isGameOver == false)
        {
            #region 대쉬 관련(스페이스바로 가능)
            /*if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
            {
                StartCoroutine(Dash());
            }*/
#endregion

            DecreaseHealthSecond(); // 초당 체력 감소 & 체크
            uiManager.UpdateHealthBar(hp, maxHp); // 체력바를 업데이트하는 함수 호출 
            spawner.transform.position = transform.position;

            //이동관련
            if (isMoveOk)
            {
                x = joystick.Horizontal;
                y = joystick.Vertical;
                
                if (!(x == 0 || y == 0))
                    lastDirection = playerDir;

                playerDir = new Vector2(x, y);
            }

            PlayerFlip(x);
            playerAni.SetBool("isWalk", x != 0);
        }
    }
    private void FixedUpdate()
    {
        if (!uiManager.isPauseScreenOn && !gm.isGameOver)
        {
            // 물리적 이동 처리
            rb.AddForce(playerDir * speed, ForceMode2D.Force);

            // 위치 제한
            rb.transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -maxClampBottom, maxClampTop));

            // 최고속도 제한
            if (rb.velocity.magnitude > currentMaxSpeed)
                rb.velocity = rb.velocity.normalized * currentMaxSpeed;
        }
    }

    #region Collision 관련
    // Fish와 충돌했을때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.tag;

        if (tagName == "Lv1" && gm.currentMission >= 0) // 레벨1 물고기는 미션1 (0) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(0); // 0번(Level_1) 물고기 인지 체크
            EatFish(collision, gm.level_1);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Lv2" && gm.currentMission >= 2) // 레벨2 물고기는 미션3 (2) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(1); // 1번(Level_2) 물고기 인지 체크
            EatFish(collision, gm.level_2);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Lv3" && gm.currentMission >= 4) // 레벨3 물고기는 미션5 (4) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(2); // 2번(Level_3) 물고기 인지 체크
            EatFish(collision, gm.level_2);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Lv4" && gm.currentMission >= 6) // 레벨4 물고기는 미션7 (6) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(3); // 3번(Level_4) 물고기 인지 체크
            EatFish(collision, gm.level_2);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Shark") // 샤크는 즉시 사망
        {
            OnDamaged(collision.gameObject.transform, 100); // 체력 100% 감소
        }

        else if (tagName == "BlowFish") // 복어는 체력 절반 삭제
        {
            OnDamaged(collision.gameObject.transform, 50); // 체력 50% 감소
            collision.gameObject.GetComponent<BlowFish_Ai>().isAttack = true; // 복어 공격 애니메이션 키기
        }
        else if (tagName == "SharkMouth") // 상어 입에 감지되었을때
        {
            collision.gameObject.GetComponentInParent<Shark_Ai>().isRunningAway = true; // 상어 달리기 On
        }
        else
        {
            OnDamaged(collision.gameObject.transform, 30); // 체력 30% 감소
        }
    }
    #endregion

    #region 함수
    // Flip 함수
    private void PlayerFlip(float x)
    {
        if (x != 0)
        {
            // 현재 localScale 값을 가져옴
            Vector3 localScale = transform.localScale;

            // x가 음수인 경우 오른쪽을 향하므로 localScale.x를 음수로 설정 (기존그림이 왼쪽 볼때 기준) x가 양수인 경우 왼쪽을 향하므로 localScale.x를 양수로 설정
            localScale.x = Mathf.Abs(localScale.x) * (x < 0 ? 1 : -1);

            // 수정된 localScale을 다시 설정
            transform.localScale = localScale;
        }
    }

    // 물고기 먹기
    private void EatFish(Collider2D collision, int plusScore)
    {
        eatEffect.SetActive(true);
        eatEffect.transform.position = mouth.transform.position;
        collision.gameObject.SetActive(false);
        playerAni.Play("PlayerDoEat");
        gm.score += plusScore;
        uiManager.scoreText.text = gm.score.ToString();
        hp += maxHp * 0.25f;
        hp = Mathf.Min(hp, 100);
    }

    // 초당 체력 감소
    private void DecreaseHealthSecond()
    {
        // 체력을 초당 healthDecreaseRate만큼 감소
        hp -= healthDecreaseRate * Time.deltaTime;
        hp = Mathf.Max(hp, 0); // 체력이 0 이하로 떨어지지 않도록 함.
        
        // 체력이 0 이하로 떨어지면 게임오버
        if (hp <= 0 && gm.isGameOver == false)
        {
            gm.isGameOver = true;
            isDie();

            uiManager.OnGameOverScreen();
        }
    }
 
    //플레이어가 데미지를 입었을때
    private void OnDamaged(Transform collision, float damage)
    {
        // 플레이어와 몬스터 간의 위치 차이를 계산
        Vector2 targetPos = transform.position - collision.transform.position;
        targetPos.Normalize(); // 방향 벡터를 정규화

        playerAni.Play("PlayerDamaged");

        hp -= maxHp * (damage * 0.01f); // 최대 체력의 damage%를 감소
        hp = Mathf.Max(hp, 0); // 체력이 0 이하로 떨어지지 않도록 합니다.

        //부딪힐시 딜레이
        isMoveOk = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(Hited(0.6f, 1f));

        //뒤로 밀려나게 + 색 투명
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(targetPos * 15f, ForceMode2D.Impulse);
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        gameObject.layer = 7;                            //레이어변경해서 충돌무시
        Camera.main.transform.DOShakePosition(0.4f, new Vector3(0.8f, 0.8f, 0)); //화면 흔들리게        
    }

    //맞았을때
    private IEnumerator Hited(float canNotMoveTime, float immortalTime)
    {
        if (hp > 0)
        {
            // canNotMoveTime 시간 만큼 움직일 수 없음
            yield return new WaitForSeconds(canNotMoveTime);
            joystick.enabled = true;
            isMoveOk = true;
            yield return new WaitForSeconds(immortalTime);
            gameObject.layer = 6; // 레이어 다시 Player로
            spriteRenderer.color = new Color(1, 1, 1, 1f); // 색상 다시 돌리기
        }
    }

    //사망할때
    private void isDie()
    {
        FishAi.DisableFish?.Invoke(); // 물고기 비활성화
        joystick.OnPointerUp(); // 조이스틱 초기화
        StopCoroutine(nameof(OnDamaged)); // 데미지 코루틴 중지
        rb.velocity = Vector2.zero; // 속도 초기화
        joystick.gameObject.SetActive(false); // 조이스틱 비활성화
        playerAni.enabled = false; // 애니메이션 비활성화
        gm.isGameOver = true; // 게임오버 상태로 변경
        isMoveOk = false;                                   // 조작불가
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // 투명도 변경
        spriteRenderer.flipY = true;                        // 방향 아래로 뒤집기
        rb.AddForce(Vector2.down * 2, ForceMode2D.Impulse); // 아래로 추락시키기 
        gameObject.layer = 7;                               // 레이어 수정으로 몹들과의 충돌 x
        
    }
    
    // 광고보고 살아나기
    public void SawAd()
    {
        gm.isGameOver = false; // 게임오버 상태 해제
        pm.StartSpawnFish(); // 물고기 스폰 재시작
        joystick.gameObject.SetActive(true); // 조이스틱 활성화
        playerAni.enabled = true; // 애니메이션 활성화
        spriteRenderer.flipY = false; // 방향 다시 뒤집기
        hp = maxHp; // 체력 초기화
        uiManager.OffGameOverScreen(); // 게임오버 화면 비활성화
        StartCoroutine(Hited(0f, 1.7f)); // 무적시간 1.7초
    }
   
    // 대쉬 기능 
    public IEnumerator Dash()
    {
        isDashing = true; // 대쉬 상태 시작
        currentMaxSpeed = maxDashSpeed; // 대쉬 속도로 속도 변경
        dashDirection = lastDirection; // 플레이어가 바라보는 방향으로 대쉬 방향 설정
        rb.velocity = dashDirection * maxDashSpeed; // 대쉬 방향으로 순간적으로 힘을 가함


        // 대쉬 지속 시간 동안 대기
        yield return new WaitForSeconds(dashTime);

        // 대쉬 후 속도 점진적으로 감소
        float currentTime = 0f;
        while (currentTime < dashTime) // 대쉬가 끝난 후 속도를 서서히 원래 속도로 복귀시킴
        {
            currentTime += Time.deltaTime;
            currentMaxSpeed = Mathf.Lerp(maxDashSpeed, maxSpeedNormal, currentTime / dashTime);
            yield return null;
        }

        currentMaxSpeed = maxSpeedNormal; // 속도를 원래대로 복구
        isDashing = false; // 대쉬 상태 종료
    }
    #endregion
}
