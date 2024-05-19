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
    [SerializeField] private Image healthBarSlider;

    [SerializeField] private Text bestScore;
    [SerializeField] private Text lastScore;

    // ����� ����
    [Header("����� ��ǥ ����")]
    public Sprite[] fishImages; // �� �̹������� �Ҵ��ҰŶ�� ��      
    [SerializeField] private TextMeshProUGUI[] fishTargetText; // ��ǥ ����� ����
    [SerializeField] private Image[] fishTargetImg; // �̹��� �Ҵ�� ���
    public TextMeshProUGUI nowMissonText; // ���� �̼� �ܰ� �ؽ�Ʈ
    //GamePause����
    public bool isPauseScreenOn = false;
    [SerializeField] private GameObject pauseBtns;
    [SerializeField] private GameObject joyStick;
    
    [Header("�÷��̾� �ɷ�")]
    [SerializeField] private GameObject abilityBtn; // �ɷ¹�ư

    //GameOver����
    [Header("���� ���� ����")]
    [SerializeField] private GameObject gameOverPanelBg; // ���ӿ��� ���
    [SerializeField] private GameObject gameOverPanel; // ���ӿ��� �г�
    [SerializeField] private Button seeAdsButton; // ������ ��Ȱ�ϱ� ��ư
    [SerializeField] private bool isCanAds = true; // ���� ��û ���� ���� (��Ȱ ���� ����)

    //GameEnd
    [Header("���� ����(Ŭ����) ����")]
    [SerializeField] private GameObject gameEndImg; // ���� ���� �̹���

    void Start()
    {
        StartCoroutine(CheckFishTarget());
    }

    // ���� ����� �̹��� ������Ʈ
    IEnumerator CheckFishTarget()
    {
        while (GameManager.Instance.isGameEnd == false)
        {
            for (int i = 0; i < fishTargetImg.Length; i++)
            {
                if (GameManager.Instance.missionTargets.TryGetValue(GameManager.Instance.currentMission, out List<TargetFishInfo> targetFishList)) // �̼��� Ÿ�� ����� ����Ʈ�� ������
                {
                    // �̼��� Ÿ�� ����⺸�� �ε����� ũ�� �� �̻� ǥ���� ����Ⱑ ���ٴ� �ǹ��̹Ƿ� UI�� ��Ȱ��ȭ
                    if (i >= targetFishList.Count)
                    {
                        // ����� �̹����� ���� ������ UI���� ��Ȱ��ȭ
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
                yield return null;
            }
        }
        nowMissonText.text = "�̼� �Ϸ�!";
        gameEndImg.SetActive(true);
        Time.timeScale = 0;
    }

    public void UpdateHealthBar(float currentHp, int maxHp)
    {
        // fiil amount ���ҷ� ü�� ���� ǥ��
        float targetFillAmount = currentHp / maxHp;
        // ü�¹ٰ� �ε巴�� �����ϵ��� DOTween�� ���
        healthBarSlider.DOFillAmount(targetFillAmount, 1f).SetEase(Ease.OutQuad); // SetEase(Ease.OutQuad) : õõ�� �����ϵ��� ����

    }

    // ���ӿ�����ũ�� ����
    public void OnGameOverScreen()
    {
        gameOverPanelBg.SetActive(true); // ���ӿ��� ��� Ȱ��ȭ(������ ���� ���)        
        gameOverPanel.SetActive(true);  // ���ӿ��� �г� Ȱ��ȭ
        gameOverPanel.transform.localScale = Vector3.zero; // �ʱ� �������� 0���� ����
        gameOverPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack); // ���� Ƣ�� ȿ���� ����

        abilityBtn.SetActive(false); // �ɷ� ��ư ��Ȱ��ȭ
        seeAdsButton.interactable = isCanAds; // ���� ��û ���� ���ο� ���� ��ư Ȱ��ȭ ���� ����
    }

    // ���ӿ�����ũ�� ����
    public void OffGameOverScreen()
    {
        gameOverPanel.transform.DOScale(0,0.5f).SetEase(Ease.InBack).OnComplete(() => 
        {
            gameOverPanel.SetActive(false);
            gameOverPanelBg.SetActive(false);
        });                         
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
    // ����ϱ� ��ư
    public void InputContinue()
    {
        Time.timeScale = 1;
        joyStick.SetActive(true);
        pauseBtns.gameObject.SetActive(false);
        isPauseScreenOn = false;
        abilityBtn.SetActive(true);
    }
    // ����ȭ������ ��ư
    public void InputStop()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    // �ٽ��ϱ� ��ư
    public void InputRetry()
    {
        GameManager.Instance.isGameEnd = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ������ ��Ȱ�ϱ�
    public void InputSeeAdsBtn()
    {
        isCanAds = false;
        RewardsBanner.Instance.ShowRewardedAd("Respawn");
    }

}
