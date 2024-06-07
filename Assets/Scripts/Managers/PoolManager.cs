using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs; // 물고기 프리팹 관리 배열: 미리 설정된 물고기 프리팹들을 저장
    [SerializeField] private GameObject[] enemyPrefabs; // 상어, 복어 같은 적 물고기 배열
    public Transform[] spawnPoints; // 스폰 장소들: 물고기가 나타날 위치들을 정의
    [SerializeField] private List<GameObject>[] fishPools; // 물고기 풀 배열: 물고기 프리팹들을 풀링하여 저장
    [SerializeField] private List<GameObject>[] enemyPools; // 적 물고기 풀 배열 
    private bool isFishSpawnCor = false; // 물고기 스폰 코루틴 중복 실행 방지 변수
    private bool isBlowFishSpawnCor = false; // BlowFish 스폰 코루틴 중복 실행 방지 변수
    private bool isSharkSpawnCor = false; // Shark 스폰 코루틴 중복 실행 방지 변수
    public FishSpawnRange fishSpawnTimeRange = new FishSpawnRange(1.0f, 2.0f); // 물고기 스폰 범위
    [ArrayElementTitle("name")]
    public List<EnemySpawnRange> enemySpawnTimeRangeList; // 적 물고기 스폰 범위 리스트
    private GameManager gm; // 게임 매니저 참조
    private bool isEnemySpawnTimePlus;

    private void Awake()
    {
        gm = GameManager.Instance; // 게임 매니저 참조

        fishPools = new List<GameObject>[fishPrefabs.Length]; // 물고기 프리팹 배열의 길이만큼 물고기 풀 배열 생성
        enemyPools = new List<GameObject>[enemyPrefabs.Length]; // 적 물고기 프리팹 배열의 길이만큼 적 물고기 풀 배열 생성      

        // 물고기 풀 배열 초기화
        for (int i = 0; i < fishPools.Length; i++)
        {
            fishPools[i] = new List<GameObject>();
        }
        // 적 물고기 풀 배열 초기화
        for (int i = 0; i < enemyPools.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
        }
    }

    private void Start()
    {
        StartSpawnFish(); // 물고기 스폰
    }

    public void Get(int index, bool isEnemy)
    {
        GameObject select = null; // 선택될 게임 오브젝트를 저장할 변수

        // 해당 인덱스의 풀에서 활성화되지 않은 게임 오브젝트를 찾음
        foreach (GameObject item in fishPools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true); // 찾았다면 활성화시키고 루프를 종료
                break;
            }
        }

        // 풀에서 사용 가능한 오브젝트를 찾지 못한 경우, 새로운 오브젝트를 생성하고 풀에 추가
        if (select == null)
        {
            int ranSpawnPoint = Random.Range(0,spawnPoints.Length);
            if (isEnemy)
            {
                select = Instantiate(enemyPrefabs[index],spawnPoints[ranSpawnPoint].position, Quaternion.identity, transform);
                enemyPools[index].Add(select);
            }
            else
            {
                select = Instantiate(fishPrefabs[index], spawnPoints[ranSpawnPoint].position, Quaternion.identity, transform);
                fishPools[index].Add(select);
            }            
        }

        // 선택된 오브젝트를 무작위 스폰 포인트 위치로 이동
        select.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        //return select; // 선택된 게임 오브젝트를 반환
    }

    // 물고기 스폰 시작
    public void StartSpawnFish()
    {
        if (isFishSpawnCor == true) // 코루틴이 실행 중이라면 중지
            StopCoroutine(SpawnFish()); // 물고기 스폰 코루틴 중지

        StartCoroutine(SpawnFish()); // 물고기 스폰 코루틴 시작
    }
    // 물고기 스폰 코루틴
    private IEnumerator SpawnFish()
    {
        isFishSpawnCor = true; // 물고기 스폰 코루틴 중복 실행 방지 변수 true로 변경
        while (!gm.isGameOver) //게임이 종료될때까지계속반복
        {
            Get(SelectFish(), false);
            //1초에서 5초사이 실수값으로 랜덤하게 등장
            yield return new WaitForSeconds(Random.Range(fishSpawnTimeRange.min, fishSpawnTimeRange.max));
        }
        isFishSpawnCor = false; // 물고기 스폰 코루틴 중복 실행 방지 변수 false로 변경
    }

    // BlowFish 스폰 코루틴
    private IEnumerator SpawnBlowFish()
    {
        isBlowFishSpawnCor = true;
        while (!gm.isGameOver)
        {
            Get(0, true); // BlowFish 스폰            
            yield return new WaitForSeconds(Random.Range(enemySpawnTimeRangeList[0].min, enemySpawnTimeRangeList[0].max));
        }
        isBlowFishSpawnCor = false;
    }
    // Shark 스폰 코루틴
    private IEnumerator SpawnShark()
    {
        isSharkSpawnCor = true;
        while (!gm.isGameOver)
        {
            Get(1, true); // Shark 스폰

            yield return new WaitForSeconds(Random.Range(enemySpawnTimeRangeList[1].min, enemySpawnTimeRangeList[1].max));
        }
        isSharkSpawnCor = false;
    }

    // Misson에 따라 적을 선택하는 로직 구현
    private int SelectFish()
    {
        float randomNum = Random.Range(0, 100); // 0에서 99까지의 숫자 중 하나를 랜덤으로 선택

        if (gm.currentMission <= 1) // 미션 0, 1
        {
            if (randomNum < 70) return 0; // 70% 확률로 0번 물고기
            else return 1; // 30% 확률로 1번 물고기            
        }

        else if (gm.currentMission <= 3) // 미션 2, 3
        {
            if (randomNum < 40) return 0; // 40% 확률로 0번 물고기            
            else if (randomNum < 80) return 1; // 40% 확률로 1번 물고기
            else return 2; // 20% 확률로 2번 물고기            
        }

        else if (gm.currentMission <= 4) // 미션 4
        {
            if (isBlowFishSpawnCor == false)
                StartCoroutine(SpawnBlowFish()); // BlowFish 스폰 코루틴 시작

            if (randomNum < 30) return 0; // 30% 확률로 0번 물고기
            else if (randomNum < 60) return 1; // 30% 확률로 1번 물고기
            else if (randomNum < 80) return 2; // 20% 확률로 2번 물고기
            else return 3; // 20% 확률로 3번 물고기
        }

        else if (gm.currentMission <= 5) // 미션 5
        {
            if (randomNum < 30) return 1; // 30% 확률로 0번 물고기
            else if (randomNum < 70) return 2; // 40% 확률로 1번 물고기
            else return 3; // 30% 확률로 3번 물고기
        }

        else if (gm.currentMission <= 6) // 미션 6
        {
            if(isSharkSpawnCor == false)
                StartCoroutine(SpawnShark());// Shark 스폰 코루틴 시작
            if (randomNum < 35) return 1;  // 35% 확률로 1번 물고기
            else if (randomNum < 65) return 2; // 35% 확률로 2번 물고기
            else return 3; // 30% 확률로 3번 물고기
        }

        else if (gm.currentMission <= 7) // 미션 7
        {
            if (randomNum < 45) return 2; // 45% 확률로 2번 물고기     
            else return 3; // 55% 확률로 3번 물고기
        }

        else // 미션 8
        {
            if (isEnemySpawnTimePlus == false)
            {
                // 스폰 시간 대폭 증가 적들
                foreach (var item in enemySpawnTimeRangeList)
                {
                    item.min *= 0.5f;
                    item.max *= 0.5f;
                }
                isEnemySpawnTimePlus = true;
            }
            
            if (randomNum < 45) return 2; // 45% 확률로 2번 물고기     
            else return 3; // 55% 확률로 3번 물고기
            
        }
    }
}
