using System.Collections;
using UnityEngine;


public class FishLevel_2 : FishAi
{
    float currentVelocity;
    float tempSpeed;
    
    protected override void OnEnable() {
        base.OnEnable();
        StartCoroutine(SleepTime());
    }
    protected override void OnDisable() {
        StopCoroutine(SleepTime());
    }

    IEnumerator SleepTime()
    {
        tempSpeed = moveSpeed;
        while (true)
        {
            moveSpeed = Mathf.SmoothDamp(moveSpeed, 0f, ref currentVelocity, 0.1f);
            Debug.Log("감속중");
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            moveSpeed = Mathf.SmoothDamp(moveSpeed, tempSpeed, ref currentVelocity, 0.3f);
            Debug.Log("가속중");

            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
    }
}