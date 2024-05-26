using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public float fishMaxDistance = 4f; // 물고기 최대 거리 증가치
    public float changeCameraSize = 0.5f; // 카메라 변경 사이즈
    public float changeBGSizeY = 0.03f; // 배경 변경 사이즈
    public float changeSpawnSizeX; // 스포너 위치 변경

    public Dictionary<int, List<TargetFishInfo>> missionTargets = new Dictionary<int, List<TargetFishInfo>>(); // 미션별 목표 물고기 정보

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        InitMissionTargets(); // 목표 물고기 설정     
        SoundManager.Instance.ChangePlayListClip("InGame_bgm");   
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
        missionTargets.Clear(); // 미션 목표값 초기화
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

#region  사이즈 관련
    // 카메라 및 배경 사이즈 변경 코루틴
    IEnumerator ChangeCameraAndBgSize(float duration, float camerChangeSize, float changeBGSizeY)
    {
        float currentTime = 0f; // 현재 시간
        float cameraStartSize = Camera.main.orthographicSize; // 시작 사이즈
        float cameraEndSize = cameraStartSize + camerChangeSize; // 끝 사이즈

        // 배경의 시작 크기 및 변경될 크기를 미리 계산
        Vector3 bgStartSize = backGround.transform.localScale; // 배경의 시작 크기
        Vector3 bgEndSize = new Vector3(bgStartSize.x, bgStartSize.y + changeBGSizeY, bgStartSize.z); // 배경의 Y 크기만 변경

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime; // 시간 갱신
            Camera.main.orthographicSize = Mathf.Lerp(cameraStartSize, cameraEndSize, currentTime / duration); // 카메라의 orthographicSize를 부드럽게 변경

            backGround.transform.localScale = Vector3.Lerp(bgStartSize, bgEndSize, currentTime / duration); // 배경 크기도 부드럽게 변경
            yield return null; // 다음 프레임까지 대기
        }

        Camera.main.orthographicSize = cameraEndSize; // 최종 사이즈로 확실하게 설정
        backGround.transform.localScale = bgEndSize; // 배경 크기도 최종값으로 설정
    }

    // 플레이어 스케일 변경 코루틴
    IEnumerator ChangePlayerSize(Vector3 targetScale, float duration)
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

    // 스포너 위치 변경
    private void ChangeSpawnerPos(float cgSpawnSizeX)
    {
        foreach (var item in poolManager.spawnPoints)
        {
            if (playerMoveScript.transform.position.x > item.position.x)
            {
                item.position = new Vector3(item.position.x - cgSpawnSizeX, item.position.y, item.position.z);
            }
            else
            {
                item.position = new Vector3(item.position.x + cgSpawnSizeX, item.position.y, item.position.z);
            }


        }
    }
#endregion

