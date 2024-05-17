using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class FishLevel_1 : FishAi
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        
        // 플레이어가 감지 범위 내에 있고 도망 상태가 아니라면 도망 시작
        if (distanceToPlayer < detectionRadius && !isRunningAway && IsMovingTowardsPlayer())
        {
            anim.SetTrigger("Lv1_Run");
            ChangeDirRunningStart(); // 도망 시작
            Invoke(nameof(ChangeDirAfterRunning), 2f); // 2초 후 방향 변경
        }
    }
    

    // 플레이어 감지 시 도망 처리: 현재 방향의 x 값을 반전하여 반대 방향으로 설정
    private void ChangeDirRunningStart()
    {
        isRunningAway = true;
        SetReverseX(); // 현재 방향을 반대로 변경
    }

    // 도망 상태 종료 후 방향 변경: x축 방향 반전, y축은 새로운 무작위 값으로 설정
    private void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // 도망 상태 종료 후 방향 반대로 바꾸기
            anim.SetTrigger("Lv1_Swim");
            isRunningAway = false;
            SetReverseX();
        }
    }
}
