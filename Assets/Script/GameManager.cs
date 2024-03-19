using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

//GAME MANAGER SCRIPT
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
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

    //이펙트 관련 변수들
    public GameObject levelEffectPrefab;
    public UIManager uiManager;
    public Transform effectGroup;   

    //적 관련 변수들
    public bool isGameOver = false;    
    public bool[] levels = new bool[5]; //스테이지 레벨관리    
    public float xRange, yRange;    //적 생성 위치 범위     
    float spawnX, spawnY;   //적 생성 위치       
    public float ranSpawnPt;    //좌,우 생성 결정 변수

    public GameObject[] enemyFishs; //적프리팹 관리 배열
    public GameObject Player;
    public PlayerMove PlayerMoveScript;

    //적 점수들
    public int ShrimpPt = 60;
    public int SardinePt = 500;
    public int DommyPt = 2000;
    public int TunaPt = 4500;   


    //레벨업 파티클
    public void LevelUp(int level)
    {
        GameObject instantEffectObj = Instantiate(levelEffectPrefab, effectGroup);
        ParticleSystem effect = instantEffectObj.GetComponent<ParticleSystem>();
        PlayerMoveScript.playerScale += 0.3f;
        effect.transform.position = Player.transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
        levels[level] = true;
    }
   

    //코루틴 함수
    IEnumerator SpawnEnemy()
    {
        //게임이 종료될때까지계속반복
        while (true)
        {
            int enemyRandom = SelectEnemy();
            ranSpawnPt = Random.Range(0, 2.0f);
            
            if (ranSpawnPt > 1.0f)
            {
                spawnX = xRange;    //스폰지점 오른쪽
            }
            else
            {
                spawnX = -xRange;   //스폰지점 왼쪽
            }

            spawnY = Random.Range(-yRange, yRange);

            //점수에 따라 소환되는 적들 변경

            if (enemyFishs[enemyRandom]!= null) 
                Instantiate(enemyFishs[enemyRandom], new Vector3(spawnX, spawnY, 0), Quaternion.identity);


            //1초에서 5초사이 실수값으로 랜덤하게 등장
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    // TODO: score에 따라 적을 선택하는 로직 구현
    int SelectEnemy()
    {
        if (levels[4])
            return Random.Range(3, 6);
        else if (levels[3])
            return Random.Range(2, 6);
        else if (levels[2])
            return Random.Range(1, 5);
        else if (levels[1])
            return Random.Range(0, 3);
        else if (levels[0])
            return Random.Range(0, 2);
        else
            return 0;
    }
    void Start()
    {
        DOTween.KillAll();
        StartCoroutine(SpawnEnemy());
    }
   
}
