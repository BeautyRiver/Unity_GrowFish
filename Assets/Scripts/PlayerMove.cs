using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public GameManager manager;
    public float playerScale = 0.3f; //플레이어 크기
    Rigidbody2D rb;
    Animator playerAni; //플레이어 애니메이터
    SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //이동관련
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 playerMove = new Vector2(x, y);
        rb.AddForce(playerMove * speed, ForceMode2D.Impulse);

        //최고속도 제한
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        //Flip
        if (x > 0) transform.localScale = new Vector3(-playerScale, playerScale, 0);

        else if (x < 0) transform.localScale = new Vector3(playerScale, playerScale, 0);
    }

    //먹을 수 있는지 없는지 검사
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
        if (tagName == "Shrimp" && manager.score >= 0) EatFish(tagName, collision, 50);
        else if (tagName == "Sardine" && manager.score >= 500) EatFish(tagName, collision, 200);
        else if (tagName == "Dommy" && manager.score >= 4000) EatFish(tagName, collision, 500);
        else if (tagName == "Tuna" && manager.score >= 10000) EatFish(tagName, collision, 800);

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
        //뒤로 밀려나게
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        Invoke("OffDamaged", 2f);
    }

    void OffDamaged()
    {
        gameObject.layer = 6;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }
}
