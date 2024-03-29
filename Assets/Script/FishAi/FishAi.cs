using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f; // �⺻ �ӵ�
    public float runAwaySpeed = 4f; // ����ĥ�� �ӵ�
    public float detectionRadius = 5f; // �÷��̾� ���� �Ÿ�
    public float maxDistance; // �÷��̾�� �־����� ��Ȱ��ȭ �� �Ÿ�
    public PlayerMove player; 
    protected Vector2 currentDirection; // �̵��� ����
    protected bool isRunningAway = false; // ����ġ�°�
    [SerializeField] protected float distanceToPlayer; // �÷��̾���� �Ÿ�
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
        SetNewDirection(); // �ʱ� ���� ����
    }

    protected void Update()
    {
        // �� �����Ӹ��� �÷��̾���� �Ÿ� ���
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        // �Ÿ��� maxDistance���� ũ�� �� ���� ������Ʈ�� ��Ȱ��ȭ
        if (distanceToPlayer > maxDistance)
        {
            gameObject.SetActive(false);
        }        
    }  
    private void FixedUpdate()
    {        
        // �̵� ����
        Vector2 targetPosition = rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime;
        targetPosition.y = Mathf.Clamp(targetPosition.y, -9.1f, 10f); // y���� Clamp �Լ��� �����մϴ�.
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

    // ������ �ʱ� ���� ����
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.1f, 0.1f) * 0.1f).normalized;
        spriteRenderer.flipX = currentDirection.x > 0; // �̵� ���⿡ ���� ��������Ʈ ������
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, maxDistance); // �÷��̾� ��ġ�� �߽����� �ϴ� ���� �׸�

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // �÷��̾� ��ġ�� �߽����� �ϴ� ���� �׸�
        }
    }
}
