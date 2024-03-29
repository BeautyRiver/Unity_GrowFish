using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs; // 물고기 프리팹 관리 배열: 미리 설정된 물고기 프리팹들을 저장
    [SerializeField] private Transform[] spawnPoints; // 스폰 장소들: 물고기가 나타날 위치들을 정의
    [SerializeField] private List<GameObject>[] fishPools; // 각 물고기 타입별로 재사용 가능한 게임 오브젝트들을 저장할 리스트의 배열

    private void Awake()
    {
        // fishPrefabs 배열의 길이에 따라 fishPools 배열을 초기화
        fishPools = new List<GameObject>[fishPrefabs.Length];

        // fishPools의 각 요소(리스트)를 초기화
        for (int i = 0; i < fishPools.Length; i++)
        {
            fishPools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
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
            select = Instantiate(fishPrefabs[index], transform); // 프리팹을 인스턴스화하여 생성
            fishPools[index].Add(select); // 생성된 오브젝트를 해당 풀에 추가
        }

        // 선택된 오브젝트를 무작위 스폰 포인트 위치로 이동
        select.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        return select; // 선택된 게임 오브젝트를 반환
    }

}
