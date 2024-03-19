using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runAwaySpeed = 5f;
    public float detectionRadius = 5f; // 플레이어 감지 범위
    public Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private bool isRunningAway = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ChangeDirection", 0f, 2f); // 게임 시작 시 즉시 무작위 방향 선택, 그 후 2초마다 방향 변경
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer < detectionRadius && !isRunningAway)
        {
            // 플레이어를 감지하면 반대 방향으로 도망
            Vector2 runDirection = (transform.position - playerTransform.position).normalized;
            movementDirection = runDirection;
            isRunningAway = true;
            CancelInvoke("ChangeDirection"); // 도망치는 동안은 무작위 이동 중지
            Invoke("EnableRandomMovement", 2f); // 2초 후에 다시 무작위 이동 시작
        }
    }

    void FixedUpdate()
    {
        // 현재 방향으로 이동한 후의 예상 위치 계산
        Vector2 newPosition = rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime;

        // 예상 위치가 지정된 범위를 벗어나지 않도록 조정
        newPosition.x = Mathf.Clamp(newPosition.x, -16f, 16f);
        newPosition.y = Mathf.Clamp(newPosition.y, -10f, 10f);

        // 조정된 위치로 물고기 이동
        rb.MovePosition(newPosition);
    }

    void ChangeDirection()
    {
        // 무작위 방향으로 이동 설정
        movementDirection = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
        isRunningAway = false; // 도망 상태 해제
    }

    void EnableRandomMovement()
    {
        // 다시 무작위 이동 시작
        isRunningAway = false;
        InvokeRepeating("ChangeDirection", 0f, 2f);
    }
}
