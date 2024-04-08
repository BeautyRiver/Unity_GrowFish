using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMove : MonoBehaviour
{
    
    // 플레이어의 기본 속성
    [Header("플레이어 속성")]
    public float playerScale = 0.3f; //플레이어 크기
    public int maxHp = 100; // 최대 체력값 추가
    public float hp; // 플레이어 현재 체력
    private float healthDecreaseRate = 4f; // 체력이 감소하는 비율 (초당)
    private bool isMoveOk = true; //움직임 가능 체크 변수  

    // 플레이어 이동 관련 속성
    [Header("플레이어 이동")]
    public float maxSpeedNormal;    // 일반 이동 시 최대 속도
    public float maxClampTop;       // 플레이어 Y축 상단 제한 범위
    public float maxClampBottom;    // 플레이어 Y축 하단 제한 범위
    public float speed;             // 플레이어 현재 속도
    Vector2 playerDir;              // 플레이어의 이동 방향
    public float currentMaxSpeed;   // 현재 적용되는 최대 속도
    public float x;                 
    public float y;                 

    // 대쉬 관련 속성
    [Header("대쉬")]
    public float maxDashSpeed = 10f;    // 대쉬 중 최대 속도
    private bool isDashing = false;     // 대쉬 상태인지 여부
    public float dashTime = 0.2f;       // 대쉬 지속 시간
    private Vector2 dashDirection;      // 대쉬 방향

    // 외부 참조
    [Header("외부 참조")]
    public UIManager uiManager;         // UI 매니저 참조
    public VariableJoystick joystick;   // 조이스틱 컨트롤러 참조
    public GameObject spawner;          // 물고기 스포너 오브젝트 참조
    private GameManager gm;             // GM 인스턴스 담을 변수

    // 내부 컴포넌트
    [Header("내부 컴포넌트")]
    private Rigidbody2D rb;     // Rigidbody2D 
    private Animator playerAni; // 플레이어 애니메이터
    private SpriteRenderer spriteRenderer; // 스프라이트 랜더러

    private void Start()
    {
        gm = GameManager.Instance; // 게임 매니저 할당
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 할당
        playerAni = GetComponent<Animator>(); // Animator 컴포넌트 할당
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 할당
        hp = maxHp; // 초기 체력 설정
        currentMaxSpeed = maxSpeedNormal; // 초기 최대 속도 설정
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }
        
        // ESC나 게임오버가 아닐때
        if (!uiManager.isPauseScreenOn && !gm.IsGameOver)
        {
            HpCheck(); // 체력 체크
            DecreaseHealthSecond(); // 초당 체력감소
            uiManager.UpdateHealthBar(hp, maxHp); // 체력바를 업데이트하는 함수 호출 
            spawner.transform.position = transform.position;
            //이동관련
            if (isMoveOk)
            {
                x = joystick.Horizontal;
                y = joystick.Vertical;
            }
            playerDir = new Vector2(x, y);

            rb.AddForce(playerDir * speed, ForceMode2D.Force);
            rb.transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -maxClampBottom, maxClampTop));

            // 최고속도 제한
            if (rb.velocity.magnitude > currentMaxSpeed)
                rb.velocity = rb.velocity.normalized * currentMaxSpeed;

            // Flip
            if (x != 0)
            {
                // 현재 localScale 값을 가져옴
                Vector3 localScale = transform.localScale;

                // x가 음수인 경우 오른쪽을 향하므로 localScale.x를 음수로 설정 (기존그림이 왼쪽 볼때 기준)
                // x가 양수인 경우 왼쪽을 향하므로 localScale.x를 양수로 설정
                localScale.x = Mathf.Abs(localScale.x) * (x < 0 ? 1 : -1);

                // 수정된 localScale을 다시 설정
                transform.localScale = localScale;
            }

            playerAni.SetBool("isWalk", x != 0);

            // 사망체크
            if (gm.IsGameOver)
                isDie();
        }
    }

    // Fish와 충돌했을때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.tag;

        if (tagName == "Lv1" && gm.CurrentMission >= 0) // 레벨1 물고기는 미션1 (0) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(0); // 0번(Level_1) 물고기 인지 체크
            EatFish(collision, gm.Level_1);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Lv2" && gm.CurrentMission >= 2) // 레벨2 물고기는 미션3 (2) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(1); // 1번(Level_2) 물고기 인지 체크
            EatFish(collision, gm.Level_2);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Lv3" && gm.CurrentMission >= 4) // 레벨3 물고기는 미션5 (4) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(2); // 2번(Level_3) 물고기 인지 체크
            EatFish(collision, gm.Level_2);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Lv4" && gm.CurrentMission >= 6) // 레벨4 물고기는 미션7 (6) 부터 먹을 수 있음
        {
            gm.UpdateFishCount(3); // 3번(Level_4) 물고기 인지 체크
            EatFish(collision, gm.Level_2);
            collision.gameObject.SetActive(false);
        }
        else if (tagName == "Shark") // 샤크는 즉시 사망
        {
            OnDamaged(collision.gameObject.transform, 100); // 체력 100% 감소
        }

        else if (tagName == "BlowFish") // 복어는 체력 절반 삭제
        {
            OnDamaged(collision.gameObject.transform, 50); // 체력 50% 감소
        }
        else if (tagName == "SharkMouth") // 상어 입에 감지되었을때
        {
            collision.gameObject.GetComponentInParent<Shark_Ai>().isRunningAway = true;
        }
        else
        {
            OnDamaged(collision.gameObject.transform, 30); // 체력 30% 감소
        }        
    }

    // 대쉬 기능 
    IEnumerator Dash()
    {
        isDashing = true; // 대쉬 상태 시작
        currentMaxSpeed = maxDashSpeed; // 대쉬 속도로 속도 변경
        dashDirection = playerDir; // 플레이어가 바라보는 방향으로 대쉬 방향 설정
        rb.AddForce(dashDirection * speed, ForceMode2D.Impulse); // 대쉬 방향으로 순간적으로 힘을 가함

        // 대쉬 지속 시간 동안 대기
        yield return new WaitForSeconds(dashTime);

        // 대쉬 후 속도 점진적으로 감소
        float startTime = Time.time;
        while (Time.time - startTime < dashTime) // 대쉬가 끝난 후 속도를 서서히 원래 속도로 복귀시킴
        {
            currentMaxSpeed = Mathf.Lerp(maxDashSpeed, maxSpeedNormal, (Time.time - startTime) / dashTime);
            yield return null;
        }

        currentMaxSpeed = maxSpeedNormal; // 속도를 원래대로 복구
        isDashing = false; // 대쉬 상태 종료
    }


    private void DecreaseHealthSecond()
    {
        // 체력을 초당 healthDecreaseRate만큼 감소
        hp -= healthDecreaseRate * Time.deltaTime;
        hp = Mathf.Max(hp, 0); // 체력이 0 이하로 떨어지지 않도록 함
    }

    // 물고기 먹기
    private void EatFish(Collider2D collision, int plusScore)
    {

        collision.gameObject.SetActive(false);
        playerAni.Play("PlayerDoEat");
        gm.Score += plusScore;
        uiManager.scoreText.text = gm.Score.ToString();
        hp += maxHp * 0.2f;
        hp = Mathf.Min(hp, 100);

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
        StartCoroutine(Hited(1f));

        //뒤로 밀려나게 + 색 투명
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(targetPos * 5f, ForceMode2D.Impulse);
        spriteRenderer.color = new Color(1, 1, 1, 0.2f);

        gameObject.layer = 7;                            //레이어변경해서 충돌무시
        Camera.main.transform.DOShakePosition(0.4f, new Vector3(0.8f, 0.8f, 0)); //화면 흔들리게        
    }

    //사망할때
    private void isDie()
    {
        StopCoroutine(nameof(OnDamaged));
        rb.velocity = Vector2.zero;
        joystick.gameObject.SetActive(false);
        playerAni.enabled = false;
        gm.IsGameOver = true;
        isMoveOk = false;                                   // 조작불가
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // 투명도 변경
        spriteRenderer.flipY = true;                        // 방향 아래로 뒤집기
        rb.AddForce(Vector2.down * 2, ForceMode2D.Impulse); // 아래로 추락시키기 
        gameObject.layer = 7;                               // 레이어 수정으로 몹들과의 충돌 x
    }

    // 광고보고 살아나기
    public void SawAd()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        joystick.gameObject.SetActive(true);
        playerAni.enabled = true;
        gm.IsGameOver = false;
        spriteRenderer.flipY = false; // 방향 다시 뒤집기
        hp = maxHp;
        uiManager.OffGameOverScreen();
        StartCoroutine(Hited(1.7f));
    }
    //맞았을때
    IEnumerator Hited(float immortalTime)
    {
        if (hp > 0)
        {
            yield return new WaitForSeconds(0.6f);
            isMoveOk = true; // 움직일 수 있게
            yield return new WaitForSeconds(immortalTime);
            gameObject.layer = 6; // 레이어 다시 Player로
            spriteRenderer.color = new Color(1, 1, 1, 1f); // 색상 다시 돌리기
        }
    }

    //Hp검사
    private void HpCheck()
    {
        if (hp <= 0 && !gm.IsGameOver)
        {
            gm.IsGameOver = true;
            uiManager.OnGameOverScreen();
        }
    }
}
