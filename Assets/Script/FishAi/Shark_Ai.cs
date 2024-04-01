using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class Shark_Ai : FishAI
{
    public bool findPlayer;
    new private void Awake()
    {
        base.Awake();
        findPlayer = false;
    }
    

    new private void FixedUpdate()
    {
        base.FixedUpdate();
        if (isRunningAway)
        {
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
        SetRandomY();
        isRunningAway = false;
    }
}
