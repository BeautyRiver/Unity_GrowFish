using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class SkinManager : Singleton<SkinManager>
{

    public bool[] isFishSkinUnlock;

    [Header("테마선택 상태 0: default 1: paper 2: halloween")]
    public int currentTheme = 0;
    public AnimatorController[] fishAnimtor;
    public AnimatorController currentFishAnimtor;
    [Header("플레이어 애니메이터")]
    public AnimatorController[] playerAnimtor;    

    [Header("물고기 스프라이트")]
    public List<FishSkin> fishSprites;

    [Header("배경 스프라이트")]
    public List<BgSkin> bgSprites;

    protected override void Awake()
    {
        base.Awake();
        DataManager dt = DataManager.Instance;
        dt.LoadData();
        // 테마 선택 여부에 따라 다른 씬으로 이동        
        foreach (var data in dt.themeList.themes)
        {
            if (data.isSelect == true)
            {
                SetSkin();
                break;
            }
            currentTheme++;
        }
    }

    private void SetSkin()
    {   
        // 물고기 스킨 설정             
        currentFishAnimtor = fishAnimtor[currentTheme];

        // 물방울 스킨 설정
        GameObject[] bubbleObj = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (var obj in bubbleObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[currentTheme].bg[0];
        }
    }
}


