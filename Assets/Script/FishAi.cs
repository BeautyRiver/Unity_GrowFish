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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetNewDirection(); // �ʱ� ���� ����
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer < detectionRadius && !isRunningAway)
        {
            // �÷��̾� ���� �� �ݴ� �������� ����
            spriteRenderer.flipX = currentDirection.x < 0; // �̵� ���⿡ ���� ��������Ʈ ������
            isRunningAway = true;
            currentDirection = new Vector2(-currentDirection.x,currentDirection.y); // ���� ������ �ݴ�� ����
            Invoke(nameof(ChangeDirAfterRunning), 3.5f); // 2�� �� ���� ����
        }
    }

    void FixedUpdate()
    {        
        rb.MovePosition(rb.position + currentDirection * (isRunningAway ? runAwaySpeed : moveSpeed) * Time.fixedDeltaTime);
    }

    void SetNewDirection()
    {
        // ������ �ʱ� ���� ����
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f)).normalized;
    }
    
    void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // ���� ���� ���� �� ���� �ݴ�� �ٲٱ�
            isRunningAway = false;
            currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.2f, 0.2f)).normalized; 
        }
    }
}
