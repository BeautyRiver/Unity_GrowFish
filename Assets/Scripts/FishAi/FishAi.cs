using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using System;
using UnityEngine;
using Ran = UnityEngine.Random;
public class FishAi : MonoBehaviour
{
    public static Action DisableFish;
    public static Action<bool> maxDistanceChange;
    public int turnCount = 2; // ��Ÿ� ����� Turn�� Ƚ��    
    public float moveSpeed = 2f; // �⺻ �ӵ�
    public float minSpeed = 2f; // �ּ� �ӵ�
    public float maxSpeed = 4f; // �ִ� �ӵ�
    public float runAwaySpeed; // ����ĥ�� �ӵ�
    public float detectionRadius = 5f; // �÷��̾� ���� �Ÿ�
    public float maxDistance; // �÷��̾�� �־����� ��Ȱ��ȭ �� �Ÿ�
    public PlayerMove player; // �÷��̾�

    [SerializeField] protected Vector2 currentDirection; // �̵��� ����
    [SerializeField] public bool isRunningAway = false; // ���� ������ ����
    [SerializeField] protected float distanceToPlayer; // �÷��̾���� �Ÿ�
    [HideInInspector] public Animator anim;
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
    private Color originalColor;

    protected virtual void Awake()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerMove>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // �ʱ� ���� ����
        originalColor = spriteRenderer.color; // ���� ���� ����
        InvokeRepeating(nameof(SetRandomY), 1f, 2.5f); // 2.5�ʸ��� Y��  ����
    }

    protected virtual void Start()
    {
        anim.runtimeAnimatorController = SkinManager.Instance.currentFishAnimtor; // �׸��� �´� �ִϸ����� ����        
        anim.SetBool(gameObject.tag, true); // �ִϸ��̼� ����
    }
    protected virtual void OnEnable()
    {
        DisableFish += FadeOut; // ����� ��Ȱ��ȭ �̺�Ʈ ���
        maxDistanceChange += MaxDistanceChange; // �ִ� ���� �Ÿ� ���� �̺�Ʈ ���

        maxDistance += GameManager.Instance.currentMission * GameManager.Instance.fishMaxDistance; // �̼ǿ� ���� �ִ� �Ÿ� ����

        spriteRenderer.color = originalColor; // ���� ����
        anim.SetBool(gameObject.tag, true); // �ִϸ��̼� ����
        turnCount = 2; // �� ī��Ʈ �ʱ�ȭ
        RandomSpeed(minSpeed, maxSpeed); // �ӵ� �� Y ��ġ ���� ����
    }

    private void OnDisable()
    {
        DisableFish -= FadeOut; // ����� ��Ȱ��ȭ �̺�Ʈ ����
        maxDistanceChange -= MaxDistanceChange; // �ִ� ���� �Ÿ� ���� �̺�Ʈ ����
    }

    protected virtual void Update()
    {
        runAwaySpeed = moveSpeed * 1.5f; // ����ĥ�� �ӵ� ����


        FlipX(); // �̵� ���⿡ ���� ��������Ʈ ������
        DistanceCal(); // �÷��̾���� �Ÿ� ��� �Լ�
    }
    
    protected virtual void FixedUpdate()
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
    // �÷��̾���� �Ÿ� ��� �Լ�
    private void DistanceCal()
    {
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position); // �� �����Ӹ��� �÷��̾���� �Ÿ� ���

        // �Ÿ��� maxDistance���� ũ�� ����� ��ġ �ٽ� �ùٸ��� �����ϱ�(�ִ� turnCount ��ŭ) / ���͸� ��Ȱ��ȭ
        if (distanceToPlayer > maxDistance)
        {
            // ���͸� ��Ȱ��ȭ
            if (fishType == TypeOfFish.Enemy)
            {
                gameObject.SetActive(false);
            }
            // ������ U��            
            else
            {
                // �� ī��Ʈ�� �����ִ� ��� ��ġ ������Ʈ
                if (turnCount > 0)
                {
                    // ����� ��ġ ������Ʈ ����
                    float newPosX = CalculateNewPositionX(); // ���ο� X ��ġ ���
                    float newPosY = transform.position.y; // Y ��ġ ����

                    transform.position = new Vector2(newPosX, newPosY); // �� ��ġ ����
                    SetRandomY();
                    RandomSpeed(minSpeed, maxSpeed); // �ӵ� �� Y ��ġ ���� ����
                    turnCount--; // �� ī��Ʈ ����
                }
                else
                {
                    // �� ī��Ʈ�� ������ ��Ȱ��ȭ
                    gameObject.SetActive(false);
                }

            }
        }
    }

    // �ִ� ���� �Ÿ� ���� �Լ�
    private void MaxDistanceChange(bool boolean)
    {
        if (boolean == true)
            maxDistance += GameManager.Instance.fishMaxDistance;
        else
            maxDistance -= GameManager.Instance.fishMaxDistance;
    }
    // ��������Ʈ ������ �Լ�
    private void FlipX()
    {
        Vector3 localScale = transform.localScale; // �̵� ���⿡ ���� ��������Ʈ ������
        // ���������� �̵��ϴ� ���, localScale.x�� ������ ����
        if (currentDirection.x > 0)
        {
            localScale.x = -Mathf.Abs(localScale.x);
        }
        // �������� �̵��ϴ� ���, localScale.x�� ����� ����
        else if (currentDirection.x < 0)
        {
            localScale.x = Mathf.Abs(localScale.x);
        }

        transform.localScale = localScale; // ��������Ʈ ������ ����
    }


    // ���̵� �ƿ� �Լ�
    void FadeOut()
    {
        // ��������Ʈ�� ������ ���� 0���� ����
        spriteRenderer.DOFade(0, 1.0f).OnComplete(() =>
        {
            gameObject.SetActive(false); // ���̵� �ƿ� �� ������Ʈ ��Ȱ��ȭ
        });
    }
    // ���ο� X ��ġ�� ����ϴ� �Լ�
    float CalculateNewPositionX()
    {
        float newPosX = player.transform.position.x; // �ʱ� ��ġ ����
        float range = Ran.Range(maxDistance - 11.5f, maxDistance - 1.5f);

        // �÷��̾�� ������� ����� ��ġ �� ���⿡ ���� X ��ġ ���� 
        // �÷��̾� ���� �������� ����� ����, ������ ������
        if (player.transform.localScale.x > 0 && currentDirection.x < 0) // �÷��̾ ������ �ٶ󺸰� ����⵵ �������� �̵�
        {
            SetReverseX(); // X�� ����
            return player.transform.position.x - range; // �÷��̾� ��ġ���� range ��ŭ �������� �̵�
        }
        else if (player.transform.localScale.x < 0 && currentDirection.x > 0) // �÷��̾ �������� �ٶ󺸰� ����⵵ ���������� �̵�
        {
            SetReverseX(); // X�� ����
            return player.transform.position.x + range; // �÷��̾� ��ġ���� range ��ŭ ���������� �̵�
        }
        else if (player.transform.localScale.x > 0 && currentDirection.x > 0) // �÷��̾ ������ �ٶ󺸰� ������ ���������� �̵�
        {
            return player.transform.position.x - range; // �÷��̾� ��ġ���� range ��ŭ �������� �̵�
        }
        else if (player.transform.localScale.x < 0 && currentDirection.x < 0) // �÷��̾ �������� �ٶ󺸰� ������ �������� �̵�
        {
            return player.transform.position.x + range; // �÷��̾� ��ġ���� range ��ŭ ���������� �̵�
        }
        else
        {
            // ���� ��Ȳ ó��
            gameObject.SetActive(false);
            return newPosX; // �ʱ� ��ġ ��ȯ
        }
    }

    // ������ �ӵ� �Լ�
    protected void RandomSpeed(float minSpeed, float maxSpeed)
    {
        moveSpeed = Ran.Range(minSpeed, maxSpeed); // ���� �ӵ� ����
    }
    // X�� ����
    protected void SetReverseX()
    {
        currentDirection = new Vector2(-currentDirection.x, Ran.Range(-0.2f, 0.2f)).normalized;
    }
    // Y�� ���� ����
    protected void SetRandomY()
    {
        currentDirection = new Vector2(currentDirection.x, Ran.Range(-0.2f, 0.2f)).normalized;
    }

    // ���� ���� ����
    protected void SetRandomDir()
    {
        SetRandomY();
        SetReverseX();
    }
    // ������ �ʱ� ���� ����
    private void SetNewDirection()
    {
        currentDirection = new Vector2(Ran.Range(-1f, 1f), Ran.Range(-1f, 1f) * 0.1f).normalized;
    }

    // �÷��̾�� ���� �������� �̵� �˻� (��� = �÷��̾� ���� �̵�)
    protected bool IsMovingTowardsPlayer()
    {
        Vector2 toPlayer = (player.transform.position - transform.position).normalized;
        float dotProduct = Vector2.Dot(toPlayer, currentDirection);

        // �÷��̾ �������� �ʴ� ���, ���� ���� üũ
        if (player.GetComponent<Rigidbody2D>().velocity.magnitude == 0)
        {
            return dotProduct > 0 && currentDirection.x == Mathf.Sign(player.transform.position.x - transform.position.x);
        }
        return dotProduct > 0;
    }

    // ����� ���� ���� �׸���
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
