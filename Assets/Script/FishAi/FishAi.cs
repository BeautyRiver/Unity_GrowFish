using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f; // 기본 속도
    public float runAwaySpeed = 4f; // 도망칠때 속도
    public float detectionRadius = 5f; // 플레이어 감지 거리
    public float maxDistance; // 플레이어에서 멀어지면 비활성화 할 거리
    public PlayerMove player; 
    protected Vector2 currentDirection; // 이동할 방향
    protected bool isRunningAway = false; // 도망치는가
    [SerializeField] protected float distanceToPlayer; // 플레이어와의 거리
    private Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    private bool isTurnDown = true;
    private bool isTurnUp = true;

    protected void Awake()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerMove>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // 초기 방향 설정
    }

    protected void Update()
    {
        // 매 프레임마다 플레이어와의 거리 계산
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        // 거리가 maxDistance보다 크면 이 게임 오브젝트를 비활성화
        if (distanceToPlayer > maxDistance)
        {
            gameObject.SetActive(false);
        }        
    }  
    private void FixedUpdate()
    {        
        // 이동 로직
        Vector2 targetPosition = rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime;
        targetPosition.y = Mathf.Clamp(targetPosition.y, -9.1f, 10f); // y값을 Clamp 함수로 제한합니다.
        rb.MovePosition(targetPosition);

        // 방향 전환 로직, Y축 제한에 도달했을 때 방향을 반전
        if (isTurnDown && transform.position.y > 8.8f)
        {
            currentDirection.y = -currentDirection.y;            
            isTurnDown = false;
            isTurnUp = true;
        }
        else if (isTurnUp && transform.position.y < -8.8f)
        {
            currentDirection.y = -currentDirection.y;
            isTurnUp = false;
            isTurnDown = true;
        }
    }

    // 무작위 초기 방향 설정
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.1f, 0.1f) * 0.1f).normalized;
        spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, maxDistance); // 플레이어 위치를 중심으로 하는 원을 그림

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // 플레이어 위치를 중심으로 하는 원을 그림
        }
    }
}
