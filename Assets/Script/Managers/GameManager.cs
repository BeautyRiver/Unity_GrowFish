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

    public GameObject[] levelEffectPrefab; // 이펙트
    public UIManager uiManager; // UIManager
    public PlayerMove playerMoveScript;
    public PoolManager poolManager;

    //적 관련 변수들
    public bool isGameOver = false;
    public bool[] levels; //스테이지 레벨관리    
    public float xRange, yRange;    //적 생성 위치 범위     
    public float ranSpawnPt;    //좌,우 생성 결정 변수


    // 점수들
    public int score = 0;
    public int Level_1 = 60;
    public int Level_2 = 500;
    public int Level_3 = 2000;
    public int Level_4 = 4500;

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
    }

    private void Update()
    {
        //체력 체크
        if (!isGameOver)
        {
            if (score >= 45000 && levels[4])
            {
                LevelUp(4);
            }

            else if (score >= 15000 && levels[3])
            {
                LevelUp(3);
            }

            else if (score >= 4000 && levels[2])
            {
                LevelUp(2);
            }

            else if (score >= 500 && levels[1])
            {
                LevelUp(1);
            }

            else if (score >= 0)
                levels[0] = true;
        }
    }

    //레벨업 파티클
    public void LevelUp(int level)
    {
        playerMoveScript.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
        levelEffectPrefab[0].SetActive(true);
        levelEffectPrefab[1].SetActive(true);
        levels[level] = true;
    }

    //코루틴 함수
    private IEnumerator SpawnEnemy()
    {
        //게임이 종료될때까지계속반복
        while (!isGameOver)
        {            
            GameObject fish = poolManager.Get(SelectEnemy());            
            //1초에서 5초사이 실수값으로 랜덤하게 등장
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        }
    }   

    // TODO: score에 따라 적을 선택하는 로직 구현
    int SelectEnemy()
    {  
        if (levels[7] || levels[8])
            return Random.Range(2, 3);

        else if (levels[6])
            return Random.Range(1, 3);

        else if (levels[5])
            return Random.Range(1, 2);

        else if (levels[4])
            return Random.Range(0, 2);

        else if (levels[2] || levels[3])
            return Random.Range(0, 1);      
        
        else // 레벨 0 ~ 1 일때
            return 0;        
    }
}
