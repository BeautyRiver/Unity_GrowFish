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
        // 카메라가 플레이어를 따라다니지만 y축이 5.2에서 -6.36 사이로 고정되는 코드
        Vector3 targetPosition = new Vector3(player.position.x, Mathf.Clamp(player.position.y, -6.36f, 5.2f), transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        
    }
}
