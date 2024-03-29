using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class FishLevel_1 : FishAI
{
    new private void Awake()
    {
        base.Awake();
    }

    new private void Update()
    {
        base.Update();

        // 플레이어가 감지 범위 내에 있고 도망 상태가 아니라면 도망 시작
        if (distanceToPlayer < detectionRadius && !isRunningAway && IsMovingTowardsPlayer())
        {
            ChangeDirRunningStart(); // 도망 시작
            Invoke(nameof(ChangeDirAfterRunning), 2f); // 2초 후 방향 변경
        }
    }
    // 플레이어와 같은 방향으로 이동시에는 도망안치게
    protected bool IsMovingTowardsPlayer()
    {
        Vector2 toPlayer = (player.transform.position - transform.position).normalized;
        // Dot Product를 사용하여 방향을 체크
        // Dot Product가 양수라면 물고기는 플레이어를 향해 이동 중
        return Vector2.Dot(toPlayer, currentDirection) > 0;
    }

    // 플레이어 감지 시 도망 처리: 현재 방향의 x 값을 반전하여 반대 방향으로 설정
    private void ChangeDirRunningStart()
    {
        isRunningAway = true;
        currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.1f, 0.1f)); // 현재 방향을 반대로 변경
        spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
    }

    // 도망 상태 종료 후 방향 변경: x축 방향 반전, y축은 새로운 무작위 값으로 설정
    private void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // 도망 상태 종료 후 방향 반대로 바꾸기
            isRunningAway = false;
            currentDirection = new Vector2(-currentDirection.x, Random.Range(-0.1f, 0.1f)).normalized;
            spriteRenderer.flipX = currentDirection.x > 0; // 이동 방향에 따라 스프라이트 뒤집기
        }
    }
}
