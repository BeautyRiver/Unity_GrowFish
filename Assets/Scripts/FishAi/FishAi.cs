using UnityEngine;

public class FishAI : MonoBehaviour
{
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
        InvokeRepeating(nameof(SetRandomY), 1f, 2.5f);               
    }

    private void OnEnable()
    {
        anim.SetBool(gameObject.tag,true);
        RandomSpeed();

    }
    protected void Update()
    {
        runAwaySpeed = moveSpeed * 1.3f;
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
            //gameObject.SetActive(false);
            // ���͸� ��Ȱ��ȭ
            if (fishType == TypeOfFish.Enemy)
                gameObject.SetActive(false);
            // ������ U��
            else
            {
                if (!IsMovingTowardsPlayer())
                {
                    // �÷��̾ �������� �ٶ󺸰� ������ ��������, ������ �ٶ󺸰� ������ ���������� �̵�
                    float newPosX = player.transform.localScale.x > 0 ? player.transform.position.x - 30 : player.transform.position.x + 30;
                    // ���� y ��ġ ����
                    float newPosY = transform.position.y;
                    // �� ��ġ ����
                    transform.position = new Vector2(newPosX, newPosY);
                    currentDirection = new Vector2(-currentDirection.x, currentDirection.y);
                    SetRandomY();
                    RandomSpeed();
                }
            }
        }
    }

    protected void RandomSpeed()
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

    protected void SetReverseX()
    {
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-1f, 1f) * 0.1f).normalized;
    }

    protected void SetRandomY()
    {
        currentDirection = new Vector2(currentDirection.x, Random.Range(-1f, 1f) * 0.1f).normalized;
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
