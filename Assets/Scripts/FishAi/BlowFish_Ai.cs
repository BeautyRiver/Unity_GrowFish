using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowFish_Ai : FishAI
{
    public bool isAttack;
    new private void Awake()
    {
        base.Awake(); 
    }

    new private void Update()
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
