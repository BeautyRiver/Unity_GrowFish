using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs; // ����� ������ ���� �迭: �̸� ������ ����� �����յ��� ����
    [SerializeField] private Transform[] spawnPoints; // ���� ��ҵ�: ����Ⱑ ��Ÿ�� ��ġ���� ����
    [SerializeField] private List<GameObject>[] fishPools; // �� ����� Ÿ�Ժ��� ���� ������ ���� ������Ʈ���� ������ ����Ʈ�� �迭

    private void Awake()
    {
        // fishPrefabs �迭�� ���̿� ���� fishPools �迭�� �ʱ�ȭ
        fishPools = new List<GameObject>[fishPrefabs.Length];

        // fishPools�� �� ���(����Ʈ)�� �ʱ�ȭ
        for (int i = 0; i < fishPools.Length; i++)
        {
            fishPools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null; // ���õ� ���� ������Ʈ�� ������ ����

        // �ش� �ε����� Ǯ���� Ȱ��ȭ���� ���� ���� ������Ʈ�� ã��
        foreach (GameObject item in fishPools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true); // ã�Ҵٸ� Ȱ��ȭ��Ű�� ������ ����
                break;
            }
        }

        // Ǯ���� ��� ������ ������Ʈ�� ã�� ���� ���, ���ο� ������Ʈ�� �����ϰ� Ǯ�� �߰�
        if (select == null)
        {
            select = Instantiate(fishPrefabs[index], transform); // �������� �ν��Ͻ�ȭ�Ͽ� ����
            fishPools[index].Add(select); // ������ ������Ʈ�� �ش� Ǯ�� �߰�
        }

        // ���õ� ������Ʈ�� ������ ���� ����Ʈ ��ġ�� �̵�
        select.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        return select; // ���õ� ���� ������Ʈ�� ��ȯ
    }

}
