using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject ThemeChangeScreen;
    public RectTransform ThemeItemRectTransform;
    public ThemeSelectManager themeSelectManager;

    // 게임 종료 버튼
    public void InputExit()
    {
        Application.Quit();
    }

    // 게임 시작 버튼
    public void InputGameStart()
    {               
       SceneManager.LoadScene("GameScene_DefaultTheme");                
    }
    // 상점 화면으로 이동 버튼
    public void InputStoreBtn()
    {
        SceneManager.LoadScene("Store");
    }
    
    // 메인 화면으로 이동 버튼
    public void InputMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
    // 테마 변경 화면 활성화 버튼
    public void InputThemeScreenOn()
    {      
        themeSelectManager.UpdateThemeMainMenu(); // 테마 선택 버튼 업데이트  
        ThemeChangeScreen.SetActive(true); // 오브젝트 활성화
        SetPositionX(ThemeItemRectTransform, 0); // rect(스크롤) 초기 위치로 설정         
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

    // 테마 변경 화면 비활성화 버튼
    public void InputThemeScreenOff()
    {
        // 통통 튀는 효과로 사라짐
        ThemeChangeScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => ThemeChangeScreen.SetActive(false)); // 애니메이션 완료 후 비활성화
    }

    
}
