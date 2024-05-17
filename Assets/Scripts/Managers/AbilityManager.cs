using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public PlayerMove player;
    public Image hideImg;
    public bool isSkill;
    public float currentTime = 0f;

    private void Start()
    {
        hideImg.gameObject.SetActive(false);
        isSkill = false;
    }
    // 대시 버튼 눌렀을때
    public void DashButton()
    {
        if (player.isDashing == false && isSkill == false)
        {
            isSkill = true;
            hideImg.gameObject.SetActive(true);
            StartCoroutine(player.Dash());
            StartCoroutine(SkillCoolTime());
        }
    }

    IEnumerator SkillCoolTime()
    {
        currentTime = 0f;
        hideImg.fillAmount = 1f;

        while(currentTime < player.dashCoolTime)
        {
            currentTime += Time.deltaTime;
            hideImg.fillAmount = 1 - currentTime / player.dashCoolTime;            
            yield return null;
        }
        currentTime = 0f;
        hideImg.fillAmount = 0f;
        isSkill = false;
        hideImg.gameObject.SetActive(false);
    }

}
