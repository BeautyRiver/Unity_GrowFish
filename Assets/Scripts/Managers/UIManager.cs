using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using static GameManager;

public class UIManager : MonoBehaviour
{
    //점수관련
    public TextMeshProUGUI scoreText;

    //하트 관련
    [SerializeField] private Slider healthBarSlider;

    [SerializeField] private Text bestScore;
    [SerializeField] private Text lastScore;

    // 물고기 섭취
    [Header("물고기 목표 섭취")]
    [SerializeField] private Sprite[] fishImages; // 이 이미지들을 할당할거라는 뜻      
    [SerializeField] private TextMeshProUGUI[] fishTargetText; // 목표 물고기 개수
    [SerializeField] private Image[] fishTargetImg; // 이미지 할당될 장소

    //GamePause관련
    public bool isPauseScreenOn = false;
    [SerializeField] private GameObject pauseBtns;
    [SerializeField] private GameObject joyStick;

    //GameOver관련
    //[SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject gameOverPanel;

    private void Update()
    {
        for (int i = 0; i < fishTargetImg.Length; i++)
        {
            if (Instance.missionTargets.TryGetValue(Instance.CurrentMission, out List<TargetFishInfo> targetFishList))
            {
                // 미션의 타겟 물고기보다 인덱스가 크면 더 이상 표시할 물고기가 없다는 의미이므로 UI를 비활성화
                if (i >= targetFishList.Count) 
                {
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
        float healthPercentage = currentHp / maxHp;
        healthBarSlider.DOValue(healthPercentage, 0.5f); // 0.5초 동안 목표값으로 부드럽게 이동합니다.        
    }

    public void OnGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }

    public void OffGameOverScreen()
    {
        gameOverPanel.SetActive(false);
    }

    /*//GameOver
    public void GameOverScreen()
    {
        blackScreen.GetComponent<SpriteRenderer>().DOFade(180 / 255f, 0.5f).SetDelay(0.1f); //0.7만큼 어둡게
        finalWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetDelay(0.5f);
        if (GameManager.Instance.Score > PlayerPrefs.GetInt("BS"))
        {
            PlayerPrefs.SetInt("BS", GameManager.Instance.Score);
        }
        lastScore.text = GameManager.Instance.Score.ToString();
        bestScore.text = PlayerPrefs.GetInt("BS").ToString();
    }*/

    //버튼들
    public void InputPause()
    {
        if (!isPauseScreenOn)
        {
            Time.timeScale = 0;
            joyStick.SetActive(false);
            pauseBtns.gameObject.SetActive(true);
            isPauseScreenOn = true;
        }
    }
    public void InputContinue()
    {
        Time.timeScale = 1;
        joyStick.SetActive(true);
        pauseBtns.gameObject.SetActive(false);
        isPauseScreenOn = false;
    }
    public void InputStop()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void InputRetry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
