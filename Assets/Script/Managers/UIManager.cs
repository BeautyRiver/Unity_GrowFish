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
    //��������
    public TextMeshProUGUI scoreText;

    //��Ʈ ����
    [SerializeField] private Slider healthBarSlider;

    [SerializeField] private Text bestScore;
    [SerializeField] private Text lastScore;

    // ����� ����
    [Header("����� ��ǥ ����")]
    [SerializeField] private Sprite[] fishImages; // �� �̹������� �Ҵ��ҰŶ�� ��      
    [SerializeField] private TextMeshProUGUI[] fishTargetText; // ��ǥ ����� ����
    [SerializeField] private Image[] fishTargetImg; // �̹��� �Ҵ�� ���

    //GamePause����
    public bool isPauseScreenOn = false;
    [SerializeField] private GameObject pauseBtns;
    [SerializeField] private GameObject joyStick;

    //GameOver����
    //[SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject gameOverPanel;

    private void Update()
    {
        for (int i = 0; i < fishTargetImg.Length; i++)
        {
            if (Instance.missionTargets.TryGetValue(Instance.CurrentMission, out List<TargetFishInfo> targetFishList))
            {
                // �̼��� Ÿ�� ����⺸�� �ε����� ũ�� �� �̻� ǥ���� ����Ⱑ ���ٴ� �ǹ��̹Ƿ� UI�� ��Ȱ��ȭ
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
                    // Ÿ�� ������� �̹����� ���� ������ UI�� ����
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
        healthBarSlider.DOValue(healthPercentage, 0.5f); // 0.5�� ���� ��ǥ������ �ε巴�� �̵��մϴ�.        
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
        blackScreen.GetComponent<SpriteRenderer>().DOFade(180 / 255f, 0.5f).SetDelay(0.1f); //0.7��ŭ ��Ӱ�
        finalWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetDelay(0.5f);
        if (GameManager.Instance.Score > PlayerPrefs.GetInt("BS"))
        {
            PlayerPrefs.SetInt("BS", GameManager.Instance.Score);
        }
        lastScore.text = GameManager.Instance.Score.ToString();
        bestScore.text = PlayerPrefs.GetInt("BS").ToString();
    }*/

    //��ư��
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
