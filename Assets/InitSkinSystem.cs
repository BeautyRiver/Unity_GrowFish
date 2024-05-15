using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class InitSkinSystem : MonoBehaviour
{
    [Header("������ �ִϸ����� 0: �⺻, 1: ����, 2: �ҷ���")]
    public AnimatorController[] fishAnimtor;
    [Header("�÷��̾� �ִϸ����� 0: �⺻, 1: ����, 2: �ҷ���")]
    public AnimatorController[] playerAnimtor;
    
    [Header("����� ��������Ʈ")]
    public List<FishSkin> fishSprites;

    [Header("��� ��������Ʈ")]
    public List<BgSkin> bgSprites;
}

[System.Serializable]
public class FishSkin
{
    public string name;
    public List<Sprite> fishSprites;
}

[System.Serializable]
public class BgSkin
{
    public string name;
    public Sprite bubble;
    public Sprite bg;
    public Sprite fFloorDeco1;
    public Sprite fFloorDeco2;
}