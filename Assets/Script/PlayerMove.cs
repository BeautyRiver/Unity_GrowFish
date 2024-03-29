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
    public ParticleSystem effect;
    public VariableJoystick joystick;

    private GameManager gm;
    private Rigidbody2D rb;
    private Animator playerAni; //플레이어 애니메이터
    private SpriteRenderer spriteRenderer;
    private bool isMoveOk; //움직임 가능 체크 변수

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
        if (!uiManager.isPauseScreenOn && !gm.isGameOver)
        {
            HpCheck();
            DecreaseHealthOverTime();
            uiManager.UpdateHealthBar(hp, maxHp); // 체력바를 업데이트하는 함수 호출 (해당 함수는 UIManager 스크립트에 정의되어 있어야 함)

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
                spriteRenderer.flipX = x < 0;
            }

            playerAni.SetBool("isWalk", x != 0);

            // DieCheck
            if (gm.isGameOver)
                isDie();
        }
    }
    private void DecreaseHealthOverTime()
    {
        // 체력을 초당 healthDecreaseRate만큼 감소
        hp -= healthDecreaseRate * Time.deltaTime;
        hp = Mathf.Max(hp, 0); // 체력이 0 이하로 떨어지지 않도록 함
    }

    //먹을 수 있는지 없는지 검
    private void EatFish(string tagName, Collider2D collision, int plusScore)
    {
        if (collision.CompareTag(tagName))
        {
            collision.gameObject.SetActive(false);
            playerAni.Play("PlayerDoEat");
            gm.score += plusScore;
            uiManager.scoreText.text = gm.score.ToString();
            hp += maxHp * 0.2f;
        }
    }

    //Enemy와 충돌했을때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.tag;

        //상어에 닿으면 즉사
        if (tagName == "Shark") hp = 1;

        if (tagName == "Shrimp" && gm.levels[0])
        {
            EatFish(tagName, collision, gm.Level_1);
        }

        else if (tagName == "Sardine" && gm.levels[1])
        {
            EatFish(tagName, collision, gm.Level_2);
        }

        else if (tagName == "Dommy" && gm.levels[2])
        {
            EatFish(tagName, collision, gm.Level_3);
        }

        else if (tagName == "Tuna" && gm.levels[3])
        {
            EatFish(tagName, collision, gm.Level_4);
        }

        else
        {
            // 플레이어와 몬스터 간의 위치 차이를 계산
            Vector2 bounceDirection = transform.position - collision.transform.position;
            bounceDirection.Normalize(); // 방향 벡터를 정규화
            OnDamaged(bounceDirection);
        }
    }

    //플레이어가 데미지를 입었을때
    private void OnDamaged(Vector2 targetPos)
    {
        playerAni.Play("PlayerDamaged");

        hp -= maxHp * 0.3f; // 최대 체력의 30%를 감소
        hp = Mathf.Max(hp, 0); // 체력이 0 이하로 떨어지지 않도록 합니다.

        gameObject.layer = 7;                            //레이어변경해서 충돌무시
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 투명
        Camera.main.transform.DOShakePosition(0.4f, new Vector3(0.8f, 0.8f, 0)); //화면 흔들리게

        //부딪힐시 딜레이
        isMoveOk = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(Hited());

        //뒤로 밀려나게
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(targetPos * 5f, ForceMode2D.Impulse);
    }

    //사망할때
    private void isDie()
    {
        rb.velocity = Vector2.zero;
        joystick.gameObject.SetActive(false);
        playerAni.enabled = false;
        gm.isGameOver = true;
        isMoveOk = false;                                   // 조작불가
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // 투명도 변경
        spriteRenderer.flipY = true;                        // 방향 아래로 뒤집기
        rb.AddForce(Vector2.down * 2, ForceMode2D.Impulse); // 아래로 추락시키기 
        gameObject.layer = 7;                               // 레이어 수정으로 몹들과의 충돌 x
    }
    //맞았을때
    IEnumerator Hited()
    {
        yield return new WaitForSeconds(0.6f);
        isMoveOk = true; // 움직일 수 있게
        yield return new WaitForSeconds(1f);
        gameObject.layer = 6; // 레이어 다시 Player로
        spriteRenderer.color = new Color(1, 1, 1, 1f); // 색상 다시 돌리기
    }

    //Hp검사
    private void HpCheck()
    {
        if (hp <= 0)
        {
            gm.isGameOver = true;
            uiManager.GameOverScreen();
        }
    }
}
