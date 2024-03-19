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

    public GameObject LineBottom;
    public GameManager gameManager;
    public UIManager uiManager;
    public ParticleSystem effect;

    Rigidbody2D rb;
    Animator playerAni; //플레이어 애니메이터
    SpriteRenderer spriteRenderer;
    bool isMoveOk; //움직임 가능 체크 변수
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isMoveOk = true;
    }

    void Update()
    {
        //이동관련
        if (!isMoveOk)
            return;
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        

        Vector2 playerMove = new Vector2(x, y);

        rb.AddForce(playerMove * speed, ForceMode2D.Impulse);

        // 최고속도 제한
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        // Flip
        if (x != 0)
        {
            spriteRenderer.flipX = x == -1;
        }

        playerAni.SetBool("isWalk", x != 0);

        // DieCheck
        if (gameManager.isGameOver)
            isDie();

    }
    //먹을 수 있는지 없는지 검
    void EatFish(string tagName, Collider2D collision, int plusScore)
    {
        if (collision.CompareTag(tagName))
        {
            Destroy(collision.gameObject);
            playerAni.Play("PlayerDoEat");
            uiManager.score += plusScore;
            uiManager.scoreText.text = uiManager.score.ToString();
        }
    }

    //Enemy와 충돌했을때
    void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.tag;

        //상어에 닿으면 즉사
        if (tagName == "Shark") uiManager.hp = 1;

        if (tagName == "Shrimp" && gameManager.levels[0]) EatFish(tagName, collision, gameManager.ShrimpPt);
        else if (tagName == "Sardine" && gameManager.levels[1]) EatFish(tagName, collision, gameManager.SardinePt);
        else if (tagName == "Dommy" && gameManager.levels[2]) EatFish(tagName, collision, gameManager.DommyPt);
        else if (tagName == "Tuna" && gameManager.levels[3]) EatFish(tagName, collision, gameManager.TunaPt);

        else
        {
            // 플레이어와 몬스터 간의 위치 차이를 계산
            Vector2 bounceDirection = transform.position - collision.transform.position;
            bounceDirection.Normalize(); // 방향 벡터를 정규화
            OnDamaged(bounceDirection);
        }
    }

    //플레이어가 데미지를 입었을때
    void OnDamaged(Vector2 targetPos)
    {
        playerAni.Play("PlayerDamaged");
        uiManager.hp -= 1;        
        gameObject.layer = 7;                            //레이어변경해서 충돌무시
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 투명
        Camera.main.transform.DOShakePosition(0.4f, new Vector3(0.8f, 0.8f, 0)); //화면 흔들리게

        //부딪힐시 딜레이
        isMoveOk = false;
        rb.velocity = Vector3.zero;
        Invoke("OnMoveOk", 0.6f);

        //뒤로 밀려나게

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(targetPos * 5f, ForceMode2D.Impulse);

        Invoke("OffDamaged", 1f);
    }

    //사망할때
    void isDie()
    {
        playerAni.enabled = false;
        isMoveOk = false;                                   // 조작불가
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // 투명도 변경
        spriteRenderer.flipY = true;                        // 방향 아래로 뒤집기
        rb.AddForce(Vector2.down * 2, ForceMode2D.Impulse); // 아래로 추락시키기 
        gameObject.layer = 7;                               // 레이어 수정으로 몹들과의 충돌 x
        LineBottom.SetActive(false);                        // 바닥 더 아래로(떨어지는 연출)

    }
    //무적 종료
    void OffDamaged()
    {
        gameObject.layer = 6;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }
    //움직임 가능
    void OnMoveOk()
    {
        isMoveOk = true;
    }

}
