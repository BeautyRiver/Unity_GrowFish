using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ
    public GameObject background1; // ��� 1
    public GameObject background2; // ��� 2

    private float backgroundWidth; // ����� �ʺ�
    private float lastPlayerX; // ���� �����ӿ��� �÷��̾��� x ��ġ

    void Start()
    {
        // ����� �ʺ� ����մϴ� (����: ��� ��������Ʈ�� �ʺ� ���)
        backgroundWidth = background1.GetComponent<SpriteRenderer>().bounds.size.x;
        // �÷��̾��� �ʱ� x ��ġ�� ����
        lastPlayerX = player.position.x;
    }

    void Update()
    {
        // �÷��̾��� �̵� ���� �Ǵ� (������ �Ǵ� ���� �̵�)
        bool movingRight = (player.position.x > lastPlayerX);

        // �÷��̾ ���������� �̵��ϸ� ��� 1�� �߰��� �������� ���
        if (movingRight && player.position.x >= background1.transform.position.x)
        {
            // ��� 2�� ��� 1�� ���������� �̵�
            background2.transform.position = new Vector3(background1.transform.position.x + backgroundWidth, background1.transform.position.y, background1.transform.position.z);
        }
        // �÷��̾ �������� �̵��ϸ� ��� 1�� �߰��� �������� ���
        else if (!movingRight && player.position.x <= background1.transform.position.x)
        {
            // ��� 2�� ��� 1�� �������� �̵�
            background2.transform.position = new Vector3(background1.transform.position.x - backgroundWidth, background1.transform.position.y, background1.transform.position.z);
        }

        // �÷��̾ ���������� �̵��ϸ� ��� 1�� �߰��� �������� ���
        if (movingRight && player.position.x >= background2.transform.position.x)
        {
            // ��� 2�� ��� 1�� ���������� �̵�
            background1.transform.position = new Vector3(background2.transform.position.x + backgroundWidth, background2.transform.position.y, background2.transform.position.z);
        }
        // �÷��̾ �������� �̵��ϸ� ��� 1�� �߰��� �������� ���
        else if (!movingRight && player.position.x <= background2.transform.position.x)
        {
            // ��� 2�� ��� 1�� �������� �̵�
            background1.transform.position = new Vector3(background2.transform.position.x - backgroundWidth, background2.transform.position.y, background2.transform.position.z);
        }

        // �̹� �����ӿ����� �÷��̾� ��ġ�� ���� ������ �񱳸� ���� ����
        lastPlayerX = player.position.x;
    }
}
