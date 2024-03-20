using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runAwaySpeed = 4f;
    public float detectionRadius = 5f;
    public Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private bool isRunningAway = false;
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 추가


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetNewDirection(); // 초기 방향 설정
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer < detectionRadius && !isRunningAway)
        {
            // 플레이어 감지 시 반대 방향으로 도망
            spriteRenderer.flipX = currentDirection.x < 0; // 이동 방향에 따라 스프라이트 뒤집기
            isRunningAway = true;
            currentDirection = new Vector2(-currentDirection.x,currentDirection.y); // 현재 방향을 반대로 변경
            Invoke(nameof(ChangeDirAfterRunning), 3.5f); // 2초 후 방향 변경
        }
    }

    void FixedUpdate()
    {        
        rb.MovePosition(rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime);
    }

    void SetNewDirection()
    {
        // 무작위 초기 방향 설정
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f)).normalized;
    }
    
    void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // 도망 상태 종료 후 방향 반대로 바꾸기
            isRunningAway = false;
            currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)).normalized; 
        }
    }
}
