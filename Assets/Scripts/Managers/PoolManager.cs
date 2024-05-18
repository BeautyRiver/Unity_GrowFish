using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs; // ����� ������ ���� �迭: �̸� ������ ����� �����յ��� ����
    [SerializeField] private GameObject[] enemyPrefabs; // ���, ���� ���� �� ����� �迭
    public Transform[] spawnPoints; // ���� ��ҵ�: ����Ⱑ ��Ÿ�� ��ġ���� ����
    [SerializeField] private List<GameObject>[] fishPools; // ����� Ǯ �迭: ����� �����յ��� Ǯ���Ͽ� ����
    [SerializeField] private List<GameObject>[] enemyPools; // �� ����� Ǯ �迭 
    private bool isFishSpawnCor = false; // ����� ���� �ڷ�ƾ �ߺ� ���� ���� ����
    private bool isBlowFishSpawnCor = false; // BlowFish ���� �ڷ�ƾ �ߺ� ���� ���� ����
    private bool isSharkSpawnCor = false; // Shark ���� �ڷ�ƾ �ߺ� ���� ���� ����
    public FishSpawnRange fishSpawnRange = new FishSpawnRange(1.0f, 2.0f); // ����� ���� ����
    [ArrayElementTitle("name")]
    public List<EnemySpawnRange> enemySpawnRangeList; // �� ����� ���� ���� ����Ʈ
    private GameManager gm; // ���� �Ŵ��� ����

    private void Awake()
    {
        gm = GameManager.Instance; // ���� �Ŵ��� ����

        fishPools = new List<GameObject>[fishPrefabs.Length]; // ����� ������ �迭�� ���̸�ŭ ����� Ǯ �迭 ����
        enemyPools = new List<GameObject>[enemyPrefabs.Length]; // �� ����� ������ �迭�� ���̸�ŭ �� ����� Ǯ �迭 ����      

        // ����� Ǯ �迭 �ʱ�ȭ
        for (int i = 0; i < fishPools.Length; i++)
        {
            fishPools[i] = new List<GameObject>();
        }
        // �� ����� Ǯ �迭 �ʱ�ȭ
        for (int i = 0; i < enemyPools.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
        }
    }

    private void Start()
    {
        StartSpawnFish(); // ����� ����
    }

    public void Get(int index, bool isEnemy)
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
            if (isEnemy)
            {
                select = Instantiate(enemyPrefabs[index], transform);
                enemyPools[index].Add(select);
            }
            else
            {
                select = Instantiate(fishPrefabs[index], transform);
                fishPools[index].Add(select);
            }
        }

        // ���õ� ������Ʈ�� ������ ���� ����Ʈ ��ġ�� �̵�
        select.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        //return select; // ���õ� ���� ������Ʈ�� ��ȯ
    }

    // ����� ���� ����
    public void StartSpawnFish()
    {
        if (isFishSpawnCor == true) // �ڷ�ƾ�� ���� ���̶�� ����
            StopCoroutine(SpawnFish()); // ����� ���� �ڷ�ƾ ����

        StartCoroutine(SpawnFish()); // ����� ���� �ڷ�ƾ ����
    }
    // ����� ���� �ڷ�ƾ
    private IEnumerator SpawnFish()
    {
        isFishSpawnCor = true; // ����� ���� �ڷ�ƾ �ߺ� ���� ���� ���� true�� ����
        while (!gm.isGameOver) //������ ����ɶ�������ӹݺ�
        {
            Get(SelectFish(), false);
            //1�ʿ��� 5�ʻ��� �Ǽ������� �����ϰ� ����
            yield return new WaitForSeconds(Random.Range(fishSpawnRange.min, fishSpawnRange.max));
        }
        isFishSpawnCor = false; // ����� ���� �ڷ�ƾ �ߺ� ���� ���� ���� false�� ����
    }

    // BlowFish ���� �ڷ�ƾ
    private IEnumerator SpawnBlowFish()
    {
        isBlowFishSpawnCor = true;
        while (!gm.isGameOver)
        {
            Get(0, true); // BlowFish ����            
            yield return new WaitForSeconds(Random.Range(enemySpawnRangeList[0].min, enemySpawnRangeList[0].max));
        }
        isBlowFishSpawnCor = false;
    }
    // Shark ���� �ڷ�ƾ
    private IEnumerator SpawnShark()
    {
        isSharkSpawnCor = true;
        while (!gm.isGameOver)
        {
            Get(1, true); // Shark ����

            yield return new WaitForSeconds(Random.Range(enemySpawnRangeList[1].min, enemySpawnRangeList[1].max));
        }
        isSharkSpawnCor = false;
    }

    // Misson�� ���� ���� �����ϴ� ���� ����
    private int SelectFish()
    {
        float randomNum = Random.Range(0, 100); // 0���� 99������ ���� �� �ϳ��� �������� ����

        if (gm.currentMission <= 1) // �̼� 0, 1
        {
            if (randomNum < 70) return 0; // 70% Ȯ���� 0�� �����
            else return 1; // 30% Ȯ���� 1�� �����            
        }
        else if (gm.currentMission <= 3) // �̼� 2, 3
        {
            if (randomNum < 45) return 0; // 45% Ȯ���� 0�� �����            
            else if (randomNum < 75) return 1; // 30% Ȯ���� 1�� �����
            else return 2; // 25% Ȯ���� 2�� �����            
        }
        else if (gm.currentMission <= 5) // �̼� 4, 5
        {
            if (isBlowFishSpawnCor == false)
                StartCoroutine(SpawnBlowFish()); // BlowFish ���� �ڷ�ƾ ����

            if (randomNum < 30) return 0; // 30% Ȯ���� 0�� �����
            else if (randomNum < 70) return 1; // 40% Ȯ���� 1�� �����
            else if (randomNum < 90) return 2; // 20% Ȯ���� 2�� �����
            else return 3; // 10% Ȯ���� 3�� �����
        }
        else // �̼� 6,7,8
        {
            if (gm.currentMission <= 7 && isSharkSpawnCor == false)
                StartCoroutine(SpawnShark()); // Shark ���� �ڷ�ƾ ����

            if (randomNum < 10) return 0; // 10% Ȯ���� 0�� �����
            else if (randomNum < 40) return 1; // 30% Ȯ���� 1�� �����
            else if (randomNum < 70) return 2; // 30% Ȯ���� 2�� �����
            else return 3; // 30% Ȯ���� 3�� �����
        }
    }
}
