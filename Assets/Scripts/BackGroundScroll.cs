using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public Transform player; // 플레이어 위치
    public GameObject background1; // 배경 1
    public GameObject background2; // 배경 2

    public GameObject bgDeco1; // 배경 장식 1
    public GameObject bgDeco2; // 배경 장식 2
    private float backgroundWidth; // 배경의 너비
    private float lastPlayerX; // 이전 프레임에서 플레이어의 x 위치

    private void Start()
    {
        // 배경의 너비를 계산합니다 (가정: 배경 스프라이트의 너비 사용)
        backgroundWidth = background1.GetComponent<SpriteRenderer>().bounds.size.x;
        // 플레이어의 초기 x 위치를 저장
        lastPlayerX = player.position.x;
    }

    private void FixedUpdate()
    {
        // 플레이어의 이동 방향 판단 (오른쪽 또는 왼쪽 이동)
        bool movingRight = (player.position.x > lastPlayerX);

        // 플레이어가 오른쪽으로 이동하며 배경 1의 중간을 지나갔을 경우
        if (movingRight && player.position.x >= background1.transform.position.x)
        {
            // 배경 2를 배경 1의 오른쪽으로 이동
            background2.transform.position = new Vector3(background1.transform.position.x + backgroundWidth, background1.transform.position.y, background1.transform.position.z);
            bgDeco2.transform.position = background2.transform.position;
        }
        // 플레이어가 왼쪽으로 이동하며 배경 1의 중간을 지나갔을 경우
        else if (!movingRight && player.position.x <= background1.transform.position.x)
        {
            // 배경 2를 배경 1의 왼쪽으로 이동
            background2.transform.position = new Vector3(background1.transform.position.x - backgroundWidth, background1.transform.position.y, background1.transform.position.z);
            bgDeco2.transform.position = background2.transform.position;
        }

        // 플레이어가 오른쪽으로 이동하며 배경 1의 중간을 지나갔을 경우
        if (movingRight && player.position.x >= background2.transform.position.x)
        {
            // 배경 2를 배경 1의 오른쪽으로 이동
            background1.transform.position = new Vector3(background2.transform.position.x + backgroundWidth, background2.transform.position.y, background2.transform.position.z);
            bgDeco1.transform.position = background1.transform.position;
        }
        // 플레이어가 왼쪽으로 이동하며 배경 1의 중간을 지나갔을 경우
        else if (!movingRight && player.position.x <= background2.transform.position.x)
        {
            // 배경 2를 배경 1의 왼쪽으로 이동
            background1.transform.position = new Vector3(background2.transform.position.x - backgroundWidth, background2.transform.position.y, background2.transform.position.z);
            bgDeco1.transform.position = background1.transform.position;
        }

        // 이번 프레임에서의 플레이어 위치를 다음 프레임 비교를 위해 저장
        lastPlayerX = player.position.x;
    }
}
