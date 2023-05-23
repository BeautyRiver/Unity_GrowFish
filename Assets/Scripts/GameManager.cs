using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//GAME MANAGER SCRIPT
public class GameManager : MonoBehaviour
{
    public GameObject[] enemyFishs; //�������� ���� �迭
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public float xRange, yRange;    //x,y�� ���� ����
    public int hp = 0;
    public int score = 0;   //Score    

    public float ranSpawnPt;

    float spawnX, spawnY;   //�� ���� ��ġ

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    //�ڷ�ƾ �Լ�
    IEnumerator SpawnEnemy()
    {
        //������ ����ɶ�������ӹݺ�
        while (true)
        {
            int enemyRandom = SelectEnemy();
            ranSpawnPt = Random.Range(0, 2.0f);
            
            if (ranSpawnPt > 1.0f)
            {
                spawnX = xRange;    //�������� ������
            }
            else
            {
                spawnX = -xRange;   //�������� ����
            }

            spawnY = Random.Range(-yRange, yRange);

            //������ ���� ��ȯ�Ǵ� ���� ����

            if (enemyFishs[enemyRandom]!= null) 
                Instantiate(enemyFishs[enemyRandom], new Vector3(spawnX, spawnY, 0), Quaternion.identity);


            //1�ʿ��� 5�ʻ��� �Ǽ������� �����ϰ� ����
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    // TODO: score�� ���� ���� �����ϴ� ���� ����
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
