using Unity.VisualScripting;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public int turnCount = 2; // 사거리 벗어나고 Turn할 횟수
    public float moveSpeed = 2f; // 기본 속도
    public float runAwaySpeed; // 도망칠때 속도
    public float detectionRadius = 5f; // 플레이어 감지 거리
    public float maxDistance; // 플레이어에서 멀어지면 비활성화 할 거리
    public PlayerMove player;

    [SerializeField] protected Vector2 currentDirection; // 이동할 방향
    [SerializeField] public bool isRunningAway = false; // 도망치는가
    [SerializeField] protected float distanceToPlayer; // 플레이어와의 거리
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    private bool isTurnDown = true;
    private bool isTurnUp = true;

    public enum TypeOfFish
    {
        Fish,
        Enemy
    };
    public TypeOfFish fishType;

    protected virtual void Awake()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerMove>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // 초기 방향 설정

        InvokeRepeating(nameof(SetRandomY), 1f, 2.5f); // 2.5초마다 Y값  변경
    }

    protected virtual void OnEnable()
    {
        anim.SetBool(gameObject.tag, true);
        turnCount = 2;
        RandomSpeed(2f, 3.7f);
    }
    protected virtual void Update()
    {
        runAwaySpeed = moveSpeed * 1.4f;
        // 이동 방향에 따라 스프라이트 뒤집기
        Vector3 localScale = transform.localScale;
        if (currentDirection.x > 0)
        {
            // 오른쪽으로 이동하는 경우, localScale.x를 음수로 설정
            localScale.x = -Mathf.Abs(localScale.x);
        }
        else if (currentDirection.x < 0)
        {
            // 왼쪽으로 이동하는 경우, localScale.x를 수로 설정
            localScale.x = Mathf.Abs(localScale.x);
        }

        transform.localScale = localScale;

        // 매 프레임마다 플레이어와의 거리 계산
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        // 거리가 maxDistance보다 크면 이 게임 오브젝트를 비활성화
        if (distanceToPlayer > maxDistance)
        {
            // 몬스터면 비활성화
            if (fishType == TypeOfFish.Enemy)
            {
                gameObject.SetActive(false);
            }
            // 물고기면 U턴            
            else
            {
                // 턴 카운트가 남아있는 경우 위치 업데이트
                if (turnCount > 0)
                {
                    // 물고기 위치 업데이트 로직
                    float newPosX = CalculateNewPositionX(); // 새로운 X 위치 계산
                    float newPosY = transform.position.y; // Y 위치 유지

                    transform.position = new Vector2(newPosX, newPosY); // 새 위치 설정
                    SetRandomY();
                    RandomSpeed(2f, 3.7f); // 속도 및 Y 위치 랜덤 설정
                }
                else
                {
                    // 턴 카운트가 없으면 비활성화
                    gameObject.SetActive(false);
                }

            }
        }
    }
       
    protected virtual void FixedUpdate()
    {
        // 이동 로직
        Vector2 targetPosition = rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime;
        targetPosition.y = Mathf.Clamp(targetPosition.y, -player.maxClampBottom, player.maxClampTop); // y값을 Clamp 함수로 제한합니다.
        rb.MovePosition(targetPosition);

        // 방향 전환 로직, Y축 제한에 도달했을 때 방향을 반전
        if (isTurnDown && transform.position.y > player.maxClampBottom)
        {
            currentDirection.y = -currentDirection.y;
            isTurnDown = false;
            isTurnUp = true;
        }
        else if (isTurnUp && transform.position.y < -player.maxClampBottom)
        {
            currentDirection.y = -currentDirection.y;
            isTurnUp = false;
            isTurnDown = true;
        }
    }

    // 새로운 X 위치를 계산하는 함수
    float CalculateNewPositionX()
    {
        float newPosX = player.transform.position.x; // 초기 위치 설정
        float range = Random.Range(26f, 29f);

        // 플레이어와 물고기의 상대적 위치 및 방향에 따라 X 위치 결정
        if (player.transform.localScale.x > 0 && currentDirection.x > 0)
        {
            return player.transform.position.x - range; // 둘 다 왼쪽 이동
        }
        else if (player.transform.localScale.x < 0 && currentDirection.x < 0)
        {
            return player.transform.position.x + range; // 둘 다 오른쪽 이동
        }
        else if ((player.transform.localScale.x > 0 && currentDirection.x < 0) ||
                 (player.transform.localScale.x < 0 && currentDirection.x > 0))
        {
            SetReverseX(); // 방향 반전
            return player.transform.localScale.x > 0 ? player.transform.position.x - range : player.transform.position.x + range;
        }
        else
        {
            // 예외 상황 처리
            gameObject.SetActive(false);
            return newPosX; // 초기 위치 반환
        }
    }

    // 랜덤한 속도 함수
    protected void RandomSpeed(float minSpeed, float maxSpeed)
    {
        moveSpeed = Random.Range(2f, 4f);
    }
    protected void SetReverseX()
    {
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)).normalized;
    }

    protected void SetRandomY()
    {
        currentDirection = new Vector2(currentDirection.x, Random.Range(-0.2f, 0.2f)).normalized;
    }

    protected void SetRandomDir()
    {
        SetRandomY();
        SetReverseX();
    }
    // 무작위 초기 방향 설정
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f) * 0.1f).normalized;
    }

    // 플레이어와 같은 방향으로 이동 검사 (양수 = 플레이어 향해 이동)
    protected bool IsMovingTowardsPlayer()
    {
        Vector2 toPlayer = (player.transform.position - transform.position).normalized;
        float dotProduct = Vector2.Dot(toPlayer, currentDirection);

        // 플레이어가 움직이지 않는 경우, 동일 방향 체크
        if (player.GetComponent<Rigidbody2D>().velocity.magnitude == 0)
        {
            return dotProduct > 0 && currentDirection.x == Mathf.Sign(player.transform.position.x - transform.position.x);
        }
        return dotProduct > 0;
    }


    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, maxDistance); // 플레이어 위치를 중심으로 하는 원을 그림

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // 물고기 위치를 중심으로 하는 원을 그림 (감지거리)
        }
    }
}
