using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowFish_Ai : FishAi
{
    public bool isAttack;
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
        if (isAttack)
        {
            StartCoroutine(AttackAnimation());
        }
    }
    IEnumerator AttackAnimation()
    {
        anim.SetTrigger("BlowFish_Attack");
        yield return new WaitForSeconds(1.5f);
        isAttack = false;
        anim.SetTrigger("BlowFish_Swim");
    }
}
