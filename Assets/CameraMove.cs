using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    float smoothing = 0.2f;

    
    void FixedUpdate()
    {
        Vector3 targetPos;
        //x축 범위 조정 
        if (player.transform.position.x >= 13.4f)       
            targetPos = new Vector3(13.4f, player.position.y, -10);        
        else if (player.transform.position.x <= -13.4f)
            targetPos = new Vector3(-13.4f, player.position.y, -10);
        else
            targetPos = new Vector3(player.position.x, player.position.y, -10);

        //y축 범위 조정
        if (player.transform.position.y >= 5.7f)
            targetPos = new Vector3(player.position.x, 5.7f, -10);
        else if (player.transform.position.y <= -5.7f)
            targetPos = new Vector3(player.position.x, -5.7f, -10);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
