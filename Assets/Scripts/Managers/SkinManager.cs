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
    public AnimatorController currentPlayerAnimator;
    [Header("�÷��̾� �ִϸ�����")]
    public AnimatorController[] playerAnimtor;    

    [Header("����� ��������Ʈ")]
    public List<FishSkin> fishSprites;

    [Header("��� ��������Ʈ")]
    public List<BgSkin> bgSprites;

    [Header("������Ʈ")]
    public PlayerMove player;
    protected override void Awake()
    {
        base.Awake();        
    }

    void Start()
    {
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
        // �÷��̾� �ִϸ����� ����        
        currentPlayerAnimator = playerAnimtor[currentTheme];
        player.playerAni.runtimeAnimatorController = currentPlayerAnimator; 

        // ��� ��Ų ����
        GameObject[] bgObj = GameObject.FindGameObjectsWithTag("Bg");
        foreach (var obj in bgObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[currentTheme].bg[1];
        }

        // ��� ��� ����
        GameObject[] bgDecoObj = GameObject.FindGameObjectsWithTag("BgDeco1");
        foreach (var obj in bgDecoObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[currentTheme].bg[2];
        }

        // ��� ��� ����2
        GameObject[] bgDecoObj2 = GameObject.FindGameObjectsWithTag("BgDeco2");
        foreach (var obj in bgDecoObj2)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[currentTheme].bg[3];
        }

        // ����� ��Ų ����
        GameObject[] bubbleObj = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (var obj in bubbleObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[currentTheme].bg[0];
        }        



    }
}


