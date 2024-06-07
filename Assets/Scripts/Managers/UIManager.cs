using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    //점수관련
    public TextMeshProUGUI scoreText;

    [Header("체력, 점수 관련")]
    [SerializeField] private Image healthBarSlider;

    [SerializeField] private Text bestScore;
    [SerializeField] private Text lastScore;

    // 물고기 섭취
    [Header("물고기 목표 섭취")]
    public Sprite[] fishImages; // 이 이미지들을 할당할거라는 뜻 (Fish)
    public Sprite[] enemyImages; // 이 이미지들을 할당할거라는 뜻 (Enemy)  
    [SerializeField] private Image[] fishTargetImg; // 이미지 할당될 장소 (Fish)
    [SerializeField] private TextMeshProUGUI[] fishTargetText; // 목표 물고기 개수
    [SerializeField] private Image[] enemyTargetImg; // 이미지 할당될 장소 (Enemy)
    public TextMeshProUGUI nowMissonText; // 현재 미션 단계 텍스트
    //GamePause관련
    public bool isPauseScreenOn = false;
    [SerializeField] private GameObject pauseBtns;
    [SerializeField] private GameObject joyStick;

    [Header("플레이어 능력")]
    [SerializeField] private GameObject abilityBtn; // 능력버튼

    //GameOver관련
    [Header("게임 종료 관련")]
    [SerializeField] private GameObject gameOverPanelBg; // 게임오버 배경
    [SerializeField] private GameObject gameOverPanel; // 게임오버 패널
    [SerializeField] private Button seeAdsButton; // 광고보고 부활하기 버튼
    [SerializeField] private bool isCanAds = true; // 광고 시청 가능 여부 (부활 가능 여부)

    //GameEnd
    [Header("게임 종료(클리어) 관련")]
    [SerializeField] private GameObject gameEndImg; // 게임 종료 이미지

    [Header("사운드 설정UI")]
    public GameObject BlackScreen; // 배경(검은 배경)
    public GameObject SoundSettingScreen; // 사운드 설정 화면
    public Slider sfxSlider; // 효과음 슬라이더
    public Slider bgmSlider; // 배경음 슬라이더

    void Start()
    {        
        SoundManager.Instance.SoundSliderSetting(sfxSlider, bgmSlider); // 사운드 슬라이더 설정
        StartCoroutine(CheckFishTarget());
    }

    // 남은 물고기 이미지 업데이트
    IEnumerator CheckFishTarget()
    {
        while (GameManager.Instance.isGameEnd == false)
        {
            for (int i = 0; i < fishTargetImg.Length; i++)
            {
                if (GameManager.Instance.missionTargets.TryGetValue(GameManager.Instance.currentMission, out List<TargetFishInfo> targetFishList)) // 미션의 타겟 물고기 리스트를 가져옴
                {
                    // 미션의 타겟 물고기보다 인덱스가 크면 더 이상 표시할 물고기가 없다는 의미이므로 UI를 비활성화
                    if (i >= targetFishList.Count)
                    {
                        // 물고기 이미지와 남은 개수를 UI에서 비활성화
                        for (int j = i; j < fishTargetImg.Length; j++)
                        {
                            fishTargetImg[j].gameObject.SetActive(false);
                            fishTargetText[j].gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        // 타겟 물고기의 이미지와 남은 개수를 UI에 설정
                        fishTargetImg[i].sprite = fishImages[targetFishList[i].targetFish];
                        fishTargetText[i].text = targetFishList[i].targetFishCounts.ToString();
                        fishTargetImg[i].gameObject.SetActive(true);
                        fishTargetText[i].gameObject.SetActive(true);
                    }
                }
                yield return null;
            }
        }
        nowMissonText.text = "미션 완료!";
        gameEndImg.SetActive(true);
        Time.timeScale = 0;
    }

    public void EnemyTargetImgChange()
    {
        for (int i = 0; i < enemyImages.Length; i++)
        {
            enemyTargetImg[i].sprite = enemyImages[i];
        }
    }

    public void UpdateHealthBar(float currentHp, int maxHp)
    {
        // fiil amount 감소로 체력 감소 표시
        float targetFillAmount = currentHp / maxHp;
        // 체력바가 부드럽게 감소하도록 DOTween을 사용
        healthBarSlider.DOFillAmount(targetFillAmount, 1f).SetEase(Ease.OutQuad); // SetEase(Ease.OutQuad) : 천천히 감소하도록 설정

    }

    // 게임오버스크린 띄우기
    public void OnGameOverScreen()
    {
        gameOverPanelBg.SetActive(true); // 게임오버 배경 활성화(검은색 투명 배경)        
        gameOverPanel.SetActive(true);  // 게임오버 패널 활성화
        gameOverPanel.transform.localScale = Vector3.zero; // 초기 스케일을 0으로 설정
        gameOverPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack); // 통통 튀는 효과로 등장

        abilityBtn.SetActive(false); // 능력 버튼 비활성화
        seeAdsButton.interactable = isCanAds; // 광고 시청 가능 여부에 따라 버튼 활성화 여부 결정
    }

    // 게임오버스크린 끄기
    public void OffGameOverScreen()
    {
        gameOverPanel.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameOverPanel.SetActive(false);
            gameOverPanelBg.SetActive(false);
        });
        abilityBtn.SetActive(true);
    }

    //버튼들
    // 일시정지 버튼
    public void InputPause()
    {
        if (!isPauseScreenOn)
        {
            Time.timeScale = 0;
            joyStick.SetActive(false);
            pauseBtns.gameObject.SetActive(true);
            isPauseScreenOn = true;
            abilityBtn.SetActive(false);
        }
    }
    // 계속하기 버튼
    public void InputContinue()
    {
        Time.timeScale = 1;
        joyStick.SetActive(true);
        pauseBtns.gameObject.SetActive(false);
        isPauseScreenOn = false;
        abilityBtn.SetActive(true);
    }
    // 메인화면으로 버튼
    public void InputStop()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    // 다시하기 버튼
    public void InputRetry()
    {
        GameManager.Instance.isGameEnd = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 광고보고 부활하기
    public void InputSeeAdsBtn()
    {
        isCanAds = false;
        RewardsBanner.Instance.ShowRewardedAd("Respawn");
    }

    // 사운드 설정 버튼
    public void InputSoundSettingBtn()
    {
        BlackScreen.SetActive(true); // 검은 배경 활성화
        BlackScreen.GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true);

        SoundSettingScreen.SetActive(true); // 사운드 설정 화면 활성화
        SoundSettingScreen.transform.localScale = Vector3.zero; // 초기 스케일을 0으로 설정        
        SoundSettingScreen.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true); // 통통 튀는 효과로 등장
    }

    // 사운드 설정 화면 닫기 버튼
    public void InputSoundSettingCloseBtn()
    {
        BlackScreen.GetComponent<Image>().DOFade(0, 0.5f).OnComplete(() => BlackScreen.SetActive(false)).SetUpdate(true);

        SoundSettingScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
        .OnComplete(() => SoundSettingScreen.SetActive(false)).SetUpdate(true);
    }
}