#region 레벨업 + Debug
    //레벨업 
    public void LevelUp()
    {
        if (currentMission < 9)
        {
            // 카메라 및 배경 사이즈 변경 (시간,카메라 변경 사이즈, 배경 변경 사이즈)
            StartCoroutine(ChangeCameraAndBgSize(0.5f, changeCameraSize, changeBGSizeY));

            // 스포너 위치 변경 (플레이어 왼쪽에 있는 스포너들은 -쪽으로, 오른쪽에 있는 스포너들은 +쪽으로 이동)

            currentMission += 1;
            // 미션 8을 넘어서면 게임 종료 상태 설정
            if (currentMission > 8)
            {
                isGameEnd = true;
                SoundManager.Instance.PlaySound("ClearSound"); // 클리어 사운드 재생    
                // 추가적인 게임 종료 처리를 여기에 작성할 수 있습니다.
            }
            SoundManager.Instance.PlaySound("LevelUpSound"); // 레벨업 사운드 재생
            uiManager.nowMissonText.text = "미션 : " + (currentMission + 1).ToString(); // 미션 텍스트 업데이트

            Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale); // 스케일 변경량
            if (playerMoveScript.transform.localScale.x < 0)
            {
                scaleChange.x = -scaleChange.x;
            }
            Vector3 targetScale = playerMoveScript.transform.localScale + scaleChange; // 목표 스케일
            StartCoroutine(ChangePlayerSize(targetScale, 0.5f)); // 플레이어 스케일 변경
            ChangeSpawnerPos(changeSpawnSizeX); // 스포너 위치 변경

            // 이펙트 끄기
            levelEffectPrefab[0].SetActive(true);
            levelEffectPrefab[1].SetActive(true);

            FishAi.maxDistanceChange?.Invoke(true); // 최대 감지 거리 변경 이벤트 발생

        }
    }

    // 디버그용 스테이지 다운
    public void LevelDown()
    {
        if (currentMission > 0)
        {
            // 카메라 및 배경 사이즈 변경 (시간,카메라 변경 사이즈, 배경 변경 사이즈)
            StartCoroutine(ChangeCameraAndBgSize(0.5f, -changeCameraSize, -changeBGSizeY));
            currentMission -= 1;
            uiManager.nowMissonText.text = "미션 : " + (currentMission + 1).ToString();

            Vector3 scaleChange = new Vector3(LevelUpScale, LevelUpScale, LevelUpScale); // 스케일 변경량
            if (playerMoveScript.transform.localScale.x < 0)
            {
                scaleChange.x = -scaleChange.x;
            }
            Vector3 targetScale = playerMoveScript.transform.localScale - scaleChange; // 목표 스케일
            StartCoroutine(ChangePlayerSize(targetScale, 0.5f));
            ChangeSpawnerPos(-changeSpawnSizeX); // 스포너 위치 변경
            FishAi.maxDistanceChange?.Invoke(false); // 최대 감지 거리 변경 이벤트 발생

        }
    }

    // 무적 버튼 
    public void InvincibleBtn()
    {
        playerMoveScript.gameObject.layer = LayerMask.NameToLayer("PlayerDamaged");
        playerMoveScript.hp = 99999999999999999;
    }

    // 무적 해제 버튼
    public void DeathBtn()
    {
        playerMoveScript.hp = 0;
    }

    // 디폴트 스킨 적용 토글
    public void ToggleClick_Default(bool boolean)
    {
        string skinName = "DefaultTheme";
        if (boolean == true)
        {
            Debug.Log("DefaultTheme");
            DataManager.Instance.UnLockTheme(skinName);
            DataManager.Instance.SelectTheme(skinName);
            SkinManager.Instance.SetSkin(0);
            FishAi[] fishAis = FindObjectsOfType<FishAi>();
            foreach (FishAi fishAi in fishAis)
            {
                fishAi.anim.runtimeAnimatorController = SkinManager.Instance.currentFishAnimtor;
                fishAi.anim.SetBool(fishAi.gameObject.tag, true); // 애니메이션 실행

            }
        }
    }

    // 페이퍼 스킨 적용 토글
    public void ToggleClick_Paper(bool boolean)
    {
        string skinName = "PaperTheme";
        if (boolean == true)
        {
            Debug.Log("PaperTheme");
            DataManager.Instance.UnLockTheme(skinName);
            DataManager.Instance.SelectTheme(skinName);
            SkinManager.Instance.SetSkin(1);
            FishAi[] fishAis = FindObjectsOfType<FishAi>();
            foreach (FishAi fishAi in fishAis)
            {
                fishAi.anim.runtimeAnimatorController = SkinManager.Instance.currentFishAnimtor;
                fishAi.anim.SetBool(fishAi.gameObject.tag, true); // 애니메이션 실행

            }
        }
    }

    // 할로윈 스킨 적용 토글
    public void ToggleClick_Halloween(bool boolean)
    {
        string skinName = "HalloweenTheme";
        if (boolean == true)
        {
            Debug.Log("HalloweenTheme");
            DataManager.Instance.UnLockTheme(skinName);
            DataManager.Instance.SelectTheme(skinName);
            SkinManager.Instance.SetSkin(2);
            FishAi[] fishAis = FindObjectsOfType<FishAi>();
            foreach (FishAi fishAi in fishAis)
            {
                fishAi.anim.runtimeAnimatorController = SkinManager.Instance.currentFishAnimtor;
                fishAi.anim.SetBool(fishAi.gameObject.tag, true); // 애니메이션 실행

            }
        }
    }
    #endregion
}