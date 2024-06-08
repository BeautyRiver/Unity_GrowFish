using System.Collections;
using UnityEngine;


public class FishLevel_2 : FishAi
{
    public float randomMin;
    public float randomMax;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(RandomDirX());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }
    protected override void Update()
    {
        base.Update();
    }

    IEnumerator RandomDirX()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(randomMin * 1.2f, randomMax * 1.2f));
            ChangeDirRunningStart();
            yield return new WaitForSeconds(Random.Range(randomMin * 0.5f, randomMax * 0.5f));
            ChangeDirAfterRunning();

        }
    }
    private void ChangeDirRunningStart()
    {
        isRunningAway = true;
        SetReverseX(); // ���� ������ �ݴ�� ����
    }

    private void ChangeDirAfterRunning()
    {
        if (isRunningAway)
        {
            // ���� ���� ���� �� ���� �ݴ�� �ٲٱ�
            isRunningAway = false;
            SetReverseX();

        }
    }

}