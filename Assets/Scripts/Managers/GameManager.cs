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
using UnityEngine.UIElements;

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
//GAME MANAGER SCRIPT
public class GameManager : Singleton<GameManager>
{

    [SerializeField] private UIManager uiManager; // UIManager
    [SerializeField] private PlayerMove playerMoveScript;
    [SerializeField] private PoolManager poolManager;

    // 이펙트
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject[] levelEffectPrefab;
    [SerializeField] private float LevelUpScale;

    //적 관련 변수들
    public FishSpawnRange fishSpawnRange = new FishSpawnRange(1.0f, 2.0f);
    public EnemySpawnRange enemySpawnRange = new EnemySpawnRange(1.0f, 2.0f);
    public bool isGameOver = false;
    public bool isBlowFishOn = false;
    public bool isSharkOn = false;
   
    //스테이지 레벨관리
    public int currentMission;
   
    // 점수들
    public int score;
 
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

    

    public Dictionary<int, List<TargetFishInfo>> missionTargets = new Dictionary<int, List<TargetFishInfo>>();

    protected override void Awake()
    {
        DoDestoryObj = false;
        base.Awake();
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

        }
    }

    #region 물고기 목표값 초기 할당
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
    #endregion

    //코루틴 함수
    private IEnumerator SpawnFish()
    {
        //게임이 종료될때까지계속반복
        while (!isGameOver)
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
            return Random.Range(2, 4);
        }

        else if (currentMission >= 5)
        {
            return Random.Range(1, 4);
        }

        else if (currentMission >= 4)
        {
            if (!isBlowFishOn)
            {
                StartCoroutine(SpawnBlowFish());
                isBlowFishOn = true;
            }
            return Random.Range(0, 4);
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


    // 현재 카메라의 사이즈에서 0.2f 만큼 증가시키되, 이 변화를 1초에 걸쳐서 부드럽게 적용하려면
    IEnumerator ChangeCameraAndBgSize(float duration, float changeSize, float changeBGSize)
    {
        float currentTime = 0f; // 현재 보간 진행 시간
        float startSize = Camera.main.orthographicSize; // 시작 사이즈
        float endSize = startSize + changeSize; // 끝 사이즈

        // 배경의 시작 크기 및 변경될 크기를 미리 계산
        Vector3 bgStartSize = backGround.transform.localScale;
        Vector3 bgEndSize = new Vector3(bgStartSize.x, bgStartSize.y + changeBGSize, bgStartSize.z);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime; // 시간 갱신
            Camera.main.orthographicSize = Mathf.Lerp(startSize, endSize, currentTime / duration);

            // 배경의 localScale을 부드럽게 변경
            backGround.transform.localScale = Vector3.Lerp(bgStartSize, bgEndSize, currentTime / duration);
            yield return null; // 다음 프레임까지 대기
        }

        Camera.main.orthographicSize = endSize; // 최종 사이즈로 확실하게 설정
        backGround.transform.localScale = bgEndSize; // 배경 크기도 최종값으로 설정

    }

    //레벨업 
    public void LevelUp()
    {
        if (currentMission < 9)
        {
            // 카메라 및 배경 사이즈 변경 (시간,카메라 변경 사이즈, 배경 변경 사이즈)
            StartCoroutine(ChangeCameraAndBgSize(0.5f, 0.5f, 0.03f));
            currentMission += 1;

            Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale);
            if (playerMoveScript.transform.localScale.x < 0)
            {
                scaleChange.x = -scaleChange.x;
            }
            playerMoveScript.transform.localScale += scaleChange;

            // 이펙트 끄기
            levelEffectPrefab[0].SetActive(true);
            levelEffectPrefab[1].SetActive(true);

            if(currentMission > 9)
            {
                Time.timeScale = 0;
                print("게임종료");
            }
        }        
    }

    // 디버그용 스테이지 다운
    public void LevelDown()
    {
        if (currentMission > 0)
        {
            // 카메라 및 배경 사이즈 변경 (시간,카메라 변경 사이즈, 배경 변경 사이즈)
            StartCoroutine(ChangeCameraAndBgSize(0.5f, -0.5f, -0.03f));
            currentMission -= 1;

            Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale);
            if (playerMoveScript.transform.localScale.x < 0)
            {
                scaleChange.x = -scaleChange.x;
            }
            playerMoveScript.transform.localScale -= scaleChange;
        }
    }

}
