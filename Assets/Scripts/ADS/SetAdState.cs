using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAdState : MonoBehaviour
{
    public ThemeSelectManager themeSelectManager;

    // private void Awake()
    // {
    //     if (setAdState == RewardsBanner.AdState.Store)
    //     {
    //         for (int idx = 0; idx < unLockButtons.Count; idx++)
    //         {
    //             int capturedIdx = idx; // 로컬 변수로 캡처
    //             unLockButtons[capturedIdx].onClick.AddListener(() => RewardsBanner.Instance.ShowRewardedAd(DataManager.Instance.themeNames[capturedIdx]));
    //         }
    //         RewardsBanner.Instance.adState = setAdState;
    //     }
    //     else if (setAdState == RewardsBanner.AdState.InGame)
    //     {
    //         respawnButton.onClick.AddListener(() => RewardsBanner.Instance.ShowRewardedAd("Respawn"));
    //     }
    // }

    public void RespawnSawAd()
    {
        RewardsBanner.Instance.ShowRewardedAd("Respawn");
    }

    public void UnlockThemeSawAd(int idx)
    {
        RewardsBanner.Instance.ShowRewardedAd(DataManager.Instance.themeNames[idx]);        
    }

}
