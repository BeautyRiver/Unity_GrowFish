using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyMove : MonoBehaviour
{        
    public float speed;
    public int checkEnemy;
    GameManager manager;    
    
    Animator animator;
    int moveDir = -1;

    void Awake()
    {
    
        manager = FindObjectOfType<GameManager>();
        animator = GetComponentInChildren<Animator>();

        
    }
    void Start()
    {                               
        for (int i = 0; i < manager.enemyFishs.Length; i++)
        {
            if (checkEnemy == i)
            {
                animator.SetInteger("EnemyValue", i);
                break;
            }
        }
        
        //���ʽ����� ��������Ʈ ���� �ݴ�(���� ������), �������� �̵��Ҽ� �ְ� moveDir ��������
        if (transform.position.x < 0) //���ʽ���
        {
            transform.localScale = new Vector3(-1, 1, 1);
            moveDir = -1;
        }

        else //�����ʽ���
        {
            transform.localScale = new Vector3(1, 1, 1);
            moveDir = 1;
        }
    }     
    
    void Update()
    {        
        //moveDir�� ���� ���� ������ �̵� ����
        transform.Translate(Vector3.left * moveDir * speed * Time.deltaTime,Space.World);
                     

        //x���� ����� �� ����
        if (transform.position.x > manager.xRange + 1|| transform.position.x < -(manager.xRange) - 1)
            Destroy(gameObject);
    }

    
    
}
