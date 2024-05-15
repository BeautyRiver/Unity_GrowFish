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
using System;


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
    public bool isBlowFishOn = false;
    public bool isSharkOn = false;

    //스테이지 레벨관리
    public int currentMission;
    public bool isGameEnd;
    public bool isGameOver = false;
    public bool isSpawnning = false;
    // 점수들
    public int score;

    public int level_1 = 60;
    public int level_2 = 500;
    public int level_3 = 2000;
    public int level_4 = 4500;

   
    public Dictionary<int, List<TargetFishInfo>> missionTargets = new Dictionary<int, List<TargetFishInfo>>(); // 미션별 목표 물고기 정보

    protected override void Awake()
    {
        base.Awake();
        DataManager dt = DataManager.Instance;
        dt.LoadData();
        // 테마 선택 여부에 따라 다른 씬으로 이동
        foreach (var data in dt.themeList.themes)
        {
            if(data.isSelect == true)
            {
                // 스킨 입히는 작업
                InitSkinSystem(data.themeName);
            }
        }
    }

    private void InitSkinSystem(string themeName)
    {
        // 스킨 입히는 작업
        // 스킨 이름에 따라 다른 스킨을 입히는 작업
        // 애니메이션, 배경이미지, 플레이어 이미지 등을 변경
        
    }

    private void Start()
    {
        InitMissionTargets(); // 목표 물고기 설정        
    }

    // 물고기 먹을 때마다 호출되는 함수
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
    void InitMissionTargets()
    {
        // 미션0
        missionTargets.Add(0, new List<TargetFishInfo> { new TargetFishInfo(0, 4) }); // 미션0은 0번 물고기 4마리 먹기

        // 미션1
        missionTargets.Add(1, new List<TargetFishInfo> { new TargetFishInfo(0, 5) }); // 미션1은 0번 물고기 5마리 먹기

        // 미션2
        missionTargets.Add(2, new List<TargetFishInfo> // 미션2는 0번 물고기 3마리, 1번 물고기 3마리 먹기
        {
            new TargetFishInfo(0, 3),
            new TargetFishInfo(1, 3)
        });

        // 미션3
        missionTargets.Add(3, new List<TargetFishInfo> // 미션3은 0번 물고기 3마리, 1번 물고기 5마리 먹기
        {
            new TargetFishInfo(0, 3),
            new TargetFishInfo(1, 5)
        });

        // 미션4
        missionTargets.Add(4, new List<TargetFishInfo> // 미션4는 0번 물고기 2마리, 1번 물고기 5마리, 2번 물고기 3마리 먹기
        {
            new TargetFishInfo(0, 2),
            new TargetFishInfo(1, 5),
            new TargetFishInfo(2, 3)
        });

        // 미션5
        missionTargets.Add(5, new List<TargetFishInfo> // 미션5는 1번 물고기 3마리, 2번 물고기 5마리 먹기
        {
            new TargetFishInfo(1, 3),
            new TargetFishInfo(2, 5)
        });

        // 미션6
        missionTargets.Add(6, new List<TargetFishInfo> // 미션6는 1번 물고기 3마리, 2번 물고기 3마리, 3번 물고기 2마리 먹기
        {
            new TargetFishInfo(1, 3),
            new TargetFishInfo(2, 3),
            new TargetFishInfo(3, 2)
        });

        // 미션7
        missionTargets.Add(7, new List<TargetFishInfo> // 미션7는 2번 물고기 5마리, 3번 물고기 3마리 먹기
        {
            new TargetFishInfo(2, 5),
            new TargetFishInfo(3, 3)
        });
        // 미션8
        missionTargets.Add(8, new List<TargetFishInfo> // 미션8는 2번 물고기 3마리, 3번 물고기 5마리 먹기
        {
            new TargetFishInfo(2, 3),
            new TargetFishInfo(3, 5)
        });
    }
    #endregion
   


    // 카메라 및 배경 사이즈 변경 코루틴
    IEnumerator ChangeCameraAndBgSize(float duration, float changeSize, float changeBGSizeY)
    {
        float currentTime = 0f; // 현재 보간 진행 시간
        float startSize = Camera.main.orthographicSize; // 시작 사이즈
        float endSize = startSize + changeSize; // 끝 사이즈

        // 배경의 시작 크기 및 변경될 크기를 미리 계산
        Vector3 bgStartSize = backGround.transform.localScale;
        Vector3 bgEndSize = new Vector3(bgStartSize.x, bgStartSize.y + changeBGSizeY, bgStartSize.z);

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

    // 스케일 변경 코루틴
    IEnumerator ChangeScaleCoroutine(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = playerMoveScript.transform.localScale;
        float time = 0;

        while (time < duration)
        {
            playerMoveScript.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        playerMoveScript.transform.localScale = targetScale;  // 최종 스케일 보장
    }

    //레벨업 
    public void LevelUp()
    {
        if (currentMission < 9)
        {
            // 카메라 및 배경 사이즈 변경 (시간,카메라 변경 사이즈, 배경 변경 사이즈)
            StartCoroutine(ChangeCameraAndBgSize(0.5f, 0.5f, 0.03f));
            currentMission += 1;
            uiManager.nowMissonText.text = "미션 : " + (currentMission + 1).ToString();
            // 미션 8을 넘어서면 게임 종료 상태 설정
            if (currentMission > 8)
            {
                isGameEnd = true;
                // 추가적인 게임 종료 처리를 여기에 작성할 수 있습니다.
            }

            Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale);
            if (playerMoveScript.transform.localScale.x < 0)
            {
                scaleChange.x = -scaleChange.x;
            }
            Vector3 targetScale = playerMoveScript.transform.localScale + scaleChange;
            StartCoroutine(ChangeScaleCoroutine(targetScale, 0.5f));

            // 이펙트 끄기
            levelEffectPrefab[0].SetActive(true);
            levelEffectPrefab[1].SetActive(true);
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
            uiManager.nowMissonText.text = "미션 : " + (currentMission + 1).ToString();

            Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale);
            if (playerMoveScript.transform.localScale.x < 0)
            {
                scaleChange.x = -scaleChange.x;
            }
            playerMoveScript.transform.localScale -= scaleChange;
        }
    }

}
