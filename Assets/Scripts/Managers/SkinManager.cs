using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class SkinManager : Singleton<SkinManager>
{

    public bool[] isFishSkinUnlock;

    [Header("�׸����� ���� 0: default 1: paper 2: halloween")]
    public int currentTheme = 0;
    public AnimatorController[] fishAnimtor;
    public AnimatorController currentFishAnimtor;
    [Header("�÷��̾� �ִϸ�����")]
    public AnimatorController[] playerAnimtor;    

    [Header("����� ��������Ʈ")]
    public List<FishSkin> fishSprites;

    [Header("��� ��������Ʈ")]
    public List<BgSkin> bgSprites;

    protected override void Awake()
    {
        base.Awake();
        DataManager dt = DataManager.Instance;
        dt.LoadData();
        // �׸� ���� ���ο� ���� �ٸ� ������ �̵�        
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
        // ����� ��Ų ����             
        currentFishAnimtor = fishAnimtor[currentTheme];

        // ����� ��Ų ����
        GameObject[] bubbleObj = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (var obj in bubbleObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[currentTheme].bg[0];
        }
    }
}


