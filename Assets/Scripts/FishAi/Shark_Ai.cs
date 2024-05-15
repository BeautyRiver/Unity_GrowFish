using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class Shark_Ai : FishAI
{
    public bool findPlayer;
    protected override void Awake()
    {
        base.Awake();
        findPlayer = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RandomSpeed(minSpeed, maxSpeed); // 상어는 속도 3.0~ 4.4 
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (GameManager.Instance.isGameOver)
            isRunningAway = false;

        if (isRunningAway)
        {
            anim.SetTrigger("Shark_Attack");
            // 플레이어 위치를 향한 방향 벡터 계산
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            // 현재 방향 업데이트
            currentDirection = directionToPlayer;

            Invoke(nameof(FindPlayerOff), 3f);
        }
    }

    // 플레이어 감지 종료
    private void FindPlayerOff()
    {
        anim.SetTrigger("Shark_Swim");
        SetRandomY();
        isRunningAway = false;
        //SetReverseX();
    }
}
