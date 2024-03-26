using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothing = 1f;
    public float maxY;

    void FixedUpdate()
    {
        float targetY = player.position.y;

        // y축 범위 조정
        if (player.position.y >= 2f)
            targetY = 2.3f;
        else if (player.position.y <= -maxY)
            targetY = -maxY;

        Vector3 targetPos = new Vector3(player.transform.position.x, targetY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
