using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishType
{
    Lv1,
    Lv2,
    Lv3,
    Lv4,
};

public enum EnemyType
{
    BlowFish,
    Shark,
};
public class TargetFishInfo
{
    public int targetFish;
    public int targetFishCounts;

    public TargetFishInfo(int targetFish, int targetFishCounts)
    {
        this.targetFish = targetFish;
        this.targetFishCounts = targetFishCounts;
    }
}
[System.Serializable]
public class EnemySpawnRange  // 적 물고기 스폰 범위 클래스
{
    public EnemyType name;
    public float min;
    public float max;

    public EnemySpawnRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
[System.Serializable]
public class FishSpawnRange // 물고기 스폰 범위 클래스 
{    
    public float min;
    public float max;
    public FishSpawnRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
[System.Serializable]
public class FishSkin
{    
    public string name;
    public List<Sprite> fs;
}
[System.Serializable]
public class BgSkin
{
    public string name;
    public List<Sprite> bg;
}