using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyMove : FishAI
{
    [SerializeField]
    private enum EnemyNames
    {
        
    }
    public float speed;
    public int checkEnemy;
    GameManager manager;    
    
    Animator animator;
    int moveDir = -1;

    void Awake()
    {    
        manager = FindObjectOfType<GameManager>();
        animator = GetComponentInChildren<Animator>();        
    }
    void Start()
    {      
        //enemyFishs의 배열 길이 만큼 반복 
        for (int i = 0; i < manager.enemyFishs.Length; i++)
        {
            //해당물고기 인지 체크후 해당 애니메이션 할당 
            if (checkEnemy == i)
            {
                animator.SetInteger("EnemyValue", i);
                break;
            }
        }

        if (gameObject.transform.GetChild(0).CompareTag("BlowFish"))
            StartCoroutine(BlowFishRanSpeed());

        //왼쪽스폰시 스프라이트 방향 반대(기존 오른쪽), 왼쪽으로 이동할수 있게 moveDir 음수지정
        if (transform.position.x < 0) //왼쪽스폰
        {
            transform.localScale = new Vector3(-1, 1, 1);
            moveDir = -1;
        }

        else //오른쪽스폰
        {
            transform.localScale = new Vector3(1, 1, 1);
            moveDir = 1;
        }
    }     
    
    void Update()
    {        
        //moveDir을 통한 왼쪽 오른쪽 이동 조정
        transform.Translate(Vector3.left * moveDir * speed * Time.deltaTime,Space.World);
                     

        //x범위 벗어날시 적 삭제
        if (transform.position.x > manager.xRange + 1|| transform.position.x < -(manager.xRange) - 1)
            Destroy(gameObject);
    }

    IEnumerator BlowFishRanSpeed()
    {
        speed = Random.Range(2.9f, 5.4f);
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
    }

   

    // 플레이어 감지 시 도망 처리: 현재 방향의 x 값을 반전하여 반대 방향으로 설정
    private void ChangeDirRunningStart()
    {
        isRunningAway = true;
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.1f, 0.1f)); // 현재 방향을 반대로 변경
        spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
    }

    // 도망 상태 종료 후 방향 변경: x축 방향 반전, y축은 새로운 무작위 값으로 설정
    private void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // 도망 상태 종료 후 방향 반대로 바꾸기
            isRunningAway = false;
            currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.1f, 0.1f)).normalized;
            spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
        }
    }
}
