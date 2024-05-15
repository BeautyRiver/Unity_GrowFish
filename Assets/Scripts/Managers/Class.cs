using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class EnemySpawnRange
{
    public float min;
    public float max;

    public EnemySpawnRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
[System.Serializable]
public class FishSpawnRange
{
    public float min;
    public float max;

    public FishSpawnRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

public class FishSkin
{
    public string name;
    public List<Sprite> fs;
}

public class BgSkin
{
    public string name;
    public List<Sprite> bg;
}