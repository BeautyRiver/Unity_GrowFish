using System.Collections;
using UnityEngine;


public class FishLevel_2 : FishAi
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
    }

    protected override void OnEnable() {
        base.OnEnable();
        StartCoroutine(VelocityChange());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(VelocityChange());
    }
    IEnumerator VelocityChange()
    {
        while(true)
        {
            float tempSpeed = moveSpeed;
            if (moveSpeed <= 0)
            {
                moveSpeed -= 1f * Time.deltaTime;
            }
            yield return new WaitForSeconds(Random.Range(2f,4f));

            if (moveSpeed >= tempSpeed)
            {
                moveSpeed += 1f * Time.deltaTime;
            }
            yield return new WaitForSeconds(Random.Range(3f,5f));

        }
    }
}