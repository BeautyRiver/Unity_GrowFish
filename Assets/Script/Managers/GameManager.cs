using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using DG.Tweening.Core.Easing;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using System.Diagnostics;

//GAME MANAGER SCRIPT
public class GameManager : MonoBehaviour
{
    private static GameManager instance; // 싱글톤

    [SerializeField] private UIManager uiManager; // UIManager
    [SerializeField] private PlayerMove playerMoveScript;
    [SerializeField] private PoolManager poolManager;

    // 이펙트
    [SerializeField] private GameObject[] levelEffectPrefab;
    [SerializeField] private float LevelUpScale;

    //적 관련 변수들
    public FishSpawnRange fishSpawnRange = new FishSpawnRange(1.0f, 2.0f);
    public EnemySpawnRange enemySpawnRange = new EnemySpawnRange(1.0f, 2.0f);
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private bool isBlowFishOn = false;
    [SerializeField] private bool isSharkOn = false;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    //스테이지 레벨관리
    [SerializeField] private int currentMission;
    public int CurrentMission
    {
        get { return currentMission; }
        set { currentMission = value; }
    }

    // 점수들
    [SerializeField] private int score;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }


    public int Level_1 { get; } = 60;
    public int Level_2 { get; } = 500;
    public int Level_3 { get; } = 2000;
    public int Level_4 { get; } = 4500;

    [System.Serializable] 
    public class FishSpawnRange
    {
        public float min;
        public float max;

        public FishSpawnRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [System.Serializable]
    public class EnemySpawnRange
    {
        public float min;
        public float max;

        public EnemySpawnRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
    [System.Serializable]
    public class TargetFishInfo
    {
        public int targetFish;
        public int targetFishCounts;

        public TargetFishInfo(int targetFish, int targetFishCounts)
        {
            this.targetFish = targetFish;
            this.targetFishCounts = targetFishCounts;
        }
    }

    public Dictionary<int, List<TargetFishInfo>> missionTargets = new Dictionary<int, List<TargetFishInfo>>();

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        DOTween.KillAll();
        StartCoroutine(SpawnFish());
        InitializeMissionTargets(); // 목표 물고기 설정        
    }

    public void UpdateFishCount(int fishType)
    {
        // 현재 미션의 목표 물고기 정보 확인
        List<TargetFishInfo> currentMissionFish = missionTargets[currentMission];

        // 먹은 물고기가 현재 미션의 목표에 포함되는지 확인
        foreach (var item in currentMissionFish)
        {
            if (item.targetFish == fishType)
            {
                item.targetFishCounts -= 1; // 목표 개수 감소               
                item.targetFishCounts = Mathf.Max(0, item.targetFishCounts);
                break;
            }
        }
        // 모든 목표가 완료되었는지 검사
        bool missonCompleted = true;
        foreach (var item2 in currentMissionFish)
        {
            // 하나라도 목표 개수가 남아있다면 아직 미션 완료 아님
            if (item2.targetFishCounts > 0)
            {
                missonCompleted = false;
                break;
            }
        }

        // 모든 목표를 완료했다면 다음 미션으로
        if (missonCompleted)
        {
            LevelUp();
            if(CurrentMission < 9)
                CurrentMission += 1;
        }
    }

    // 물고기 목표값 초기 할당
    void InitializeMissionTargets()
    {
        // 미션0
        missionTargets.Add(0, new List<TargetFishInfo> { new TargetFishInfo(0, 4) });

        // 미션1
        missionTargets.Add(1, new List<TargetFishInfo> { new TargetFishInfo(0, 5) });

        // 미션2
        missionTargets.Add(2, new List<TargetFishInfo>
        {
            new TargetFishInfo(0, 3),
            new TargetFishInfo(1, 3)
        });

        // 미션3
        missionTargets.Add(3, new List<TargetFishInfo>
        {
            new TargetFishInfo(0, 3),
            new TargetFishInfo(1, 5)
        });

        // 미션4
        missionTargets.Add(4, new List<TargetFishInfo>
        {
            new TargetFishInfo(0, 2),
            new TargetFishInfo(1, 5),
            new TargetFishInfo(2, 3)
        });

        // 미션5
        missionTargets.Add(5, new List<TargetFishInfo>
        {
            new TargetFishInfo(1, 3),
            new TargetFishInfo(2, 5)
        });

        // 미션6
        missionTargets.Add(6, new List<TargetFishInfo>
        {
            new TargetFishInfo(1, 3),
            new TargetFishInfo(2, 3),
            new TargetFishInfo(3, 2)
        });

        // 미션7
        missionTargets.Add(7, new List<TargetFishInfo>
        {
            new TargetFishInfo(2, 5),
            new TargetFishInfo(3, 3)
        });
        // 미션8
        missionTargets.Add(8, new List<TargetFishInfo>
        {
            new TargetFishInfo(2, 3),
            new TargetFishInfo(3, 5)
        });

    }

    //레벨업 파티클
    public void LevelUp()
    {
        Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale);
        if(playerMoveScript.transform.localScale.x < 0)
        {
            scaleChange.x = -scaleChange.x;
        }
        playerMoveScript.transform.localScale += scaleChange;

        // 이펙트 끄기
        levelEffectPrefab[0].SetActive(true);
        levelEffectPrefab[1].SetActive(true);      
    }

    //코루틴 함수
    private IEnumerator SpawnFish()
    {
        //게임이 종료될때까지계속반복
        while (!IsGameOver)
        {
            poolManager.Get(SelectFish(), false);
            //1초에서 5초사이 실수값으로 랜덤하게 등장
            yield return new WaitForSeconds(Random.Range(fishSpawnRange.min, fishSpawnRange.max));
        }
    }

    private IEnumerator SpawnBlowFish()
    {
        while (true)
        {
            
                poolManager.Get(0, true); // BlowFish 스폰
            
            yield return new WaitForSeconds(Random.Range(enemySpawnRange.min, enemySpawnRange.max));
        }
    }

    private IEnumerator SpawnShark()
    {
        while (true)
        {
            
                poolManager.Get(1, true); // Shark 스폰
            
            yield return new WaitForSeconds(Random.Range(enemySpawnRange.min, enemySpawnRange.max));
        }
    }

    // Misson에 따라 적을 선택하는 로직 구현
    int SelectFish()
    {
        if (currentMission >= 7)
        {
            if (!isSharkOn)
            {
                StopCoroutine(SpawnBlowFish());
                StartCoroutine(SpawnShark());
                isSharkOn = true;
                isBlowFishOn = false;
            }
            return RanInt(2, 4);
        }

        else if (currentMission >= 5)
        {
            return RanInt(1, 4);
        }

        else if (currentMission >= 4)
        {
            if (!isBlowFishOn)
            {
                StartCoroutine(SpawnBlowFish());
                isBlowFishOn = true;
            }
            return RanInt(0, 4);
        }
        else if (currentMission >= 2)
        {
            float randomNum = Random.Range(0, 10); // 0에서 9까지의 숫자 중 하나를 랜덤으로 선택
            if (randomNum < 3.5) // 35% 확률로 0 반환
            {
                return 0;
            }
            else if (randomNum < 7) // 35% 확률로 1 반환
            {
                return 1;
            }
            else // 나머지 확률로 2 반환
            {
                return 2;
            }
        }

        else if (currentMission >= 0)
        {
            int randomNum = Random.Range(0, 10); // 0에서 9까지의 숫자 중 하나를 랜덤으로 선택
            if (randomNum < 7) // 70% 확률로 0 반환
            {
                return 0;
            }
            else // 나머지 30% 확률로 1 반환
            {
                return 1;
            }
        }

        else
            return 0;
    }

    private int RanInt(int min, int max)
    {
        return Random.Range(min, max);
    }
}
