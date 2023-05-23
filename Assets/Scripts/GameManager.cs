using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//GAME MANAGER SCRIPT
public class GameManager : MonoBehaviour
{
    public GameObject[] enemyFishs; //적프리팹 관리 배열
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public float xRange, yRange;    //x,y의 범위 지정
    public int hp = 0;
    public int score = 0;   //Score    

    public float ranSpawnPt;

    float spawnX, spawnY;   //적 생성 위치

    void Start()
    {
        StartCoroutine(SpawnEnemy());
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
        /*if (score <= 300)
            return Random.Range(0, 2);
        else if (score <= 1000)
            return Random.Range(0, 3);
        else if (score <= 3000)
        {
            return Random.Range(0, 4);
        }
        else if (score <= 6000)
        {
            return Random.Range(1, 5);
        }
        else return Random.Range(2, 6);*/

        return Random.Range(0, 3);
        
    }
}
