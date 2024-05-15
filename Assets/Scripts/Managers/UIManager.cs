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

    //하트 관련
    [SerializeField] private Image healthBarSlider;

    [SerializeField] private Text bestScore;
    [SerializeField] private Text lastScore;

    // 물고기 섭취
    [Header("물고기 목표 섭취")]
    [SerializeField] private Sprite[] fishImages; // 이 이미지들을 할당할거라는 뜻      
    [SerializeField] private TextMeshProUGUI[] fishTargetText; // 목표 물고기 개수
    [SerializeField] private Image[] fishTargetImg; // 이미지 할당될 장소
    public TextMeshProUGUI nowMissonText; // 현재 미션 단계 텍스트
    //GamePause관련
    public bool isPauseScreenOn = false;
    [SerializeField] private GameObject pauseBtns;
    [SerializeField] private GameObject joyStick;

    //GameOver관련
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject abilityBtn; // 능력버튼

    //GameEnd
    [SerializeField] private GameObject gameEndImg;

    private void Update()
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            gameEndImg.SetActive(true);
            Time.timeScale = 0;
        }

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
        }
    }

    public void UpdateHealthBar(float currentHp, int maxHp)
    {
        // fiil amount 감소로 체력 감소 표시
        float targetFillAmount = currentHp / maxHp;
        // 체력바가 부드럽게 감소하도록 DOTween을 사용
        healthBarSlider.DOFillAmount(targetFillAmount, 0.5f).SetEase(Ease.OutQuad); // SetEase(Ease.OutQuad) : 천천히 감소하도록 설정
        
    }

    // 게임오버스크린 띄우기
    public void OnGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        abilityBtn.SetActive(false);
    }

    // 게임오버스크린 끄기
    public void OffGameOverScreen()
    {
        gameOverPanel.SetActive(false);
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
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    // 다시하기 버튼
    public void InputRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

}
