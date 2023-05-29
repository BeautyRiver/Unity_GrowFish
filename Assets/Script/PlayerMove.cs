using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public float playerScale = 0.3f; //플레이어 크기
    
    public GameObject LineBottom;
    public GameManager manager;
    public ParticleSystem effect;

    Rigidbody2D rb;
    Animator playerAni; //플레이어 애니메이터
    SpriteRenderer spriteRenderer;
    bool isMoveOk; //움직임 가능 체크 변수
    float lastDirection = 1f; //마지막 방향 체크 변수
    void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isMoveOk = true;
    }
    
    void Update()
    {
        float x = 0, y = 0;
        //이동관련
        if (isMoveOk)
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }
        
        Vector2 playerMove = new Vector2(x, y);

        //isMoveOk가 활성화 되어야 움직일 수 있음
        rb.AddForce(playerMove * speed, ForceMode2D.Impulse);

        //최고속도 제한
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        // Flip
        if (x > 0)
        {
            transform.localScale = new Vector3(-playerScale, playerScale, 1);
            lastDirection = -1f;
        }
        else if (x < 0)
        {
            transform.localScale = new Vector3(playerScale, playerScale, 1);
            lastDirection = 1f;
        }
        else
        {
            transform.localScale = new Vector3(lastDirection * playerScale, playerScale, 1);
        }

        //DieCheck
        if (manager.isGameOver)
            isDie();

    }
    //먹을 수 있는지 없는지 검
    void EatFish(string tagName, Collider2D collision, int plusScore)
    {
        if (collision.CompareTag(tagName))
        {
            Destroy(collision.gameObject);
            playerAni.Play("PlayerDoEat");
            manager.score += plusScore;
            manager.scoreText.text = "Score:" + manager.score.ToString();
        }                
    }

    //Enemy와 충돌했을때
    void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.tag;
        if (tagName == "Shrimp" && manager.levels[0]) EatFish(tagName, collision, manager.ShrimpPt);
        else if (tagName == "Sardine" && manager.levels[1]) EatFish(tagName, collision, manager.SardinePt);
        else if (tagName == "Dommy" && manager.levels[2]) EatFish(tagName, collision, manager.DommyPt);
        else if (tagName == "Tuna" && manager.levels[3]) EatFish(tagName, collision, manager.TunaPt);

        else OnDamaged(collision.transform.position);
    }

    //플레이어가 데미지를 입었을때
    void OnDamaged(Vector2 targetPos)
    {       
        playerAni.Play("PlayerDamaged");
        manager.hp -= 1;


        manager.hpText.text = "Hp: " + manager.hp.ToString();
        

        //레이어변경
        gameObject.layer = 7;

        //투명하게
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //화면 흔들리게
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.5f));

        //부딪힐시 딜레이
        isMoveOk = false;
        Invoke("OnMoveOk", 0.8f);

        //뒤로 밀려나게
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        Invoke("OffDamaged", 2f);
    }

    //사망할때
    void isDie()
    {
        playerAni.enabled = false;
        //조작불가
        isMoveOk = false;
        //투명도 변경
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //방향 아래로 뒤집기
        spriteRenderer.flipY = true;
        //아래로 추락시키기 
        rb.AddForce(Vector2.down * 2, ForceMode2D.Impulse);
        //레이어 수정으로 몹들과의 충돌 x
        gameObject.layer = 7;

        //바닥 더 아래로(떨어지는 연출)
        LineBottom.SetActive(false);
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
