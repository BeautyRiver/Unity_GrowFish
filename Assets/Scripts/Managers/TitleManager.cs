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
        ThemeChangeScreen.SetActive(true); // 오브젝트 활성화
        SetPositionX(ThemeItemRectTransform, 0);        
        ThemeChangeScreen.transform.localScale = Vector3.zero; // 초기 스케일을 0으로 설정
        // 통통 튀는 효과로 등장
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
        // 통통 튀는 효과로 사라짐
        ThemeChangeScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => ThemeChangeScreen.SetActive(false)); // 애니메이션 완료 후 비활성화
    }

    // 디버그용 JSON 초기화
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
