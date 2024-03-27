using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runAwaySpeed = 4f;
    public float detectionRadius = 5f; // 감지 거리
    public float maxDistance = 20f; // 비활성화 할 거리
    public Transform playerTransform;
    private float distanceToPlayer;
    private Vector2 currentDirection;
    private bool isRunningAway = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // 초기 방향 설정
    }

    private void Update()
    {
        // 매 프레임마다 플레이어와의 거리 계산
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        // 거리가 maxDistance보다 크면 이 게임 오브젝트를 비활성화
        if (distanceToPlayer > maxDistance)
        {
            gameObject.SetActive(false);
        }

        // 플레이어가 감지 범위 내에 있고 도망 상태가 아니라면 도망 시작
        if (distanceToPlayer < detectionRadius && !isRunningAway)
        {
            ChangeDirRunningStart(); // 도망 시작
            Invoke(nameof(ChangeDirAfterRunning), 2f); // 2초 후 방향 변경
        }
    }  
    private void FixedUpdate()
    {
        // 이동 로직
        rb.MovePosition(rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime);
        rb.transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -9.1f, 10f));
    }

    // 무작위 초기 방향 설정
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.1f, 0.1f)).normalized;
        spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
    }

    void OnDrawGizmos()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerTransform.position, maxDistance); // 플레이어 위치를 중심으로 하는 원을 그림

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // 플레이어 위치를 중심으로 하는 원을 그림
        }
    }
}
