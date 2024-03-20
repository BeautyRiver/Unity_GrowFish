using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    //��������
    public Text scoreText;
    public int score = 0;

    //��Ʈ ����
    public int hp = 0;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Text bestScore;
    public Text lastScore;

    //GamePause����
    bool isPauseScreenOn = false;
    public GameObject pauseBtns;

    //GameOver����
    public GameObject blackScreen;
    public GameObject finalWindow;

    void Update()
    {
        if (!isPauseScreenOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                pauseBtns.gameObject.SetActive(true);
                isPauseScreenOn = true;
            }
        }

        //ü�� üũ
        if (!gameManager.isGameOver)
        {
            HpCheck();

            if (score >= 45000 && !gameManager.levels[4])
            {
                gameManager.LevelUp(4);
            }

            else if (score >= 15000 && !gameManager.levels[3])
            {
                gameManager.LevelUp(3);
            }

            else if (score >= 4000 && !gameManager.levels[2])
            {
                gameManager.LevelUp(2);
            }

            else if (score >= 500 && !gameManager.levels[1])
            {
                gameManager.LevelUp(1);
            }

            else if (score >= 0)
                gameManager.levels[0] = true;
        }
    }
    //Hp�˻�
    void HpCheck()
    {
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        for (int i = 0; i < hp; i++)
        {
            hearts[i].sprite = fullHeart;
        }

        if (hp <= 0)
        {
            gameManager.isGameOver = true;
            GameOverScreen();
        }
    }

    //GameOver
    void GameOverScreen()
    {
        blackScreen.GetComponent<SpriteRenderer>().DOFade(180 / 255f, 0.5f).SetDelay(0.1f); //0.7��ŭ ��Ӱ�
        finalWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetDelay(0.5f);
        if (score > PlayerPrefs.GetInt("BS"))
        {
            PlayerPrefs.SetInt("BS", score);
        }
        lastScore.text = score.ToString();
        bestScore.text = PlayerPrefs.GetInt("BS").ToString();
    }

    //��ư��
    public void InputContinue()
    {
        Time.timeScale = 1;
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
