using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runAwaySpeed = 5f;
    public float detectionRadius = 5f; // �÷��̾� ���� ����
    public Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private bool isRunningAway = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ChangeDirection", 0f, 2f); // ���� ���� �� ��� ������ ���� ����, �� �� 2�ʸ��� ���� ����
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer < detectionRadius && !isRunningAway)
        {
            // �÷��̾ �����ϸ� �ݴ� �������� ����
            Vector2 runDirection = (transform.position - playerTransform.position).normalized;
            movementDirection = runDirection;
            isRunningAway = true;
            CancelInvoke("ChangeDirection"); // ����ġ�� ������ ������ �̵� ����
            Invoke("EnableRandomMovement", 2f); // 2�� �Ŀ� �ٽ� ������ �̵� ����
        }
    }

    void FixedUpdate()
    {
        // ���� �������� �̵��� ���� ���� ��ġ ���
        Vector2 newPosition = rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime;

        // ���� ��ġ�� ������ ������ ����� �ʵ��� ����
        newPosition.x = Mathf.Clamp(newPosition.x, -16f, 16f);
        newPosition.y = Mathf.Clamp(newPosition.y, -10f, 10f);

        // ������ ��ġ�� ����� �̵�
        rb.MovePosition(newPosition);
    }

    void ChangeDirection()
    {
        // ������ �������� �̵� ����
        movementDirection = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
        isRunningAway = false; // ���� ���� ����
    }

    void EnableRandomMovement()
    {
        // �ٽ� ������ �̵� ����
        isRunningAway = false;
        InvokeRepeating("ChangeDirection", 0f, 2f);
    }
}
