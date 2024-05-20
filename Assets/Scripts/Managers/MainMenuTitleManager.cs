using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuTitleManager : MonoBehaviour
{
    public GameObject ThemeChangeScreen;
    public RectTransform ThemeItemRectTransform;
    public ThemeSelectManager themeSelectManager;

    private void Start()
    {
        SoundManager.Instance.ChangePlayListClip("MainMenu_bgm");
    }
    // ���� ���� ��ư
    public void InputExitBtn()
    {
        Application.Quit();
    }

    // ���� ���� ��ư
    public void InputGameStartBtn()
    {               
       SceneManager.LoadScene("GameScene_DefaultTheme");                
    }
    // ���� ȭ������ �̵� ��ư
    public void InputStoreBtn()
    {
        SceneManager.LoadScene("Store");
    }
    
    // �׸� ���� ȭ�� Ȱ��ȭ ��ư
    public void InputThemeScreenOnBtn()
    {      
        themeSelectManager.UpdateThemeMainMenu(); // �׸� ���� ��ư ������Ʈ  
        ThemeChangeScreen.SetActive(true); // ������Ʈ Ȱ��ȭ
        SetPositionX(ThemeItemRectTransform, 0); // rect(��ũ��) �ʱ� ��ġ�� ����         
        ThemeChangeScreen.transform.localScale = Vector3.zero; // �ʱ� �������� 0���� ����
        // ���� Ƣ�� ȿ���� ����
        ThemeChangeScreen.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }
    
    // rect(��ũ��) ��ġ ����
    public void SetPositionX(RectTransform rectTransform, float newX)
    {
        Vector2 newPosition = rectTransform.anchoredPosition;
        newPosition.x = newX;
        rectTransform.anchoredPosition = newPosition;
    }

    // �׸� ���� ȭ�� ��Ȱ��ȭ ��ư
    public void InputThemeScreenOffBtn()
    {
        // ���� Ƣ�� ȿ���� �����
        ThemeChangeScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => ThemeChangeScreen.SetActive(false)); // �ִϸ��̼� �Ϸ� �� ��Ȱ��ȭ
    }

    
}
