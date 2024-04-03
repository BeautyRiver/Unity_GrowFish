using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public float playerScale = 0.3f; //플레이어 크기
    public float x;
    public float y;

    public int maxHp = 100; // 최대 체력값 추가
    public float hp;
    private float healthDecreaseRate = 4f; // 체력이 감소하는 비율(초당)

    public UIManager uiManager;
    public VariableJoystick joystick;

    private GameManager gm;
    private Rigidbody2D rb;
    private Animator playerAni; //플레이어 애니메이터
    private SpriteRenderer spriteRenderer;
    private bool isMoveOk; //움직임 가능 체크 변수

    [SerializeField] private GameObject spawner;

    /* 대쉬 구현 */

    private void Start()
    {
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHp;
        isMoveOk = true;
    }

    private void Update()
    {
        // ESC나 게임오버가 아닐때
        if (!uiManager.isPauseScreenOn && !gm.IsGameOver)
        {
            HpCheck(); // 체력 체크
            DecreaseHealthSecond(); // 초당 체력감소
            uiManager.UpdateHealthBar(hp, maxHp); // 체력바를 업데이트하는 함수 호출 (해당 함수는 UIManager 스크립트에 정의되어 있어야 함)
            spawner.transform.position = transform.position;
            //이동관련
            if (isMoveOk)
            {
                x = joystick.Horizontal;
                y = joystick.Vertical;
            }
            Vector2 playerMove = new Vector2(x, y);

            rb.AddForce(playerMove * speed, ForceMode2D.Impulse);
            rb.transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -9.1f, 8f));

            // 최고속도 제한
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;

            // Flip
            if (x != 0)
            {
                // 현재 localScale 값을 가져옴
                Vector3 localScale = transform.localScale;

                // x가 양수인 경우 오른쪽을 향하므로 localScale.x를 양수로 설정
                // x가 음수인 경우 왼쪽을 향하므로 localScale.x를 음수로 설정
                // localScale.x의 절대값은 그대로 유지하면서 부호만 x의 부호와 일치시킴
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
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 투명
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
