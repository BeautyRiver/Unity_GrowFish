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

    [Header("����� ��ǥ ����")]
    [SerializeField] private TextMeshProUGUI[] fishTargetText;
    [SerializeField] private Image[] fishTargetImg;

    //GamePause����
    public bool isPauseScreenOn = false;
    [SerializeField] private GameObject pauseBtns;
    [SerializeField] private GameObject joyStick;

    //GameOver����
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject finalWindow;

    public void UpdateHealthBar(float currentHp, int maxHp)
    {
        float healthPercentage = currentHp / maxHp;
        healthBarSlider.DOValue(healthPercentage, 0.5f); // 0.5�� ���� ��ǥ������ �ε巴�� �̵��մϴ�.        
    }

    //GameOver
    public void GameOverScreen()
    {
        blackScreen.GetComponent<SpriteRenderer>().DOFade(180 / 255f, 0.5f).SetDelay(0.1f); //0.7��ŭ ��Ӱ�
        finalWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetDelay(0.5f);
        if (GameManager.Instance.score > PlayerPrefs.GetInt("BS"))
        {
            PlayerPrefs.SetInt("BS", GameManager.Instance.score);
        }
        lastScore.text = GameManager.Instance.score.ToString();
        bestScore.text = PlayerPrefs.GetInt("BS").ToString();
    }

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
