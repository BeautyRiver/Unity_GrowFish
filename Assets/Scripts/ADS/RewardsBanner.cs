using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class RewardsBanner : Singleton<RewardsBanner>
{   
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string testId = "ca-app-pub-3940256099942544/5224354917";
    private string rewardId = "ca-app-pub-8914383313856846/2222262982";
    [SerializeField] private string _adUnitId = "";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;
    [SerializeField] private PlayerMove player;
    [SerializeField] private bool testMode = true;
    public enum AdState
    {
        None,
        InGame,
        Store,
    };

    public AdState adState;
    protected override void Awake()
    {
        base.Awake();
        if (testMode == true)
        {
            _adUnitId = testId;
        }
        else if (testMode == false)
        {
            _adUnitId = rewardId;
        }
    }
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        LoadRewardedAd();
    }
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterReloadHandler(_rewardedAd);

            });
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // 광고가 전체 화면 콘텐츠를 닫을 때 발생합니다.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // 최대한 빨리 다른 광고를 표시할 수 있도록 광고를 다시 로드하세요.
            LoadRewardedAd();
        };
        // 광고가 전체 화면 콘텐츠를 열지 못했을 때 발생합니다.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // 최대한 빨리 다른 광고를 표시할 수 있도록 광고를 다시 로드하세요.
            LoadRewardedAd();
        };
    }

    // 버튼 누르면 광고 실행
    public void ShowRewardedAd(string themeName)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                if (themeName == "Respawn")
                {
                    if (player == null)
                    {
                        player = FindObjectOfType<PlayerMove>();
                    }
                    player.SawAd(); // 광고를 보고 부활

                    Debug.Log($"부활이 완료되었습니다.");
                }
                else
                {
                    // 테마 언락
                    DataManager.Instance.UnLockTheme(themeName);
                    Debug.Log($"{themeName} 테마가 해금되었습니다.");
                }


            });
        }
    }
}
