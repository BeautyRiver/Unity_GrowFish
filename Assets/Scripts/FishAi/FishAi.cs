using UnityEngine;

public class FishAI : MonoBehaviour
{
    public int turnCount = 2; // ��Ÿ� ����� Turn�� Ƚ��
    public float moveSpeed = 2f; // �⺻ �ӵ�
    public float runAwaySpeed; // ����ĥ�� �ӵ�
    public float detectionRadius = 5f; // �÷��̾� ���� �Ÿ�
    public float maxDistance; // �÷��̾�� �־����� ��Ȱ��ȭ �� �Ÿ�
    public PlayerMove player;

    [SerializeField] protected Vector2 currentDirection; // �̵��� ����
    [SerializeField] public bool isRunningAway = false; // ����ġ�°�
    [SerializeField] protected float distanceToPlayer; // �÷��̾���� �Ÿ�
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

    protected void Awake()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerMove>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // �ʱ� ���� ����

        InvokeRepeating(nameof(SetRandomY), 1f, 2.5f); // 2.5�ʸ��� dir  ����
    }

    protected void OnEnable()
    {
        anim.SetBool(gameObject.tag, true);
        turnCount = 2;
        RandomSpeed(2f, 3.7f);
    }
    protected void Update()
    {
        runAwaySpeed = moveSpeed * 1.4f;
        // �̵� ���⿡ ���� ��������Ʈ ������
        Vector3 localScale = transform.localScale;
        if (currentDirection.x > 0)
        {
            // ���������� �̵��ϴ� ���, localScale.x�� ������ ����
            localScale.x = -Mathf.Abs(localScale.x);
        }
        else if (currentDirection.x < 0)
        {
            // �������� �̵��ϴ� ���, localScale.x�� ���� ����
            localScale.x = Mathf.Abs(localScale.x);
        }

        transform.localScale = localScale;

        // �� �����Ӹ��� �÷��̾���� �Ÿ� ���
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        // �Ÿ��� maxDistance���� ũ�� �� ���� ������Ʈ�� ��Ȱ��ȭ
        if (distanceToPlayer > maxDistance)
        {
            // ���͸� ��Ȱ��ȭ
            if (fishType == TypeOfFish.Enemy)
                gameObject.SetActive(false);
            // ������ U��            
            else
            {
                if (!IsMovingTowardsPlayer() && turnCount > 0)
                {
                    turnCount--;
                    // �÷��̾ �������� �ٶ󺸰� ������ ��������, ������ �ٶ󺸰� ������ ���������� �̵�
                    float newPosX = player.transform.localScale.x > 0 ? player.transform.position.x - 25 : player.transform.position.x + 25;
                    // ���� y ��ġ ����
                    float newPosY = transform.position.y;
                    // �� ��ġ ����
                    transform.position = new Vector2(newPosX, newPosY);
                    //currentDirection = new Vector2(-currentDirection.x, currentDirection.y);
                    SetRandomY();
                    RandomSpeed(2f, 3.7f);
                }
                else
                    gameObject.SetActive(false);
            }
        }
    }

    protected void RandomSpeed(float minSpeed, float maxSpeed)
    {
        moveSpeed = Random.Range(2f, 4f);
    }
    protected void FixedUpdate()
    {
        // �̵� ����
        Vector2 targetPosition = rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime;
        targetPosition.y = Mathf.Clamp(targetPosition.y, -player.maxClampBottom, player.maxClampTop); // y���� Clamp �Լ��� �����մϴ�.
        rb.MovePosition(targetPosition);

        // ���� ��ȯ ����, Y�� ���ѿ� �������� �� ������ ����
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

    protected void SetReverseX()
    {
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-1f, 1f) * 0.1f).normalized;
    }

    protected void SetRandomY()
    {
        currentDirection = new Vector2(currentDirection.x, Random.Range(-1f, 1f) * 0.1f).normalized;
    }
   
    protected void SetRandomDir()
    {
        SetRandomY();
        SetReverseX();
    }
    // ������ �ʱ� ���� ����
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f) * 0.1f).normalized;
    }

    // �÷��̾�� ���� �������� �̵� �˻� (��� = �÷��̾� ���� �̵�)
    protected bool IsMovingTowardsPlayer()
    {
        // Dot Product�� ����Ͽ� ������ üũ
        // Dot Product�� ������ ������ �÷��̾ ���� �̵� ��
        Vector2 toPlayer = (player.transform.position - transform.position).normalized;
        return Vector2.Dot(toPlayer, currentDirection) > 0;
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, maxDistance); // �÷��̾� ��ġ�� �߽����� �ϴ� ���� �׸�

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // ����� ��ġ�� �߽����� �ϴ� ���� �׸� (�����Ÿ�)
        }
    }
}
