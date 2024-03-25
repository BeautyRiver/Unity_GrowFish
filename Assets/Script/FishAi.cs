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


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // 초기 방향 설정
    }

    private void Update()
    {
        // 매 프레임마다 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

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
    }

    // 무작위 초기 방향 설정
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f)).normalized;
        spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
    }

    // 플레이어 감지 시 도망 처리: 현재 방향의 x 값을 반전하여 반대 방향으로 설정
    private void ChangeDirRunningStart()
    {
        isRunningAway = true;
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)); // 현재 방향을 반대로 변경
        spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
    }

    // 도망 상태 종료 후 방향 변경: x축 방향 반전, y축은 새로운 무작위 값으로 설정
    private void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // 도망 상태 종료 후 방향 반대로 바꾸기
            isRunningAway = false;
            currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)).normalized;
            spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
        }
    }
}
