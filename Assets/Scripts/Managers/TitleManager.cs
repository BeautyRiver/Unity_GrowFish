using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject ThemeChangeScreen;
    public RectTransform ThemeItemRectTransform;
    public void InputExit()
    {
        Application.Quit();
    }

    public void InputGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void InputStoreBtn()
    {
        SceneManager.LoadScene("Store");
    }
    
    public void InputMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void InputThemeScreenOn()
    {        
        ThemeChangeScreen.SetActive(true); // ������Ʈ Ȱ��ȭ
        SetPositionX(ThemeItemRectTransform, 0);        
        ThemeChangeScreen.transform.localScale = Vector3.zero; // �ʱ� �������� 0���� ����
        // ���� Ƣ�� ȿ���� ����
        ThemeChangeScreen.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    public void SetPositionX(RectTransform rectTransform, float newX)
    {
        Vector2 newPosition = rectTransform.anchoredPosition;
        newPosition.x = newX;
        rectTransform.anchoredPosition = newPosition;
    }

    public void InputThemeScreenOff()
    {
        // ���� Ƣ�� ȿ���� �����
        ThemeChangeScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => ThemeChangeScreen.SetActive(false)); // �ִϸ��̼� �Ϸ� �� ��Ȱ��ȭ
    }

    // ����׿� JSON �ʱ�ȭ
    public void DebugJsonInit()
    {
        foreach (var theme in DataManager.Instance.themeList.themes)
        {
            if (theme.themeName != "Default")
                theme.isOpen = false;
        }
        DataManager.Instance.SaveData();
    }
}
