using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using DG.Tweening.Core.Easing;
using UnityEngine.SocialPlatforms.Impl;

//GAME MANAGER SCRIPT
public class GameManager : MonoBehaviour
{
    private static GameManager instance; // 싱글톤

    [SerializeField] private GameObject[] levelEffectPrefab; // 이펙트
    [SerializeField] private UIManager uiManager; // UIManager
    [SerializeField] private PlayerMove playerMoveScript;
    [SerializeField] private PoolManager poolManager;

    public TargetFishClass[] targetFish = new TargetFishClass[3];

    public class TargetFishClass
    {
        public int TargetFishCounts { get; set; }
        public int TargetFish { get; set; }
    }

    //적 관련 변수들    
    [SerializeField] private bool isGameOver = false;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    //스테이지 레벨관리
    [SerializeField] public bool[] levels;
    public bool[] Levels
    {
        get { return levels; }
        set { levels = value; }
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
        StartCoroutine(SpawnEnemy());

        for (int i = 0; i < 3; i++)
        {
            targetFish[i] = new TargetFishClass();
        }
    }

    private void Update()
    {
        if (Levels[0])
        {
            targetFish[0].TargetFish = 0;
            targetFish[0].TargetFishCounts = 4;
        }
        else if (Levels[1])
        {
            targetFish[0].TargetFish = 0;
            targetFish[0].TargetFishCounts = 5;
        }
        else if (Levels[2])
        {
            targetFish[0].TargetFish = 0;
            targetFish[1].TargetFish = 1;
            targetFish[0].TargetFishCounts = 3;
            targetFish[1].TargetFishCounts = 3;
        }
        else if (Levels[3])
        {
            targetFish[0].TargetFish = 0;
            targetFish[1].TargetFish = 1;
            targetFish[0].TargetFishCounts = 3;
            targetFish[1].TargetFishCounts = 5;
        }
        else if (Levels[4])
        {
            targetFish[0].TargetFish = 0;
            targetFish[1].TargetFish = 1;
            targetFish[2].TargetFish = 2;

            targetFish[0].TargetFishCounts = 2;
            targetFish[1].TargetFishCounts = 5;
            targetFish[2].TargetFishCounts = 3;

        }
        else if (Levels[5])
        {
            targetFish[0].TargetFish = 1;
            targetFish[1].TargetFish = 2;

            targetFish[0].TargetFishCounts = 3;
            targetFish[1].TargetFishCounts = 5;
        }
        else if (Levels[6])
        {
            targetFish[0].TargetFish = 1;
            targetFish[1].TargetFish = 2;
            targetFish[2].TargetFish = 3;

            targetFish[0].TargetFishCounts = 3;
            targetFish[1].TargetFishCounts = 3;
            targetFish[2].TargetFishCounts = 2;
        }
        else if (Levels[7])
        {
            targetFish[0].TargetFish = 2;
            targetFish[1].TargetFish = 3;            

            targetFish[0].TargetFishCounts = 5;
            targetFish[1].TargetFishCounts = 3;            
        }
        else if (Levels[8])
        {
            targetFish[0].TargetFish = 2;
            targetFish[1].TargetFish = 3;

            targetFish[0].TargetFishCounts = 3;
            targetFish[1].TargetFishCounts = 5;
        }

    }

    //레벨업 파티클
    public void LevelUp(int level)
    {
        playerMoveScript.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
        levelEffectPrefab[0].SetActive(true);
        levelEffectPrefab[1].SetActive(true);
        Levels[level] = true;
    }

    //코루틴 함수
    private IEnumerator SpawnEnemy()
    {
        //게임이 종료될때까지계속반복
        while (!IsGameOver)
        {
            GameObject fish = poolManager.Get(0);
            //1초에서 5초사이 실수값으로 랜덤하게 등장
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        }
    }

    // TODO: score에 따라 적을 선택하는 로직 구현
    int SelectEnemy()
    {
        if (Levels[7] || Levels[8])
            return Random.Range(2, 3);

        else if (Levels[6])
            return Random.Range(1, 3);

        else if (Levels[5])
            return Random.Range(1, 2);

        else if (Levels[4])
            return Random.Range(0, 2);

        else if (Levels[2] || Levels[3])
            return Random.Range(0, 1);

        else // 레벨 0 ~ 1 일때
            return 0;
    }
}
