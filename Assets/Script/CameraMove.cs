using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    float smoothing = 0.2f;
    public float maxX;
    public float maxY;

    void FixedUpdate()
    {
        float targetX = player.position.x;
        float targetY = player.position.y;

        // x축 범위 조정
        if (player.position.x >= maxX)
            targetX = maxX;
        else if (player.position.x <= -maxX)
            targetX = -maxX;

        // y축 범위 조정
        if (player.position.y >= maxY)
            targetY = maxY;
        else if (player.position.y <= -maxY)
            targetY = -maxY;

        Vector3 targetPos = new Vector3(targetX, targetY, -10);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
