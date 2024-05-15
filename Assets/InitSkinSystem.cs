using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class InitSkinSystem : MonoBehaviour
{
    [Header("물고기들 애니메이터 0: 기본, 1: 종이, 2: 할로윈")]
    public AnimatorController[] fishAnimtor;
    [Header("플레이어 애니메이터 0: 기본, 1: 종이, 2: 할로윈")]
    public AnimatorController[] playerAnimtor;
    
    [Header("물고기 스프라이트")]
    public List<FishSkin> fishSprites;

    [Header("배경 스프라이트")]
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