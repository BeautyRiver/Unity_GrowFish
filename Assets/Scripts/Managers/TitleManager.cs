using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject ThemeChangeScreen;
    public RectTransform ThemeItemRectTransform;
    public ThemeSelectManager themeSelectManager;

    // ���� ���� ��ư
    public void InputExit()
    {
        Application.Quit();
    }

    // ���� ���� ��ư
    public void InputGameStart()
    {               
       SceneManager.LoadScene("GameScene_DefaultTheme");                
    }
    // ���� ȭ������ �̵� ��ư
    public void InputStoreBtn()
    {
        SceneManager.LoadScene("Store");
    }
    
    // ���� ȭ������ �̵� ��ư
    public void InputMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
    // �׸� ���� ȭ�� Ȱ��ȭ ��ư
    public void InputThemeScreenOn()
    {      
        themeSelectManager.UpdateThemeMainMenu(); // �׸� ���� ��ư ������Ʈ  
        ThemeChangeScreen.SetActive(true); // ������Ʈ Ȱ��ȭ
        SetPositionX(ThemeItemRectTransform, 0); // rect(��ũ��) �ʱ� ��ġ�� ����         
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

    // �׸� ���� ȭ�� ��Ȱ��ȭ ��ư
    public void InputThemeScreenOff()
    {
        // ���� Ƣ�� ȿ���� �����
        ThemeChangeScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => ThemeChangeScreen.SetActive(false)); // �ִϸ��̼� �Ϸ� �� ��Ȱ��ȭ
    }

    
}
