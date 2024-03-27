using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runAwaySpeed = 4f;
    public float detectionRadius = 5f; // ���� �Ÿ�
    public float maxDistance = 20f; // ��Ȱ��ȭ �� �Ÿ�
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
        SetNewDirection(); // �ʱ� ���� ����
    }

    private void Update()
    {
        // �� �����Ӹ��� �÷��̾���� �Ÿ� ���
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        // �Ÿ��� maxDistance���� ũ�� �� ���� ������Ʈ�� ��Ȱ��ȭ
        if (distanceToPlayer > maxDistance)
        {
            gameObject.SetActive(false);
        }

        // �÷��̾ ���� ���� ���� �ְ� ���� ���°� �ƴ϶�� ���� ����
        if (distanceToPlayer < detectionRadius && !isRunningAway)
        {
            ChangeDirRunningStart(); // ���� ����
            Invoke(nameof(ChangeDirAfterRunning), 2f); // 2�� �� ���� ����
        }
    }  
    private void FixedUpdate()
    {
        // �̵� ����
        rb.MovePosition(rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime);
        rb.transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -9.1f, 10f));
    }

    // ������ �ʱ� ���� ����
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.1f, 0.1f)).normalized;
        spriteRenderer.flipX = currentDirection.x > 0; // �̵� ���⿡ ���� ��������Ʈ ������
    }

    void OnDrawGizmos()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerTransform.position, maxDistance); // �÷��̾� ��ġ�� �߽����� �ϴ� ���� �׸�

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // �÷��̾� ��ġ�� �߽����� �ϴ� ���� �׸�
        }
    }
}
