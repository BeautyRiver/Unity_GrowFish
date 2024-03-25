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
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������ �߰�


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // �ʱ� ���� ����
    }

    private void Update()
    {
        // �� �����Ӹ��� �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

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
    }

    // ������ �ʱ� ���� ����
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f)).normalized;
        spriteRenderer.flipX = currentDirection.x > 0; // �̵� ���⿡ ���� ��������Ʈ ������
    }

    // �÷��̾� ���� �� ���� ó��: ���� ������ x ���� �����Ͽ� �ݴ� �������� ����
    private void ChangeDirRunningStart()
    {
        isRunningAway = true;
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)); // ���� ������ �ݴ�� ����
        spriteRenderer.flipX = currentDirection.x > 0; // �̵� ���⿡ ���� ��������Ʈ ������
    }

    // ���� ���� ���� �� ���� ����: x�� ���� ����, y���� ���ο� ������ ������ ����
    private void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // ���� ���� ���� �� ���� �ݴ�� �ٲٱ�
            isRunningAway = false;
            currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)).normalized;
            spriteRenderer.flipX = currentDirection.x > 0; // �̵� ���⿡ ���� ��������Ʈ ������
        }
    }
}
