using UnityEngine;
public class SetAdState : MonoBehaviour
{    
    public void RespawnSawAd()
    {
        RewardsBanner.Instance.ShowRewardedAd("Respawn");
    }
}
