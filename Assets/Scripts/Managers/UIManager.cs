using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject abilityBtn; // �ɷ¹�ư

    //GameEnd
    [SerializeField] private GameObject gameEndImg;

    private void Update()
    {
        if(GameManager.Instance.isGameEnd == true)
        {
            gameEndImg.SetActive(true);
            Time.timeScale = 0;
        }

        for (int i = 0; i < fishTargetImg.Length; i++)
        {
            if (GameManager.Instance.missionTargets.TryGetValue(GameManager.Instance.currentMission, out List<TargetFishInfo> targetFishList))
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

    // ���ӿ�����ũ�� ����
    public void OnGameOverScreen()
    {        
        gameOverPanel.SetActive(true);
        abilityBtn.SetActive(false);
    }

    // ���ӿ�����ũ�� ����
    public void OffGameOverScreen()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        abilityBtn.SetActive(true);
    }

    //��ư��
    // �Ͻ����� ��ư
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
    public void InputContinue()
    {
        Time.timeScale = 1;
        joyStick.SetActive(true);
        pauseBtns.gameObject.SetActive(false);
        isPauseScreenOn = false;
        abilityBtn.SetActive(true);
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
