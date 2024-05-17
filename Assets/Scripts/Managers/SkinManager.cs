using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : Singleton<SkinManager>
{
    public bool[] isFishSkinUnlock;

    [Header("�׸����� ���� 0: default 1: paper 2: halloween")]
    public int idx = 0;
    public RuntimeAnimatorController[] fishAnimtor;
    public RuntimeAnimatorController currentFishAnimtor;
    [Header("�÷��̾� �ִϸ�����")]
    public RuntimeAnimatorController[] playerAnimtor;    
    public RuntimeAnimatorController currentPlayerAnimator;

    [Header("����� ��������Ʈ")]
    public List<FishSkin> fishSprites;

    [Header("��� ��������Ʈ")]
    public List<BgSkin> bgSprites;

    [Header("������Ʈ")]
    public PlayerMove player;
    public UIManager uIManager;
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
                SetSkin(idx);
                break;
            }
            idx++;
        }
    }
    public void SetSkin(int themeIdx)
    {   
        // ����� ��Ų ����             
        currentFishAnimtor = fishAnimtor[themeIdx];
        // �÷��̾� �ִϸ����� ����        
        currentPlayerAnimator = playerAnimtor[themeIdx];
        player.playerAni.runtimeAnimatorController = currentPlayerAnimator; 

        //����� �Ҵ� �̹��� ����
        uIManager.fishImages = fishSprites[themeIdx].fs.ToArray();

        // ��� ��Ų ����
        GameObject[] bgObj = GameObject.FindGameObjectsWithTag("Bg");
        foreach (var obj in bgObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[themeIdx].bg[1];
        }

        // ��� ��� ����
        GameObject[] bgDecoObj = GameObject.FindGameObjectsWithTag("BgDeco1");
        foreach (var obj in bgDecoObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[themeIdx].bg[2];
        }

        // ��� ��� ����2
        GameObject[] bgDecoObj2 = GameObject.FindGameObjectsWithTag("BgDeco2");
        foreach (var obj in bgDecoObj2)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[themeIdx].bg[3];
        }

        // ����� ��Ų ����
        GameObject[] bubbleObj = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (var obj in bubbleObj)
        {
            obj.GetComponent<SpriteRenderer>().sprite = bgSprites[themeIdx].bg[0];
        }        
    }
}


